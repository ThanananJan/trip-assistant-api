using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Model.Dto.Transaction;

namespace TripAssistant.Library.Model.Dto.Trip
{
    public class TripSummary
    {
        public int IdTrip { get; set; }
        public string NamTrip { get; set; } = string.Empty;
        public decimal Total { get { return Members.Sum(p => p.PayerAmount); } }

        public IEnumerable<TripMemberSummary> Members
        {
            get
            {
                return TripMembers.Select(p =>
              new TripMemberSummary
              {
                  IdTripMember = p.IdTripMember,
                  NamMember = p.NamMember,
                  PayerAmount = Transactions
                .Where(t => t.IdPayer == p.IdTripMember)
                .Sum(p => p.Amount),
                  DebtorAmount = Transactions
                .Where(t => t.TripDebtors.Any(d => d.IdDebtor == p.IdTripMember) || !t.TripDebtors.Any())
                .Select(p => p.Amount / (p.TripDebtors.Count() > 0 ? p.TripDebtors.Count() : TripMembers.Count()))
                .Sum()
              });
            }
        }
        [JsonIgnore]
        internal IEnumerable<TripMemberDto> TripMembers { get; set; } = Enumerable.Empty<TripMemberDto>();

        [JsonIgnore]
        internal IEnumerable<TripTransaction> Transactions { get; set; } = Enumerable.Empty<TripTransaction>();
    }
    public class TripMemberSummary
    {
        public int IdTripMember { get; set; }
        public string NamMember { get; set; } = string.Empty;
        public decimal PayerAmount { get; set; }
        public decimal DebtorAmount { get; set; }
        public decimal BalanceAmount { get { return PayerAmount - DebtorAmount; } }

    }
}
