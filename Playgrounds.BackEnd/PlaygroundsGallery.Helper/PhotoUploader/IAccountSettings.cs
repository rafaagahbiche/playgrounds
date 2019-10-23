namespace PlaygroundsGallery.Helper
{
    public interface IAccountSettings
    {
        string Name { get; set; }
        string ApiKey { get; set; }
        string ApiSecret { get; set; }
    }
}