using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebParking2025.Models;

namespace WebParking2025.DbModels
{
    public class ReservationLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("log_id")]
        public string? Id { get; set; }

        [Column("first_name")]
        public string? FirstName { get; set; }

        [Column("last_name")]
        public string? LastName { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("reservation_id")]
        [InverseProperty(nameof(Reservation.Logs))]
        public virtual Reservation? Reservation { get; set; }

        [Column("user_id")]
        [InverseProperty(nameof(User.Logs))]
        public virtual User? User { get; set; }

        [Column("photo")]
        public byte[]? Photo { get; set; }
    }
}
