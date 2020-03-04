using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;

namespace Checkins.Services
{
    public class CheckinMember : ICheckinMember
    {
        private readonly IRepository<CheckIn> _checkInRepository;
		private readonly IMapper _mapper;
        public CheckinMember(
            IRepository<CheckIn> checkInRepository,
            IMapper mapper)
        {
            this._checkInRepository = checkInRepository;
            this._mapper = mapper;
        }
        public async Task<CheckinDto> GetCheckInById(int checkinId) 
            => _mapper.Map<CheckinDto>(await _checkInRepository.Get(checkinId));        
        
        public async Task<bool> CancelCheckinToPlaygroundAsync(int checkinId)
        {
            var checkin = await this._checkInRepository.Get(checkinId);
            return await this._checkInRepository.Remove(checkin);
        }

        public async Task<CheckinDto> CheckinToPlaygroundAsync(CheckInForCreationDto checkInForCreation)
        {
            var checkIn = _mapper.Map<CheckIn>(checkInForCreation);
            await _checkInRepository.Add(checkIn);

            // Eager load Member and Playground entities
            checkIn = await _checkInRepository.SingleOrDefault(
                predicate: c => c.Id == checkIn.Id, 
                includeProperties: new Expression<Func<CheckIn, object>>[] {(c => c.Member), (c => c.Playground)});
            return _mapper.Map<CheckinDto>(checkIn);
        }
    }
}