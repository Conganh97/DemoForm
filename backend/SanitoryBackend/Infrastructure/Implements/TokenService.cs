using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Shared.DTOs.Configurations;
using Common.Shared.DTOs.Identity;
using Contracts.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Implements
{
    public class TokenService : ITokenService
    {
        private string _role = "";
        private readonly JwtSettings _jwtSettings;

        public TokenService(JwtSettings jwtSettings)
        {
            _jwtSettings = jwtSettings;
        }

        public TokenResponse GetToken(TokenRequest request)
        {
            _role = request.role ?? "Admin";
            var token = GenerateJwt();
            var result = new TokenResponse(token);
            return result;
        }

        private string GenerateJwt() => GenerateEncryptedToken(GetSigningCredential());


        private string GenerateEncryptedToken(SigningCredentials signingCredentials)
        {
            var claims = new[]
            {
            new Claim("Role", _role)
        };
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials GetSigningCredential()
        {
            byte[] secret = Encoding.UTF8.GetBytes(_jwtSettings.Key);
            return new SigningCredentials(new SymmetricSecurityKey(secret),
                SecurityAlgorithms.HmacSha256);
        }
    }
}