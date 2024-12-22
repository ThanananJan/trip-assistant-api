using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Library.Model.Dto.Transaction;
using TripAssistant.Library.Model.Dto.Trip;
using TripAssistant.Library.Models.Dto;

namespace TripAssistant.Library.Services
{
    public interface ITripService
    {
        ResponseDto AddTrip(TripCreateRequestDto request, int idUser);
        ResponseDto UpdateTrip(TripUpdateRequestDto request, int idUser);
        ResponseDto DeleteTrip(int idTrip);
        ResponseDto AddTripTransaction(TripTransactionCreateRequestDto requestDto);
        ResponseDto UpdateTripTransaction(TripTransactionUpdateRequestDto request);
        ResponseDto DeleteTripTransaction(int idTripTransaction);
        IEnumerable<TripDto> GetTrips(int idUser);
        ResponseDto AssignTrip(int idTrip, int idUser);
        TripSummary? GetTripSummary(int idTrip);
        IEnumerable<TripTransactionDto> GetTripTransactions(int idTrip);
        TripDto? GetTripById(int idTrip);
        TripTransactionDto? GetTripTransaction(int idTripTransaction);
    }
}
