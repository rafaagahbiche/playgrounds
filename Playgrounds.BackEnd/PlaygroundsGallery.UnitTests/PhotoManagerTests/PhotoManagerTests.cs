using NUnit.Framework;
using Moq;
using PlaygroundsGallery.Helper;

namespace Tests
{
    public class PhotoManagerTests
    {
        private Mock<IPhotoUploader> photoUploader;
        
        [SetUp]
        public void Setup()
        {
            photoUploader = new Mock<IPhotoUploader>();
            // photoUploader.SetupSet(x => x.UploadPhoto())
        }

        [Test]
        public void UploadPhoto_SendEmptyFile_Success()
        {
            Assert.Pass();
        }
    }
}