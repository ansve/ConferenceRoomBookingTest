using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Domain.Entities
{
    public class ConferenceRoom
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public int Capacity { get; private set; }

        public decimal BasePricePerHour { get; private set; }

        public ICollection<Service> Services { get; private set; } = new List<Service>();

        public ConferenceRoom(string name, int capacity, decimal basePricePerHour)
        {
            Id = Guid.NewGuid();
            Name = name;
            Capacity = capacity;
            BasePricePerHour = basePricePerHour;
        }

        public void Update(string name, int capacity, decimal basePricePerHour)
        {
            Name = name;
            Capacity = capacity;
            BasePricePerHour = basePricePerHour;
        }
    }
}
