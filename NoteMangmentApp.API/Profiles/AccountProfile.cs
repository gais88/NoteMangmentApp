using AutoMapper;
using Core.Models;
using NoteMangmentApp.API.ViewModels;

namespace NoteMangmentApp.API.Profiles
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            // Member Users
            CreateMap<AppUser, MemberVM>().ReverseMap();
        }
    }
}
