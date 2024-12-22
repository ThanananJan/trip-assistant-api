using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TripAssistant.Library.Model.DB
{
    [Table("TripTransaction")]
    public class TripTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTripTransaction { get; set; }
        public int IdTrip { get; set; }
        [StringLength(100)]
        public string DscTransaction { get; set; } = string.Empty;
        [Column(TypeName = "decimal(6, 2)")]
        public decimal Amount { get; set; }
        public DateTime DtmCreate { get; set; }
        public int IdPayer { get; set; }
        [ForeignKey(nameof(IdPayer))]
        public virtual TripMember Payer { get; set; }
        public virtual ICollection<TripDebtor> TripDebtors { get; set; }


    }

    [Table("TripDebtor")]
    public class TripDebtor
    {
        public int IdTripTransaction { get; set; }
        public int IdDebtor { get; set; }
        [ForeignKey(nameof(IdDebtor))]
        public virtual TripMember Debtor { get; set; }
    }
}
