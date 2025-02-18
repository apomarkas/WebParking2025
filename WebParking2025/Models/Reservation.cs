using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

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
        
        [Column("reservation_date")]
        public DateTime ReservationDate { get; set; }




    }
}
