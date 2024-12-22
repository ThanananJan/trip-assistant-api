using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TripAssistant.Library.Model.DB
{
    [Table("Trip")]
    public class Trip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTrip { get; set; }
        [StringLength(100)]
        public string NamTrip { get; set; } = string.Empty;
        public DateTime DtmCreate { get; set; }
        public virtual ICollection<TripMember>? Members { get; set; }
    }
}
