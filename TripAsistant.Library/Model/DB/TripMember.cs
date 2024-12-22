using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripAssistant.Library.Model.DB
{
    [Table("TripMember")]
    public class TripMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTripMember { get; set; }
        public int IdTrip { get; set; }
        [StringLength(100)]
        public string NamMember { get; set; } = string.Empty;

        [ForeignKey(nameof(IdTrip))]
        public virtual Trip Trip { get; set; }
    }
}
