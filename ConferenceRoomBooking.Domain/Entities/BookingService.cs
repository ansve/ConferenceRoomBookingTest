using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Domain.Entities
{
    public class BookingService
    {
        public Guid Id { get; private set; }

        public Guid BookingId { get; private set; }

        public Guid ServiceId { get; private set; }

        public decimal Price { get; private set; }

        public BookingService(Guid serviceId, decimal price)
        {
            Id = Guid.NewGuid();
            ServiceId = serviceId;
            Price = price;
        }
    }
}
