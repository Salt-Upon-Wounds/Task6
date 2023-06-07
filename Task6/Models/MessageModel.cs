using System.ComponentModel.DataAnnotations;

namespace Task6.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? Text { get; set; }
        [Required]
        public DateTime SentAt { get; set; }
        [Required]
        public UserModel? Sender { get; set; }
        
        public UserModel? Recipient { get; set; }
    }
}
