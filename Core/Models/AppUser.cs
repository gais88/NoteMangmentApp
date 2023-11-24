using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; } = string.Empty;
        public ICollection<Note> Notes { get; set; } = default!;
        public ICollection<AppRole> AppRoles { get; set; } = default!;
        public ICollection<IdentityUserClaim<int>> Claims { get; set; } = default!;
        public ICollection<IdentityUserLogin<int>> Logins { get; set; } = default!;
        public ICollection<IdentityUserToken<int>> Tokens { get; set; } = default!;
    }
}
