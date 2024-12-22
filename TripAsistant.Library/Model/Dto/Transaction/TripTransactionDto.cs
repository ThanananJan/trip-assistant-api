using TripAssistant.Library.Model.DB;

namespace TripAssistant.Library.Model.Dto.Transaction
{
    public class TripTransactionDto
    {
        public int IdTripTransaction { get; set; }
        public int IdTrip { get; set; }
        public string DscTransaction { get; set; } = string.Empty;
        public decimal amount { get; set; }
        public DateTime DtmCreate { get; set; }
        public TripPayerDto TripPayer { get; set; } = new TripPayerDto();
        public IEnumerable<TripDebtorDto> TripDebtors { get; set; } = Enumerable.Empty<TripDebtorDto>();
    }
    public class TripPayerDto
    {
        public int IdTrip { get; set; }
        public int IdPayer { get; set; }
        public string NamPayer { get; set; } = string.Empty;
    }
    public class TripDebtorDto
    {
        public int IdTrip { get; set; }
        public int IdDebtor { get; set; }
        public string NamDebtor { get; set; } = string.Empty;
    }
}
