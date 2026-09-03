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
    public class ConferenceRoomConfiguration : IEntityTypeConfiguration<ConferenceRoom>
    {
        public void Configure(EntityTypeBuilder<ConferenceRoom> builder)
        {
            builder.HasKey(room => room.Id);

            builder.Property(room => room.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(room => room.Capacity)
                .IsRequired();

            builder.Property(room => room.BasePricePerHour)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.HasMany(room => room.Services)
                .WithMany()
                .UsingEntity<Dictionary<string, object>>(
                    "ConferenceRoomService",
                    right => right
                        .HasOne<Service>()
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade),
                    left => left
                        .HasOne<ConferenceRoom>()
                        .WithMany()
                        .HasForeignKey("ConferenceRoomId")
                        .OnDelete(DeleteBehavior.Cascade));
        }
    }
}
