using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PlaygroundsGallery.DataEF;
using PlaygroundsGallery.DataEF.Models;

namespace Checkins.Services
{
    public class MemberCheckinService : IMemberCheckinService
    {
        private readonly GalleryContext _context;
		private readonly IMapper _mapper;
        private readonly ILogger<MemberCheckinService> _logger;
        public MemberCheckinService(
            GalleryContext context,
            IMapper mapper,
            ILogger<MemberCheckinService> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }
        
        public async Task<CheckinDto> GetCheckInById(int checkinId) 
            => _mapper.Map<CheckinDto>(await _context.CheckIns.FindAsync(checkinId));        
        
        public async Task<bool> CancelCheckinToPlaygroundAsync(int checkinId)
        {
            bool cancelSucceeded = false;
            try
            {
                var checkin = await this._context.CheckIns.FindAsync(checkinId);
                this._context.CheckIns.Remove(checkin);
                cancelSucceeded = await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Error occurred when cancelling checkin id {checkinId}.");
            }

            return cancelSucceeded;
        }

        public async Task<CheckinDto> CheckinToPlaygroundAsync(CheckInForCreationDto checkInForCreation)
        {
            CheckinDto checkinDto = null;
            var checkIn = _mapper.Map<CheckIn>(checkInForCreation);
            bool addSucceeded = false;
            try 
            {
                checkIn.Created = DateTime.UtcNow;
                await _context.CheckIns.AddAsync(checkIn);
                addSucceeded = await _context.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error occurred when adding checkin entity.");
            }

            // Eager load Member and Playground entities
            try
            {
                if (addSucceeded)
                {
                    checkIn = await _context.CheckIns
                                    .Include(c => c.Playground)
                                    .Include(c => c.Member)
                                    .ThenInclude(m => m.ProfilePictures)
                                    .SingleOrDefaultAsync(c => c.Id == checkIn.Id);
                    checkinDto = _mapper.Map<CheckinDto>(checkIn);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Error occurred when extracting the newly added checkin.");
            }

            return checkinDto;
        }
    }
}