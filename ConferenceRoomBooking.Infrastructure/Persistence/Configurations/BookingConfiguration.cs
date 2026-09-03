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
    public class BookingConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(booking => booking.Id);

            builder.Property(booking => booking.StartTime)
                .IsRequired();

            builder.Property(booking => booking.EndTime)
                .IsRequired();

            builder.Property(booking => booking.BasePricePerHour)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasOne(booking => booking.ConferenceRoom)
                .WithMany()
                .HasForeignKey(booking => booking.ConferenceRoomId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
