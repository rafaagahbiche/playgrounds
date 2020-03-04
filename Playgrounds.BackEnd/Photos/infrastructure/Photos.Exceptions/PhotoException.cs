using System;
namespace Photos.Infrastructure.Exceptions
{
    public class PhotoUploadFileEmptyException : Exception
    {
        public PhotoUploadFileEmptyException()
            : base($"The file is empty.")
        {

        }
    }

    public class PhotoUploadToLibraryException : Exception
    {
        public PhotoUploadToLibraryException()
            : base($"An error has occurred while uploading your photo.")
        {

        }
    }

    public class PhotoUpdateException : Exception
    {
        public PhotoUpdateException()
            : base($"An error has occurred while updating your photo.")
        {

        }
    }

    public class PhotoNotFoundException : Exception
    {
        public PhotoNotFoundException(string publicId)
            : base($"A photo with publicId: {publicId} was not found.")
        {

        }
    }
}