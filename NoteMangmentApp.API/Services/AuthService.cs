using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NoteMangmentApp.API.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace NoteMangmentApp.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;



        public AuthService(UserManager<AppUser> userManager,
                           IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));

        }

        public async Task<MemberVM> RegisterAsync(RegisterVM model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new MemberVM { Message = "Email is already registered!" };


            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,

            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new MemberVM { Message = errors };
            }
            var jwtSecurityToken = await CreateToken(user);

            return new MemberVM
            {
                Email = user.Email,
                
                IsAuthenticated = true,
                Token = jwtSecurityToken,
               
            };
        }

        public async Task<MemberVM> LoginAsync(LoginVM model)
        {
            var authModel = new MemberVM();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = jwtSecurityToken;
            authModel.Email = user.Email;
            //authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            //authModel.Roles = rolesList.ToList();

            return authModel;
        }



        public async Task<string> CreateToken(AppUser? appUser)
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
