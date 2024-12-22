namespace JWTAuthentication.Library.Model.Utilities
{
    public partial class Utility
    {
        public static readonly Dictionary<string, string> UserErrorHandler = new Dictionary<string, string>()
      {
            { "duplicate",  "user duplicated" },
            { "notfound", "user not found" }

      };
        public static readonly Dictionary<string, string> IdentityProvider = new Dictionary<string, string>()
      {
            { "aws",  "aws cognito" }

      };

    }
    public class RequestHeader
    {
        public const string Authorization = "Authorization";
    }

}
