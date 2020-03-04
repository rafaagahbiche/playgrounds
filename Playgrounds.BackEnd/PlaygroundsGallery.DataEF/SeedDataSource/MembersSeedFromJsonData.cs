using System.Collections.Generic;
using PlaygroundsGallery.DataEF.Models;
using Newtonsoft.Json;

namespace PlaygroundsGallery.DataEF.Seed
{
    public class MembersSeedFromJsonData
    {
        private readonly GalleryContext _context;
        public MembersSeedFromJsonData(GalleryContext context)
        {
            _context = context;
        }

        public void AddMembersFromJson(string jsonFilePath)
        {
            var memberData = System.IO.File.ReadAllText(jsonFilePath);
            var members = JsonConvert.DeserializeObject<List<Member>>(memberData);
            foreach (var member in members)
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash("password", out passwordHash, out passwordSalt);

                member.PasswordHash = passwordHash;
                member.PasswordSalt = passwordSalt;
                _context.Members.Add(member);
            }
            
            _context.SaveChanges();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            } 
        }
    }
}