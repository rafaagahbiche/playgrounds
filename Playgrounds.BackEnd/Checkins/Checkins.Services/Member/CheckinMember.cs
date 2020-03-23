using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Models;
using PlaygroundsGallery.DataEF.Repositories;

namespace Checkins.Services
{
    public class CheckinMember : ICheckinMember
    {
        private readonly IRepository<CheckIn> _checkInRepository;
        // private readonly IGalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<CheckinMember> _logger;
        public CheckinMember(
            // IGalleryContext context,
            IRepository<CheckIn> checkInRepository,
            IMapper mapper,
            ILogger<CheckinMember> logger)
        {
            this._checkInRepository = checkInRepository;
            // this._context = context;
            this._mapper = mapper;
            this._logger = logger;
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
            bool addSucceeded = false;
            try 
            {
                addSucceeded = await _checkInRepository.Add(checkIn);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error occurred when adding checkin entity.");
            }

            if (addSucceeded)
            {
                var checkinDto = _mapper.Map<CheckinDto>(checkIn);
            }

            // Eager load Member and Playground entities
            try
            {
                checkIn = await _checkInRepository.SingleOrDefault(
                    predicate: c => c.Id == checkIn.Id, 
                    includeProperties: new Expression<Func<CheckIn, object>>[] {(c => c.Member), (c => c.Playground)});
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error occurred when extracting the newly added checkin.");
            }

            return _mapper.Map<CheckinDto>(checkIn);
        }
    }
}