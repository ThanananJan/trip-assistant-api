using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Library.Model.DB;
using System.Text.Json.Serialization;

namespace TripAssistant.Library.Model.Dto.Transaction
{
    public class TripTransactionCreateRequestDto
    {
        [Required]
        public int IdTrip { get; set; }
        [Required]
        [StringLength(100)]
        public string DscTransaction { get; set; } = string.Empty;
        [Range(0.01, int.MaxValue)]
        public decimal Amount { get; set; }
        [JsonIgnore]
        public DateTime DtmCreate { get; set; } = DateTime.UtcNow;
        [Required]
        public TransactionPayerRequestDto TripPayer { get; set; } = new TransactionPayerRequestDto();
        [Required]
        public IEnumerable<TransactionDebtorRequestDto> TripDebtors { get; set; } = new TransactionDebtorRequestDto[] { };
    }
    public class TransactionPayerRequestDto
    {
        [Required]
        public int IdPayer { get; set; }
    }
    public class TransactionDebtorRequestDto
    {
        [Required]
        public int IdDebtor { get; set; }
    }
    public class TripTransactionUpdateRequestDto : TripTransactionCreateRequestDto
    {
        [Required]
        public int IdTripTransaction { get; set; }

    }
}
