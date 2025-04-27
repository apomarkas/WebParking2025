using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using WebParking2025.Data;
using WebParking2025.Models;
using WebParking2025.ViewModels;
using Microsoft.EntityFrameworkCore;
using WebParking2025.DbModels;

namespace WebParking2025.Controllers
{
    public class ReservationController : Controller
    {
        
        private readonly UserManager<User> _userManager;
        private readonly ParkingContext _context;
        public ReservationController(UserManager<User> userManager,ParkingContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CompleteReservation(ReservationView reservationView)
        {
            var reservationDate = reservationView.ReservationDate;
            var reservationTime = reservationView.ReservationTime;
            var user = await _userManager.GetUserAsync(User);
            var startDate = DateTime.Parse($"{reservationDate} {reservationTime}");          
            var duration = int.Parse(reservationView.Duration); //take first character
            var stopDate = startDate.AddHours(duration);
            var brand = reservationView.CarBrand;
            var color = reservationView.CarColor;
            var plate = reservationView.LicensePlate;
            var spotId = Convert.ToInt32(reservationView.ParkingSpot);
            var place = _context.Places.Find(spotId);

            //check if spot is free for the given date
            if(isReserved(place, startDate, stopDate))
            {
                return View("ReservationError");
            }
                                 
            var reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                Brand = brand,
                CarColor = color,
                Plate = plate,
                Place = place,
                User = user,
                ReservationStart = startDate,
                ReservationEnd = stopDate,


            };

            //save to db
            try
            {
                _context.Add(reservation);
                _context.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Database update failed: " + ex.Message);
            }

            //send email
            await SendEmail(reservation);
            

            return View(reservation);
        }

        public async Task<IActionResult> ConfirmReservation()
        {   

            return View();
        }

        public async Task<IActionResult> UserReservations(ReservationLogsView info)
        {   
            var user = await _userManager.FindByEmailAsync(info.Email);
            var reservations = await _context.Reservation
                
                .Include(u => u.User)
                .Include(p => p.Place)
                .Include(par=>par.Place.Parking)
                .Where(u => u.User == user && u.Completed==false)
                .ToListAsync();

            using (var memoryStream = new MemoryStream())
            {
                info.Photo.CopyTo(memoryStream);
                byte[] photoBytes = memoryStream.ToArray();
                info.Base64Photo = Convert.ToBase64String(photoBytes);
            }
            info.Reservations = reservations;
            
            return View(info);
        }
        
        private bool isReserved(Place place,DateTime start,DateTime stop)
        {
            var spotReservations = _context.Reservation
                .Where(p => place == p.Place).ToList();
            foreach(var reservation in spotReservations)
            {
                if(!(stop <= reservation.ReservationStart || start >= reservation.ReservationEnd))
                {
                    return true;
                }
            }

            return false;
        }
        public async Task<IActionResult> UserConfirmReservation(ConfirmationView info)

        {   
            var reservation = await _context.Reservation
                .FindAsync(info.ReservationId);
            
            //Complete reservation
            reservation.Completed = true;

            var log = new ReservationLogs
            {
                FirstName = info.FirstName,
                LastName = info.LastName,
                Phone = info.Phone,
                Reservation = reservation,
                User = await _userManager.FindByEmailAsync(info.Email),
                Photo = Convert.FromBase64String(info.Photo)
            };
            _context.ReservationLogs.Add(log);
            await _context.SaveChangesAsync();

            return View("UserReservationSuccess");
        }
        private async Task SendEmail(Reservation reservation)
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
                message.Subject = "Reservation confirmation";
                message.Body = new TextPart("plain") { Text = $"The reservation in our parking for spot {reservation.Place.Name} was successful!\nReservation code:{reservation.Id}" };

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
