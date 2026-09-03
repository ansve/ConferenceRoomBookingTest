using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Infrastructure.Persistence.Configurations
{
    public class BookingServiceConfiguration : IEntityTypeConfiguration<BookingService>
    {
        public void Configure(EntityTypeBuilder<BookingService> builder)
        {
            builder.HasKey(bookingService => bookingService.Id);

            builder.Property(bookingService => bookingService.Price)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasOne<Booking>()
                .WithMany(booking => booking.BookingServices)
                .HasForeignKey(bookingService => bookingService.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Service>()
                .WithMany()
                .HasForeignKey(bookingService => bookingService.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
