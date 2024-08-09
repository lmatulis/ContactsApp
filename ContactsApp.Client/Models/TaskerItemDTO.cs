using System.ComponentModel.DataAnnotations;

namespace ContactsApp.Client.Models
{
    public class TaskerItemDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Every Task must have a Name")]
        public string? Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
