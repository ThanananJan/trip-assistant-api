namespace JWTAuthentication.Library.Model.Dto
{
    public class JwtToken
    {
        public string UserName { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
