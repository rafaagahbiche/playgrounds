using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;

namespace Checkins.Services
{
    public class CheckinManager : ICheckinManager
    {
        private readonly IRepository<CheckIn> _checkInRepository;
		private readonly IMapper _mapper;
        public CheckinManager(
            IRepository<CheckIn> checkInRepository,
            IMapper mapper)
        {
            this._checkInRepository = checkInRepository;
            this._mapper = mapper;
        }

        public async Task<CheckinDto> CheckInToPlayground(CheckInForCreationDto checkInForCreation)
        {
            var checkIn = _mapper.Map<CheckIn>(checkInForCreation);
            await _checkInRepository.Add(checkIn);

            // Eager load Member and Playground entities
            checkIn = await _checkInRepository.SingleOrDefault(
                predicate: c => c.Id == checkIn.Id, 
                includeProperties: new Expression<Func<CheckIn, object>>[] {(c => c.Member), (c => c.Playground)});
            return _mapper.Map<CheckinDto>(checkIn);
        }

        public async Task<CheckinDto> GetCheckInById(int checkinId) 
            => _mapper.Map<CheckinDto>(await _checkInRepository.Get(checkinId));        
        
        public async Task<IEnumerable<CheckinDto>> GetCheckInsByPlaygroundId(int playgroundId)
        {
            return _mapper.Map<IEnumerable<CheckinDto>>(await _checkInRepository.Find(
                predicate: c => c.PlaygroundId == playgroundId, 
                includeProperties: new Expression<Func<CheckIn, object>>[] 
                                    {
                                        (c => c.Member), 
                                        (c => c.Member.ProfilePictures), 
                                        (c => c.Playground)
                                    },
                orderBy: q => q.OrderByDescending(c => c.Created)));
        }
    }
}