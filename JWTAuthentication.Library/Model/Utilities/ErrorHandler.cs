using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Library.Model.Utilities
{
    public partial class Utility
    {
        public enum ErrorHandler
        {
            InvalidData = 0,

        }
        public static readonly Dictionary<string, string> ErrorHandlers = new Dictionary<string, string>()
    {
      {ErrorHandler.InvalidData.ToString(),"invalid data" }
    };
    }
}
