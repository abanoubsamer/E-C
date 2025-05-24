using Domain.Enums.Notification;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        public string ReceiverId { get; set; }

        public NotificationReceiverType ReceiverType { get; set; }

        public string Message { get; set; }

        public string Title { get; set; }

        public bool IsRead { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(ReceiverId))]
        public virtual ApplicationUser Receiver { get; set; }
    }
}