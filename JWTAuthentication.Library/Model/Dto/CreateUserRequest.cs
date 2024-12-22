using System.ComponentModel.DataAnnotations;

namespace JWTAuthentication.Library.Model.Dto
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
    }

}
