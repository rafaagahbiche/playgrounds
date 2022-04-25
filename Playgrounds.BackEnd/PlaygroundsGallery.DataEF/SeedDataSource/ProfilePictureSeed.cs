using System;
using System.Collections.Generic;
using System.Linq;
using PlaygroundsGallery.DataEF.Models;
namespace PlaygroundsGallery.DataEF
{
    public class ProfilePictureSeed
    {
        private List<ProfilePicture> getProfilePictures() =>
            new List<ProfilePicture>(){
                new ProfilePicture 
                {
                    MemberId = 2,
                    Url = "https://randomuser.me/api/portraits/men/76.jpg"
                },
                new ProfilePicture 
                {
                    MemberId = 1012,
                    Url = "https://randomuser.me/api/portraits/men/14.jpg"
                },
                new ProfilePicture 
                {
                    MemberId = 1019,
                    Url = "https://randomuser.me/api/portraits/men/4.jpg"
                },
                new ProfilePicture 
                {
                    MemberId = 1029,
                    Url = "https://randomuser.me/api/portraits/women/49.jpg"
                }
            };
        
        public bool Seed(GalleryContext context)
        {
            var changesHaveBeenMade = false;
            if (!context.ProfilePictures.Any())
            {
                context.AddRange(getProfilePictures());
                context.SaveChanges();
                changesHaveBeenMade = true;
            }
        
            return changesHaveBeenMade;
        }
    }
}