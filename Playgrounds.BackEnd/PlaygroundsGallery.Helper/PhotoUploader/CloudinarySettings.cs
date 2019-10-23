using PlaygroundsGallery.Helper;

namespace PlaygroundsGallery.Helper
{
    public class CloudinarySettings : IAccountSettings
    {
        public string Name { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
    }
}
