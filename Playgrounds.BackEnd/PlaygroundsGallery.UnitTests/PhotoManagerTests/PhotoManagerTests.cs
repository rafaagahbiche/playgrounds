using NUnit.Framework;
using Moq;
using PlaygroundsGallery.Helper;
using PlaygroundsGallery.Domain.Repositories;
using PlaygroundsGallery.Domain.Models;
using System.Threading.Tasks;
using System.Linq;

namespace Tests
{
    public class PhotoManagerTests
    {
         internal Mock<IPhotoRepository> MockPhotoRepository { get; set; }
        private FrontManagerSetter _frontManagerSetter;

        [SetUp]
        public void Setup()
        {
            _frontManagerSetter = new FrontManagerSetter();
            MockPhotoRepository = new Mock<IPhotoRepository>();
            PlaygroundTestContext.SetupRepositoryWithData<Photo>(MockPhotoRepository.As<IRepository<Photo>>(), PlaygroundTestContext.Photos);
            _frontManagerSetter.InitFrontManager(null, MockPhotoRepository);
        }

        [Test]
        public async Task DeletePhoto_ExistingPublicId_Success()
        {
            var deletionSucceeded = await _frontManagerSetter.MockFrontManager.DeletePhoto("mmmm", false);
            var deletedPhoto = PlaygroundTestContext.Photos.Where(x => x.PublicId == "mmmm");
            Assert.IsTrue(deletionSucceeded);
            Assert.IsTrue(deletedPhoto.FirstOrDefault().Deleted);
        }
    }
}