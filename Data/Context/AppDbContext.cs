using Core.Models;
using Data.Interceptors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>, AppUserRole,
                                IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        // Identity
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ManyToMany in EF5 between AppUser and AppRole with [AppUserRole]
            builder.Entity<AppUser>()
                   .HasMany(x => x.AppRoles)
                   .WithMany(x => x.AppUsers)
                   .UsingEntity<AppUserRole>(
                       x => x.HasOne<AppRole>().WithMany().HasForeignKey(x => x.RoleId),
                       x => x.HasOne<AppUser>().WithMany().HasForeignKey(x => x.UserId))
                   .Property(x => x.CreatedDate)
                   .HasDefaultValueSql("GETDATE()");

            builder.Entity<AppUser>()
                   .HasMany(x => x.Notes)
                   .WithOne(x => x.AppUser)
                   .HasForeignKey(x => x.AppUserId);

           
            builder.Entity<Note>().HasQueryFilter(b => !b.IsDeleted);




        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.SetAuditProperties();
            return await base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            ChangeTracker.SetAuditProperties();
            return base.SaveChanges();
        }


    }
}