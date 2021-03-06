using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using PlaygroundsGallery.Domain.DTOs;
using PlaygroundsGallery.Domain.Models;
using PlaygroundsGallery.Domain.Repositories;

namespace PlaygroundsGallery.Domain.Managers
{
    public class PlaygroundManager : IPlaygroundManager
    {
        private readonly IRepository<Playground> _playgroundRepository;
		private readonly IRepository<Location> _locationRepository;
		private readonly IRepository<CheckIn> _checkInRepository;
		// private readonly IRepository<ProfilePicture> _profilePictureRepository;
		private readonly IMapper _mapper;

        public PlaygroundManager(
            IRepository<Playground> playgroundRepository,
            IRepository<Location> locationRepository,
            IRepository<CheckIn> checkInRepository,
            // IRepository<ProfilePicture> profilePictureRepository,
            IMapper mapper)
        {
            this._playgroundRepository = playgroundRepository;
            this._locationRepository = locationRepository;
            this._checkInRepository = checkInRepository;
            // this._profilePictureRepository = profilePictureRepository;
            this._mapper = mapper;
        }
        public async Task<IEnumerable<LocationDto>> GetAllLocations()
            => _mapper.Map<IEnumerable<LocationDto>>(await _locationRepository.GetAll());
        

        public async Task<IEnumerable<PlaygroundDto>> GetAllPlaygroundsByLocation(int locationId)
        {
            var playgrounds = await _playgroundRepository
                    .Find(predicate: p => p.LocationId == locationId, 
                          includeProperties: new Expression<Func<Playground, object>>[]{ (p => p.Location), (p => p.Photos) });
            
            return _mapper.Map<IEnumerable<PlaygroundDto>>(playgrounds);
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

        public async Task<CheckinDto> GetCheckInById(int checkInId) 
            => _mapper.Map<CheckinDto>(await _checkInRepository.Get(checkInId));        
        
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

        public async Task<PlaygroundDto> GetPlaygroundById(int playgroundId) 
        {
            var playground = await _playgroundRepository.SingleOrDefault(
                                        predicate: p => p.Id == playgroundId, 
                                        includeProperties: new Expression<Func<Playground, object>>[]
                                                { (p => p.Location), (p => p.Photos) });
            return _mapper.Map<PlaygroundDto>(playground);
        }
    }
}