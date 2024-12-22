using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Library.Model.Dto
{
    public class RefreshTokenRequest
    {
        [Required]
        public string AccessToken { get; set; } = string.Empty;
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
