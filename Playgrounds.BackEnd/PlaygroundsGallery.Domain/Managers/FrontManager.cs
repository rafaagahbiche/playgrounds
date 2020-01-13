using AutoMapper;
using PlaygroundsGallery.Domain.Repositories;
using PlaygroundsGallery.Helper;

namespace PlaygroundsGallery.Domain.Managers
{
    public partial class FrontManager : IFrontManager
	{
		private readonly IPhotoRepository _photoRepository;
		private readonly IMapper _mapper;

		public FrontManager(
			IPhotoRepository photoRepository, 
			IMapper mapper)
		{
			_photoRepository = photoRepository;
			_mapper = mapper;
		}
	}
}
