using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Domain.Entities
{
    public class Service
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public decimal Price { get; private set; }

        public Service(string name, decimal price)
        {
            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }
    }
}
