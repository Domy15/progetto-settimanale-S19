using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using progetto_settimanale_S19.Models;

namespace progetto_settimanale_S19.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string,
        IdentityUserClaim<string>, ApplicationUserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public DbSet<Artist> Artists { get; set; }

        public DbSet<Event> Events { get; set; }

        public DbSet<Tycket> Tyckets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.ApplicationUserRoles)
                .HasForeignKey(ur => ur.UserId);

            modelBuilder.Entity<ApplicationUserRole>()
                .HasOne(ur => ur.Role)
                .WithMany(u => u.ApplicationUserRoles)
                .HasForeignKey(ur => ur.RoleId);

            modelBuilder.Entity<Event>()
                .HasOne(e => e.Artist)
                .WithMany(a => a.Events)
                .HasForeignKey(e => e.ArtistId);

            modelBuilder.Entity<Tycket>()
                .HasOne(t => t.Event)
                .WithMany(e => e.Tyckets)
                .HasForeignKey(t => t.EventId);

            modelBuilder.Entity<Tycket>()
                .HasOne(t => t.User)
                .WithMany(u => u.Tyckets)
                .HasForeignKey(t => t.UserId);
        }
    }
}
