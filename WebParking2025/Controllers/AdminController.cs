using Humanizer;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Runtime;
using WebParking2025.Data;
using WebParking2025.Models;
using WebParking2025.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebParking2025.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ParkingContext _context;
        private readonly UserManager<User> _userManager;
        public AdminController(ParkingContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Panel()
        {
            return View();
        }

        public async Task<IActionResult> ManageRoles() {

            Dictionary<User,string> userRoles = new Dictionary<User,string>();
            
            var roleEntries = await _context.UserRoles.ToListAsync();
            foreach (var entry in roleEntries)
            {   
               
                var user = _context.Users.Find(entry.UserId);
                var role = _context.Roles.Find(entry.RoleId);
                userRoles.Add(user, role.Name);
            }
          return View(userRoles);
           
        }

        public async Task<IActionResult> ManageReservations()
        {
            var reservations = await _context.Reservation
                .Include(u => u.User)
                .Include(p =>p.Place)
                .Include(par =>par.Place.Parking)
                .Where(c=>c.Completed==false)
                .ToListAsync();

            return View(reservations);
        }

        public async Task<IActionResult> UpdateRoles(UpdateRolesView usersList)
        {   
            foreach(var user in usersList.Info)
            {
                var id = user.UserId;
                var selectedUser = await _userManager.FindByIdAsync(id);
                var currentRoles = await _userManager.GetRolesAsync(selectedUser);
                
                //Remove previous role and give new to the user
                await _userManager.RemoveFromRolesAsync(selectedUser, currentRoles);
                await _userManager.AddToRoleAsync(selectedUser, user.Role);
                               
            }
            
            return RedirectToAction("ManageRoles");
        }

        public async Task<IActionResult> EditReservation(EditReservationView editInfo)
        {   
            var reservation = await _context.Reservation
                .Include(u => u.User)
                .FirstOrDefaultAsync(r => r.Id == editInfo.Id);
            //check if changed date is available
            var reservationStart = DateTime.Parse($"{editInfo.Date} {editInfo.From}");
            var reservationEnd = DateTime.Parse($"{editInfo.Date} {editInfo.To}");           
            var place = await _context.Places
                    .FirstOrDefaultAsync(n => n.Name == editInfo.Place);

            if (place != null)
            {
                reservation.Place = place;
            }
            else
            {
                throw new InvalidOperationException("Cannot assign name without a valid place.");

            }

            if (!isReserved(place, reservationStart, reservationEnd))
            {   
                //update reservation's info
                reservation.Plate = editInfo.Plate;
                reservation.CarColor = editInfo.Colour;
                reservation.Brand = editInfo.CarBrand;
                reservation.ReservationStart = reservationStart;
                reservation.ReservationEnd = reservationEnd;

                _context.Update(reservation);
                await _context.SaveChangesAsync();

                await SendUpdateEmail(reservation);

                return RedirectToAction("ManageReservations");
            }
            else
            {
                return View("UpdateError");
            }                                 
        }

        public async Task<IActionResult> DeleteReservation(Guid Id)
        {
            var reservation = await _context.Reservation
                .Include(u => u.User)
                .FirstOrDefaultAsync(r => r.Id == Id);

            //delete reservation
            _context.Remove(reservation);
            await _context.SaveChangesAsync();

            await SendDeleteEmail(reservation);

            return RedirectToAction("ManageReservations");

        }
        private bool isReserved(Place place, DateTime start, DateTime stop)
        {
            var spotReservations = _context.Reservation
                .Where(p => place == p.Place).ToList();
            foreach (var reservation in spotReservations)
            {
                if (!(stop <= reservation.ReservationStart || start >= reservation.ReservationEnd))
                {
                    return true;
                }
            }

            return false;
        }

        private async Task SendUpdateEmail(Reservation reservation)
        {
            string smtpServer = "smtp.gmail.com";
            int port = 587;
            string fromEmail = "webparkingpapi@gmail.com";
            string appPassword = "yiqj ktkt nzhs xgis";
            string toEmail = reservation.User.Email;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("UNIPI Parking", fromEmail));
                message.To.Add(new MailboxAddress(reservation.User.LastName, toEmail));
                message.Subject = "Reservation update";
                message.Body = new TextPart("plain") { Text = $"Information for reservation  with id:{reservation.Id} were changed!"};

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, port, SecureSocketOptions.StartTls);
                    client.Authenticate(fromEmail, appPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully via Gmail SMTP.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error sending email: {ex.Message}");
            }
        }

        private async Task SendDeleteEmail(Reservation reservation)
        {
            string smtpServer = "smtp.gmail.com";
            int port = 587;
            string fromEmail = "webparkingpapi@gmail.com";
            string appPassword = "yiqj ktkt nzhs xgis";
            string toEmail = reservation.User.Email;
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("UNIPI Parking", fromEmail));
                message.To.Add(new MailboxAddress(reservation.User.LastName, toEmail));
                message.Subject = "Reservation deleted";
                message.Body = new TextPart("plain") { Text = $"Your reservation with id:{reservation.Id} was deleted!" };

                using (var client = new SmtpClient())
                {
                    client.Connect(smtpServer, port, SecureSocketOptions.StartTls);
                    client.Authenticate(fromEmail, appPassword);
                    client.Send(message);
                    client.Disconnect(true);
                }

                Console.WriteLine("Email sent successfully via Gmail SMTP.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error sending email: {ex.Message}");
            }
        }
    }
}
