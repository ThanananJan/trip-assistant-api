
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TripAssistant.Library.Model.DB;


namespace TripAssistant.Library.Model.Dto.Trip
{
    public class TripCreateRequestDto
    {
        [Required]
        [StringLength(100)]
        public string NamTrip { get; set; } = string.Empty;
        [JsonIgnore]
        public DateTime DtmCreate { get; set; } = DateTime.UtcNow;
        [Required]
        public IEnumerable<TripMemeberCreateRequestDto>? Members { get; set; }
    }
    public class TripMemeberCreateRequestDto
    {
        [Required]
        [StringLength(100)]
        public string NamMember { get; set; } = string.Empty;
    }
    public class TripUpdateRequestDto
    {
        [Required]
        public int? IdTrip { get; set; }
        [StringLength(100)]
        [Required]
        public string NamTrip { get; set; } = string.Empty;
        [Required]
        public IEnumerable<TripMemberUpdateRequestDto>? Members { get; set; }

    }
    public class TripMemberUpdateRequestDto : TripMemeberCreateRequestDto
    {

        public int? IdTripMember { get; set; }
    }

}
