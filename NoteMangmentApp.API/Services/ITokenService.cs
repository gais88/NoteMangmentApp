using Core.Models;

namespace NoteMangmentApp.API.Services
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser appUser);
    }
}
