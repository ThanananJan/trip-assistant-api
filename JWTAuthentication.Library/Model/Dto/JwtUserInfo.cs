using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWTAuthentication.Library.Model.Dto
{
    public class JwtUserInfo
    {
        public int IdUser { get; set; }
        public string NamUser { get; set; } = string.Empty;

    }
}
