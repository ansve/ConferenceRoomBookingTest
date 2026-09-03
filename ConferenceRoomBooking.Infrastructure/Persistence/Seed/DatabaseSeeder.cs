using ConferenceRoomBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Infrastructure.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (await context.ConferenceRooms.AnyAsync() ||
                await context.Services.AnyAsync())
            {
                return;
            }

            var projector = new Service(
                "Проектор",
                500);

            var wiFi = new Service(
                "Wi-Fi",
                300);

            var sound = new Service(
                "Звук",
                700);

            var roomA = new ConferenceRoom(
                "Зал A",
                50,
                2000);

            var roomB = new ConferenceRoom(
                "Зал B",
                100,
                3500);

            var roomC = new ConferenceRoom(
                "Зал C",
                30,
                1500);

            roomA.Services.Add(projector);
            roomA.Services.Add(wiFi);
            roomA.Services.Add(sound);

            roomB.Services.Add(projector);
            roomB.Services.Add(wiFi);
            roomB.Services.Add(sound);

            roomC.Services.Add(projector);
            roomC.Services.Add(wiFi);
            roomC.Services.Add(sound);

            await context.Services.AddRangeAsync(
                projector,
                wiFi,
                sound);

            await context.ConferenceRooms.AddRangeAsync(
                roomA,
                roomB,
                roomC);

            await context.SaveChangesAsync();
        }
    }
}
