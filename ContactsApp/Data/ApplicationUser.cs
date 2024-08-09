using ContactsApp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        public Guid? ImageId { get; set; }
        public virtual ImageUpload? Image { get; set; }

        public virtual ICollection<TaskerItem> TaskerItems { get; set; } = new HashSet<TaskerItem>();
    }

}
