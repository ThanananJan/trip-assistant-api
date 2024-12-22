using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Library.Model.DB;

namespace TripAssistant.Library.Model.Dto.Trip
{
    public class TripDto
    {
        public int IdTrip { get; set; }
        public string NamTrip { get; set; } = string.Empty;
        public DateTime DtmCreate { get; set; }
        public IEnumerable<TripMemberDto> Members { get; set; } = Enumerable.Empty<TripMemberDto>();
    }
    public class TripMemberDto
    {
        public int IdTripMember { get; set; }
        public string NamMember { get; set; } = string.Empty;
    }
}
