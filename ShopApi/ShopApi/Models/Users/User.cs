using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Models.Users
{
    [Table("User")]
    public class User
    {
        [Key]
        [StringLength(32)]
        public string Id { get; set; }

        [StringLength(255)]
        public string Firstname { get; set; }

        [StringLength(255)]
        public string Lastname { get; set; }


        [NotMapped]
        public string Fullname { 
            get { return Firstname + " " + Lastname; }
        }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string? Phone { get; set; }
        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public long? Birthday { get; set; }

        public DateTimeOffset DateCreated { get; set; }

        // Navigation props

    }
}
