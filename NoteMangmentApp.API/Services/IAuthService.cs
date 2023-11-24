using NoteMangmentApp.API.ViewModels;

namespace NoteMangmentApp.API.Services
{
    public interface IAuthService
    {
        Task<MemberVM> RegisterAsync(RegisterVM model);
        Task<MemberVM> LoginAsync(LoginVM model);
    }
}
