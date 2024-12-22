using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JWTAuthentication.Library.Model.DB
{
    [Table("UserToken")]
    public class UserToken
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdToken { get; set; }
        public int IdUser { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime DtmRefreshTokenExpired { get; set; }
        [ForeignKey(nameof(IdUser))]
        public virtual User? User { get; set; }

    }
}
