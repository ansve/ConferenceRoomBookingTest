using ConferenceRoomBooking.Application.Interfaces;
using ConferenceRoomBooking.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBooking.Application.Services
{
    public class ConferenceRoomService : IConferenceRoomService
    {
        private readonly IConferenceRoomRepository _conferenceRoomRepository;

        public ConferenceRoomService(IConferenceRoomRepository conferenceRoomRepository)
        {
            _conferenceRoomRepository = conferenceRoomRepository;
        }
        public async Task<List<ConferenceRoom>> GetAllAsync()
        {
            return await _conferenceRoomRepository.GetAllAsync();            
        }
    }
}
