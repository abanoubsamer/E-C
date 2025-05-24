using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        public string? Picture { get; set; }

        public string? SellerID { get; set; } // Nullable property to allow optional seller

        public virtual Seller Seller { get; set; }

        public virtual Card card { get; set; }

        public DateTime DateCreate { get; set; } = DateTime.Now;

        public List<RefreshToken>? RefreshTokens { get; set; }

        public virtual ICollection<ShippingAddress> ShippingAddresses { get; set; }

        public virtual ICollection<UserFCMToken> FCMTokens { get; set; } = new List<UserFCMToken>();

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public virtual ICollection<UserPhoneNumber> PhoneNumbers { get; set; } = new List<UserPhoneNumber>();

        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

        [NotMapped] // نمنع EF من إضافته في الداتا بيز
        public override string PhoneNumber
        { get => null; set { } }
    }
}