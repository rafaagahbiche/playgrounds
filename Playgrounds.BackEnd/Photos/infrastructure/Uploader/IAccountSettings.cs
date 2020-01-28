namespace Photos.Infrastructure.Uploader
{
    public interface IAccountSettings
    {
        string Name { get; set; }
        string ApiKey { get; set; }
        string ApiSecret { get; set; }
    }
}