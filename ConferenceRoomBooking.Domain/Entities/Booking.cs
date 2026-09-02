using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Domain.Entities
{
    public class Booking
    {
        public Guid Id { get; private set; }

        public Guid ConferenceRoomId { get; private set; }

        public ConferenceRoom ConferenceRoom { get; private set; } = null!;

        public DateTime StartTime { get; private set; }

        public DateTime EndTime { get; private set; }

        public decimal BasePricePerHour { get; private set; }

        public ICollection<BookingService> BookingServices { get; private set; }
            = new List<BookingService>();

        public Booking(Guid conferenceRoomId, DateTime startTime, DateTime endTime, decimal basePricePerHour)
        {
            Id = Guid.NewGuid();
            ConferenceRoomId = conferenceRoomId;
            StartTime = startTime;
            EndTime = endTime;
            BasePricePerHour = basePricePerHour;
        }
    }
}
