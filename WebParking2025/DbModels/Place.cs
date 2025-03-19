using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebParking2025.Models
{
    public class Place
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("place_id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("parking_id")]
        public int ParkingId { get; set; }

        [InverseProperty(nameof(Parking.Places))]
        public virtual Parking Parking { get; set; }

        [Column("state")]
        public State State { get; set; }

        [InverseProperty(nameof(Reservation.Place))]
        public ICollection<Reservation> Reservations { get; set; }

        [Column("price")]
        public double Price { get; set; }

    }

    public enum State
    {
        Free = 0,
        Reserved = 1,
        Taken =2
    }
}
