using ContactsApp.Client.Models;
using ContactsApp.Data;
using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Models
{
    public class TaskerItem
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage ="Every Task must have a Name.")]
        public string? Name { get; set; }

        public bool IsComplete { get; set; }

        [Required(ErrorMessage = "Every Task must have a UserId")]
        public string? UserId { get; set; }

        public virtual ApplicationUser? User { get; set; }
    }

    public static class TaskerItemExtension
    {
        public static TaskerItemDTO ToDTO(this TaskerItem item) => new TaskerItemDTO
        {
            Id = item.Id,
            Name = item.Name,
            IsComplete = item.IsComplete,
        };
    }
}
