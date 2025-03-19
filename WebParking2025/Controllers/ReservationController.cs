using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebParking2025.Data;
using WebParking2025.Models;
using WebParking2025.ViewModels;

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
            //get this dynamically
            var duration = 4;
            var stopDate = startDate.AddHours(duration);
            var brand = reservationView.CarBrand;
            var color = reservationView.CarColor;
            var plate = reservationView.LicensePlate;
            var spotId = Convert.ToInt32(reservationView.ParkingSpot);
            var place = _context.Places.Find(spotId);
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
            _context.Add(reservation);
            _context.SaveChanges();
            return View(reservation);
        }
    }
}
