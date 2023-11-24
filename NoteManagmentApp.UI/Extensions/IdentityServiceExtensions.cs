using Core.Models;
using Data.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace NoteManagmentApp.UI.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            // CORS Configurations.
            services.AddCors(option =>
            {
                option.AddPolicy("CorsePolicy", policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
            });
            //services.AddAuthentication();
            // Add Authentication
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //  .AddCookie(options =>
            //  {
            //      options.AccessDeniedPath = new PathString("/Account/SignIn");
            //      options.LoginPath = new PathString("/Account/SignIn");
            //      options.LogoutPath = new PathString("/Home/SignOut");


            //  });

            // Add Identity
           

            return services;
        }
    }
}
