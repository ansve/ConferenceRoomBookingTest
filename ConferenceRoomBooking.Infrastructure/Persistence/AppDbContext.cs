using Microsoft.EntityFrameworkCore;
using ConferenceRoomBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public DbSet<ConferenceRoom> ConferenceRooms => Set<ConferenceRoom>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<BookingService> BookingServices => Set<BookingService>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
