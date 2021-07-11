using ShopApi.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApi.Models.Users
{

    public enum ProviderType
    {
        Google = 0,
        Facebook = 1,
    }

    [Table("AuthenticationProvider")]
    public class AuthenticationProvider
    {
        [Key]
        [StringLength(128)]
        public string Id { get; set; }

        [StringLength(128)]
        public string KeyProvided { get; set; }

        [Required]
        [StringLength(32)]
        public string UserId { get; set; }

        [Required]
        [Column("ProviderType")]
        [StringLength(128)]
        public string ProviderTypeString
        {
            get { return ProviderType.ToString(); }
            private set { ProviderType = EnumExtensions.ParseEnum<ProviderType>(value); }
        }

        [NotMapped]
        public ProviderType ProviderType;

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
