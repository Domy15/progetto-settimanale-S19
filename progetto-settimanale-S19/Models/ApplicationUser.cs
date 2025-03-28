using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace progetto_settimanale_S19.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        public ICollection<ApplicationUserRole> ApplicationUserRoles { get; set; }

        public ICollection<Tycket> Tyckets { get; set; }
    }
}
