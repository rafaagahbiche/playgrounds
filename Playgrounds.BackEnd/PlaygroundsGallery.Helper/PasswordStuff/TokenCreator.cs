using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlaygroundsGallery.Helper
{
    public class TokenCreator : ITokenManager
    {
        private readonly string _secretKey;
		
		public TokenCreator(string secretKey)
		{
			_secretKey = secretKey;
		}

        public string CreateToken(int userId, string userName, string profilePictureUrl)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
			var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

			// create some claims to put into the token
			List<Claim> claims = GetClaims(userId, userName, profilePictureUrl);

			SecurityTokenDescriptor securityTokenDescriptor = GetSecurityTokenDescriptor(signingCredentials, claims);

			// create the token, digitally sign it, and write it out to a string
			var tokenString = GetTokenString(securityTokenDescriptor);
			return tokenString;
		}        
        
        private string GetTokenString(SecurityTokenDescriptor securityTokenDescriptor)
		{
			var handler = new JwtSecurityTokenHandler();
			var token = handler.CreateJwtSecurityToken(securityTokenDescriptor);
			return handler.WriteToken(token);
		}

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(
			SigningCredentials signingCredentials,
			List<Claim> claims)
		{
			return new SecurityTokenDescriptor()
			{
				Subject = new ClaimsIdentity(claims, "Bearer"),
				SigningCredentials = signingCredentials,
				IssuedAt = DateTime.UtcNow,
				NotBefore = DateTime.UtcNow.AddMinutes(-1),
				Expires = DateTime.UtcNow.AddHours(12)
			};
		}

		private List<Claim> GetClaims(int userId, string userName, string profilePictureUrl)
		{
			return new List<Claim>()
				{
					new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
					new Claim(ClaimTypes.Name, userName),
					new Claim("profilePictureUrl", profilePictureUrl)
				};
		}
    }
}