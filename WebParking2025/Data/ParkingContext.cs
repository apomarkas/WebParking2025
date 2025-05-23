﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebParking2025.DbModels;
using WebParking2025.Models;

namespace WebParking2025.Data
{
    public class ParkingContext: IdentityDbContext<User>
    {
        public ParkingContext(DbContextOptions<ParkingContext> options) : base(options) { }

        //Tables
        public DbSet<Parking> Parkings { get; set; }

        public DbSet<Place> Places { get; set; }

        public DbSet<Reservation> Reservation { get; set; }

        public DbSet<IdentityRole> AspNetRoles {  get; set; }

        public DbSet<IdentityUserRole<string>> AspNetUserRoles {  get; set; }

        public DbSet<ReservationLogs> ReservationLogs { get; set; }
        
    }
}
