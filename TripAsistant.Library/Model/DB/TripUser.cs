using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripAssistant.Library.Model.DB
{
    [Table("TripUser")]
    public class TripUser
    {
        public int IdTrip { get; set; }
        public int IdUser { get; set; }
        public Utilities.Enums.TripRole Role { get; set; }
        [ForeignKey(nameof(IdTrip))]
        public virtual Trip Trip { get; set; }

    }
}
