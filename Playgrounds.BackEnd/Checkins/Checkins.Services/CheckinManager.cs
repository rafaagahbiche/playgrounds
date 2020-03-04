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

        public async Task<IEnumerable<CheckinDto>> GetCheckInsByPlaygroundIdAsync(int playgroundId)
        {
            return _mapper.Map<IEnumerable<CheckinDto>>(await _checkInRepository.Find(
                predicate: c => c.PlaygroundId == playgroundId, 
                includeProperties: new Expression<Func<CheckIn, object>>[] 
                                    {
                                        (c => c.Member), 
                                        (c => c.Member.ProfilePictures), 
                                        (c => c.Playground)
                                    },
                orderBy: q => q.OrderByDescending(c => c.CheckInDate)));
        }

        public async Task<IEnumerable<CheckinDto>> GetCheckinsByPlaygroundIdByDateAsync(int playgroundId, DateTime dateTime)
        {
            return _mapper.Map<IEnumerable<CheckinDto>>(await _checkInRepository.Find(
                predicate: c => c.PlaygroundId == playgroundId 
                                && c.CheckInDate.Day == dateTime.Day
                                && c.CheckInDate.Month == dateTime.Month
                                && c.CheckInDate.Year == dateTime.Year, 
                includeProperties: new Expression<Func<CheckIn, object>>[] 
                                    {
                                        (c => c.Member), 
                                        (c => c.Member.ProfilePictures), 
                                        (c => c.Playground)
                                    },
                orderBy: q => q.OrderByDescending(c => c.CheckInDate)));
        }
    }
}