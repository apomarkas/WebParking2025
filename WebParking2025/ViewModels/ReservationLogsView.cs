using WebParking2025.Models;

namespace WebParking2025.ViewModels
{
    public class ReservationLogsView
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public IFormFile Photo { get; set; }
        public string Base64Photo { get; set; }
        public IEnumerable<Reservation> Reservations { get; set; }

    }
}
