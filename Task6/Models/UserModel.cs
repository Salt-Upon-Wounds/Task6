using System.ComponentModel.DataAnnotations;

namespace Task6.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
    }
}
