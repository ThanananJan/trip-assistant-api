namespace JWTAuthentication.Library.Model.Dto
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string JwtTokenExpiredMinute = "10";
        public string RefreshTokenExpiredDay = "1";
    }
    public class AwsSettings
    {
        public string CognitoRegion { get; set; } = string.Empty;
        public string CognitoUserPoolId { get; set; } = string.Empty;
        public string CognitoUserInfoUrl { get; set; } = string.Empty;
    }
}
