using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NoteMangmentApp.API.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config, UserManager<AppUser> userManager)
        {
            _config = config;
            _userManager = userManager;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));
        }
       
        public async Task<string> CreateToken(AppUser appUser)
        {
            // Add Claims
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, appUser.UserName)
            };

            // Get User Roles
            var roles = await _userManager.GetRolesAsync(appUser);

            // Add Roles to Claim List
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // Create Credential
            var credential = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Describe Token Look 
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = credential
            };

            // Create Token Handler 
            var tokenHandler = new JwtSecurityTokenHandler();

            // Create Token
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return Created Token
            return tokenHandler.WriteToken(token);
        }
    }
}
