using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using WebParking2025.DbModels;

namespace WebParking2025.Models
{
    public class User:IdentityUser
    {       
        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [InverseProperty(nameof(Reservation.User))]
        public virtual ICollection<Reservation> Reservations { get; set; }

        [InverseProperty(nameof(ReservationLogs.User))]
        public virtual ICollection<ReservationLogs> Logs { get; set; }



    }
}
