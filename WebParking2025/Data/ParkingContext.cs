using Microsoft.EntityFrameworkCore;
using WebParking2025.Models;

namespace WebParking2025.Data
{
    public class ParkingContext: DbContext
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options) { }

        //Tables
        public DbSet<Parking> Parkings { get; set; }

    }
}
