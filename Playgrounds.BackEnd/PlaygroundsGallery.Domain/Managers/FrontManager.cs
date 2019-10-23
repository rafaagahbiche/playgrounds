using AutoMapper;
using PlaygroundsGallery.Domain.Repositories;
using PlaygroundsGallery.Helper;

namespace PlaygroundsGallery.Domain.Managers
{
    public partial class FrontManager : IFrontManager
	{
		private readonly IPhotoRepository _photoRepository;
		private readonly IMemberRepository _memberRepository;
		private readonly IPhotoUploader _photoUploader;
		private readonly IPasswordManager _passwordManager;
		private readonly ITokenManager _tokenManager;
		private readonly IMapper _mapper;

		public FrontManager(
			IPhotoRepository photoRepository, 
			IMemberRepository memberRepository,
			IPhotoUploader photoUploader,
			IPasswordManager passwordManager,
			ITokenManager tokenManager,
			IMapper mapper)
		{
			_tokenManager = tokenManager;
			_photoUploader = photoUploader;
			_passwordManager = passwordManager;
			_photoRepository = photoRepository;
			_memberRepository = memberRepository;
			_mapper = mapper;
		}
	}
}
