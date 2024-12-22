using JWTAuthentication.Library.Model.DB;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Library.Model.DB
{
    [Table("User")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdUser { get; set; }
        [Required]
        [StringLength(100)]
        public string NamUser { get; set; } = string.Empty;
        [Required]
        public Guid IdSub { get; set; }
        [StringLength(50)]
        public string IdentityProviderName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime DtmCreate { get; set; }
        public DateTime DtmUpdate { get; set; }
        public virtual ICollection<UserToken>? Tokens { get; set; }
    }
}
