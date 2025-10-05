// Models/Customer.cs
using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace ST10449143_CLDV6212_POEPART1.Models
{
    public class Customer : ITableEntity
    {
        public string PartitionKey { get; set; } = "Customer";

        public string RowKey { get; set; } = Guid.NewGuid().ToString();

        public DateTimeOffset? Timestamp { get; set; }

        [IgnoreDataMember] // prevents direct serialization
        public ETag ETag { get; set; }

        [Display(Name = "ETag")]
        public string ETagValue
        {
            get => ETag.ToString();
            set => ETag = string.IsNullOrEmpty(value) ? default : new ETag(value);
        }

        [Display(Name = "Customer ID")]
        public string CustomerId => RowKey;

        [Required]
        [Display(Name = "First Name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string Surname { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
