using System.ComponentModel.DataAnnotations;
using ContactsApp.Client.Models;
using ContactsApp.Data;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ContactsApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        public string? Name { get; set; }

        [Required]
        public string? AppUserId { get; set; }
        public virtual ApplicationUser? AppUser { get; set; }

        public virtual ICollection<Contact> Contacts { get; set; } = [];
    }

    public static class CategoryItemExtension
    {
        public static CategoryDTO ToDTO(this Category item)
        {
            CategoryDTO categoryDTO = new()
            {
                Id = item.Id,
                Name = item.Name,
            };

            foreach (Contact contact in item.Contacts)
            {
                contact.Categories.Clear();
                categoryDTO.Contacts.Add(contact.ToDTO());
            }

            return categoryDTO;
        }
    }
}
