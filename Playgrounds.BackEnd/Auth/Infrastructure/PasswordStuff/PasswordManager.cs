using System;
using System.Text;

namespace Auth.Infrastructure.PasswordStuff
{
    public class PasswordManager : IPasswordManager
    {
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

		public EncryptedPassword SetPasswordHashAndSalt(string password)
		{
			var encryptedPassword = new EncryptedPassword();
			using (var hmac = new System.Security.Cryptography.HMACSHA512())
			{
				encryptedPassword.PasswordSalt = hmac.Key;
				encryptedPassword.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
			}

			return encryptedPassword;
		}

		public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
		{
			bool passwordMatchs = true;
			using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
			{
				var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
				int i = 0;
				while (i < computedHash.Length && passwordMatchs)
				{
					if (computedHash[i] != passwordHash[i])
					{
						passwordMatchs = false;
					}

					i++;
				}
			}

			return passwordMatchs;
		}
    }
}