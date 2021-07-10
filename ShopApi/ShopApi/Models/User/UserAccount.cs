using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Models.User
{
    [Table("UserAccount")]
    public class UserAccount
    {
        [Key]
        [StringLength(20)]
        public string Id { get; set; }

        [Required]
        [StringLength(32)]
        public string UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordSalt { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        // Relation props
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
