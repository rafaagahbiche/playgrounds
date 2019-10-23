using System;
using System.Text;

namespace PlaygroundsGallery.Helper
{
    public class PasswordManager : IPasswordManager
    {
		public byte[] PasswordHash { get; set; }
        
        public byte[] PasswordSalt { get; set; }

        public Tuple<string, string> ParseBasicAuthCredential(string credential)
		{
			string password = null;
			var subject = (Encoding.GetEncoding("iso-8859-1").GetString(Convert.FromBase64String(credential)));
			if (string.IsNullOrEmpty(subject))
			{
				return new Tuple<string, string>(null, null);
			}

			if (subject.Contains(":"))
			{
				var index = subject.IndexOf(':');
				password = subject.Substring(index + 1);
				subject = subject.Substring(0, index);
			}

			return new Tuple<string, string>(subject, password);
		}

		public void SetPasswordHashAndSalt(string password)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				PasswordSalt = hmac.Key;
				PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}
		}

		public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				for (int i = 0; i < computedHash.Length; i++)
				{
					if (computedHash[i] != passwordHash[i])
					{
						return false;
					}
				}

				return true;
			}
		}
    }
}