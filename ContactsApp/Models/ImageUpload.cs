using System.ComponentModel.DataAnnotations;
namespace ContactsApp.Models
{
    public class ImageUpload
    {
        public Guid Id { get; set; }
        [Required]
        public byte[]? Data { get; set; }
        public string? Extension { get; set; }
    }
}
