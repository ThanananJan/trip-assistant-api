namespace TripAssistant.Library.Model.Utilities
{
    public partial class Utility
    {
        public static readonly Dictionary<string, string> DateFormat = new Dictionary<string, string>()
      {
            { "UTC",  "yyyy-MM-ddTHH:mm:ssZ" },
            { "LOCAL", "yyyy-MM-ddTHH:mm:ss" }

      };
    }
}


