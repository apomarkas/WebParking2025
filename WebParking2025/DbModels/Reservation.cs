using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebParking2025.DbModels;

namespace WebParking2025.Models
{
    public class Reservation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("reservation_id")]
        public Guid Id { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [InverseProperty(nameof(User.Reservations))]
        public virtual User User { get; set; }

        [Column("place_id")]
        public int PlaceId { get; set; }

        [InverseProperty(nameof(Place.Reservations))]
        public virtual Place Place { get; set; }
        
        [Column("reservation_start")]
        public DateTime ReservationStart { get; set; }

        [Column("reservation_stop")]
        public DateTime ReservationEnd { get; set;}

        [Column("brand")]
        public string Brand { get; set; }

        [Column("license_plate")]
        public string Plate {  get; set; }

        [Column("car_color")]
        public string CarColor {  get; set; }

        [Column("completed")]
        public bool Completed { get; set; }

        [InverseProperty(nameof(ReservationLogs.Reservation))]
        public virtual ICollection<ReservationLogs> Logs { get; set; }


    }
}
