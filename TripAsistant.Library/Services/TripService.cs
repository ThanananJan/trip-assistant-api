using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Model.Dto.Transaction;
using TripAssistant.Library.Model.Dto.Trip;
using TripAssistant.Library.Model.Utilities;
using TripAssistant.Library.Models.Dto;

namespace TripAssistant.Library.Services
{
    public class TripService(TripAssistantDbContext db, IMapper mapper, ILogger<TripService> logger)
      : ITripService
    {
        public ResponseDto AddTrip(TripCreateRequestDto request, int idUser)
    {
        using var tran = db.Database.BeginTransaction();
        try
        {
            var trip = AddTrip(request);
            AddTripMember(trip, request);
            AddTripOwner(trip, idUser);
            tran.Commit();
            return ResponseDtoExtension.GetResponseSuccess();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    public ResponseDto DeleteTrip(int idTrip)
    {
        if (!InvalidTripDeleteRequest(idTrip)) throw new InvalidDataException(ErrorHandler.InValiddata);
        using var tran = db.Database.BeginTransaction();
        try
        {
            DeleteTripWithData(idTrip);
            tran.Commit();
            return ResponseDtoExtension.GetResponseSuccess();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }


    public ResponseDto DeleteTripTransaction(int idTripTransaction)
    {
        if (!ValidTripTransactionDeleteRequest(idTripTransaction)) throw new InvalidDataException(ErrorHandler.InValiddata);
        using var tran = db.Database.BeginTransaction();
        try
        {
            DeleteTripTransactionWithDetail(idTripTransaction);
            tran.Commit();
            return ResponseDtoExtension.GetResponseSuccess();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    private void AddTripOwner(Trip trip, int idUser)
    {
        var userTrip = new TripUser()
        {
            IdTrip = trip.IdTrip,
            IdUser = idUser,
            Role = Model.Utilities.Enums.TripRole.Owner,
        };
        db.Users.Add(userTrip);
        db.SaveChanges();
    }

    public ResponseDto UpdateTrip(TripUpdateRequestDto request, int idUser)
    {
        if (!ValidUpdateTripRequest(request, idUser)) throw new InvalidDataException(ErrorHandler.InValiddata);
        using var tran = db.Database.BeginTransaction();
        try
        {
            var trip = db.Trips.Include(p => p.Members).Where(p => p.IdTrip == request.IdTrip).First();
            trip.NamTrip = request.NamTrip;
            RemoveNotExsitMember(trip, request.Members?.Where(p => p.IdTripMember.HasValue).ToArray());
            AddNewTripMember(trip, request.Members?.Where(p => !p.IdTripMember.HasValue).ToArray());

            tran.Commit();
            return ResponseDtoExtension.GetResponseSuccess();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    private void AddTripMember(Trip trip, TripCreateRequestDto request)
    {
        AddNewTripMember(trip, request.Members);

    }

    private void RemoveNotExsitMember(Trip trip, IEnumerable<TripMemberUpdateRequestDto>? memberRequest)
    {
        if (trip.Members == null || memberRequest == null) return;
        var idTripMembers = memberRequest.Where(p => p.IdTripMember.HasValue).Select(p => p.IdTripMember).ToArray();
        var removeMember = trip.Members.Where(p => !idTripMembers.Contains(p.IdTripMember)).AsEnumerable();
        db.TripMembers.RemoveRange(removeMember);
        db.SaveChanges();
    }

    private void AddNewTripMember(Trip trip, IEnumerable<TripMemeberCreateRequestDto>? memberRequest)
    {
        if (memberRequest == null) return;
        var newMembers = memberRequest
          .Select(p => new TripMember()
          {
              IdTrip = trip.IdTrip,
              IdTripMember = 0,
              NamMember = p.NamMember.Trim()
          })
          .ToList();
        db.TripMembers.AddRange(newMembers);
        db.SaveChanges();
    }

    private Trip AddTrip(TripCreateRequestDto request)
    {
        var trip = mapper.Map<Trip>(request);
        db.Trips.Add(trip);
        db.SaveChanges();
        return trip;
    }
    private bool ValidUpdateTripRequest(TripUpdateRequestDto request, int idUser)
    {
        if (!db.Users.Any(p => p.IdTrip == request.IdTrip && p.IdUser == idUser)) return false;
        if (!request.IdTrip.HasValue || !db.Trips.Any(p => p.IdTrip == request.IdTrip)) return false;

        if (request.Members == null) return false;
        var idEsitMember = request.Members
          .Where(p => p.IdTripMember.HasValue)
          .Select(p => p.IdTripMember)
          .ToArray();

        if (!ValidExistTripMember(idEsitMember)) return false;

        var idRemoveMembers = db.TripMembers
          .Where(p => !idEsitMember.Contains(p.IdTripMember))
          .Select(p => p.IdTripMember)
          .ToArray();

        if (RemoveMemberIsTripPayer(request.IdTrip.Value, idRemoveMembers)) return false;
        if (RemoveMemberIsTripDebtor(request.IdTrip.Value, idRemoveMembers)) return false;
        return true;

    }

    private bool ValidExistTripMember(int?[] idEsitMember)
    {
        return db.TripMembers.Count(p => idEsitMember.Contains(p.IdTripMember)) == idEsitMember.Length;
    }
    private bool InvalidTripDeleteRequest(int idTrip)
    {
        return db.Trips.Any(p => p.IdTrip == idTrip);
    }
    private bool RemoveMemberIsTripDebtor(int idTrip, int[] idRemoveMembers)
    {
        return db.Transactions
         .Include(p => p.TripDebtors)
         .Where(p => p.IdTrip == idTrip)
         .SelectMany(p => p.TripDebtors)
         .Where(p => idRemoveMembers.Contains(p.IdDebtor))
         .Any();
    }

    private bool RemoveMemberIsTripPayer(int idTrip, int[] idRemoveMembers)
    {
        return db.Transactions
          .Where(p => p.IdTrip == idTrip && idRemoveMembers.Contains(p.IdPayer))
          .Any();

    }

    public ResponseDto AddTripTransaction(TripTransactionCreateRequestDto request)
    {
        if (!ValidTripTransactionCreateRequest(request)) throw new InvalidDataException(ErrorHandler.InValiddata);
        using var tran = db.Database.BeginTransaction();
        try
        {
            AddTripTransactionWithDetail(request);
            tran.Commit();
            return ResponseDtoExtension.GetResponseSuccess();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }


    public ResponseDto UpdateTripTransaction(TripTransactionUpdateRequestDto request)
    {
        if (!ValidTripTransactionUpdateRequest(request)) throw new InvalidDataException(ErrorHandler.InValiddata);
        using var tran = db.Database.BeginTransaction();
        try
        {
            UpdateTripTransactionWithDetail(request);
            tran.Commit();
            return ResponseDtoExtension.GetResponseSuccess();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }
    private void AddTripTransactionWithDetail(TripTransactionCreateRequestDto request)
    {
        var transaction = mapper.Map<TripTransaction>(request);
        db.Transactions.Add(transaction);
        db.SaveChanges();
        var debtors = request.TripDebtors
          .Select(p => new TripDebtor() { IdTripTransaction = transaction.IdTripTransaction, IdDebtor = p.IdDebtor })
          .ToArray();
        transaction.TripDebtors = debtors;
        db.SaveChanges();
    }
    private void UpdateTripTransactionWithDetail(TripTransactionUpdateRequestDto request)
    {
        var transaction = db.Transactions
          .Include(p => p.TripDebtors)
          .First(p => p.IdTripTransaction == request.IdTripTransaction);
        mapper.Map(request, transaction);
        removeExistDebtors(request);
        var debtors = request.TripDebtors
          .Select(p => new TripDebtor() { IdTripTransaction = transaction.IdTripTransaction, IdDebtor = p.IdDebtor })
          .ToArray();
        transaction.TripDebtors = debtors;
        db.SaveChanges();
    }

    private void removeExistDebtors(TripTransactionUpdateRequestDto request)
    {
        db.TripDebtors.RemoveRange(db.TripDebtors.Where(p => p.IdTripTransaction == request.IdTripTransaction));
        db.SaveChanges();
    }

    private void DeleteTripWithData(int idTrip)
    {
        var trip = db.Trips.Include(p => p.Members).First(p => p.IdTrip == idTrip);
        db.Trips.Remove(trip);
        var user = db.Users.Where(p => p.IdTrip == idTrip).First();
        db.Users.Remove(user);
        var transactions = db.Transactions
            .Include(p => p.TripDebtors)
            .Where(p => p.IdTrip == idTrip)
            .ToList();
        db.Transactions.RemoveRange(transactions);
        db.SaveChanges();

    }
    private void DeleteTripTransactionWithDetail(int idTripTransaction)
    {
        var transaction = db.Transactions
         .Include(p => p.TripDebtors)
         .First(p => p.IdTripTransaction == idTripTransaction);
        db.Transactions.Remove(transaction);
        db.SaveChanges();
    }
    private bool ValidTripTransactionDeleteRequest(int idTripTransaction)
    {
        return db.Transactions.Any(p => p.IdTripTransaction == idTripTransaction);
    }
    private bool ValidTripTransactionCreateRequest(TripTransactionCreateRequestDto request)
    {
        return ValidTripMember(request);
    }
    private bool ValidTripTransactionUpdateRequest(TripTransactionUpdateRequestDto request)
    {
        if (!db.Transactions.Any(p => p.IdTripTransaction == request.IdTripTransaction)) return false;
        return ValidTripMember(request);
    }
    private bool ValidTripMember(TripTransactionCreateRequestDto request)
    {
        var idDebtors = request.TripDebtors.Select(p => p.IdDebtor).ToArray();
        if (!db.Trips.Include(p => p.Members)
          .Any(p => p.IdTrip == request.IdTrip
          && p.Members != null
          && p.Members.Any(m => m.IdTripMember == request.TripPayer.IdPayer)
          && (p.Members.Count(m => idDebtors.Contains(m.IdTripMember)) == idDebtors.Count())
          )) return false;

        return true;
    }
    public ResponseDto AssignTrip(int idTrip, int idUser)
    {
        if (!ValidTripAssignRequest(idTrip, idUser)) throw new InvalidDataException(ErrorHandler.InValiddata);
        using var tran = db.Database.BeginTransaction();
        try
        {
            var user = new TripUser() { IdTrip = idTrip, IdUser = idUser, Role = Enums.TripRole.Guest };
            db.Users.Add(user);
            db.SaveChanges();
            tran.Commit();
            return ResponseDtoExtension.GetResponseSuccess();
        }
        catch (Exception ex)
        {
            tran.Rollback();
            logger.LogError(ex, "{Message},{StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    private bool ValidTripAssignRequest(int idTrip, int idUser)
    {
        if (!db.Trips.Any(p => p.IdTrip == idTrip)) return false;
        if (db.Users.Any(p => p.IdTrip == idTrip && p.IdUser == idUser)) return false;
        return true;
    }

    public IEnumerable<TripDto> GetTrips(int idUser)
    {
        return (from user in db.Users.Where(p => p.IdUser == idUser)
                join trip in db.Trips.Include(p => p.Members) on user.IdTrip equals trip.IdTrip
                into userTripJoin
                from userTrip in userTripJoin.DefaultIfEmpty()
                where userTrip != null
                select
                userTrip
                      ).AsEnumerable()
                      .Select(p => mapper.Map<TripDto>(p))
                      .ToArray();
    }

    public TripSummary? GetTripSummary(int idTrip)
    {
        var trip = db.Trips.Include(p => p.Members)
          .Where(p => p.IdTrip == idTrip)
          .AsEnumerable()
          .Select(p => mapper.Map<TripSummary>(p))
          .FirstOrDefault();
        if (trip == null) return null;
        trip.Transactions = db.Transactions
                        .Include(p => p.TripDebtors)
                        .Where(p => p.IdTrip == idTrip);
        return trip;

    }

    public IEnumerable<TripTransactionDto> GetTripTransactions(int idTrip)
    {
        return db.Transactions
              .Include(p => p.Payer)
              .Include(p => p.TripDebtors)
              .ThenInclude(p => p.Debtor)
              .Where(p => p.IdTrip == idTrip)
              .AsEnumerable()
              .Select(p => mapper.Map<TripTransactionDto>(p))
              .OrderBy(p => p.DtmCreate);

    }

    public TripDto? GetTripById(int idTrip)
    {
        return db.Trips.Where(p => p.IdTrip == idTrip)
                  .Include(p => p.Members)
                      .Select(p => mapper.Map<TripDto>(p))
                      .FirstOrDefault();
    }

    public TripTransactionDto? GetTripTransaction(int idTripTransaction)
    {

        return db.Transactions
              .Include(p => p.Payer)
              .Include(p => p.TripDebtors)
              .ThenInclude(p => p.Debtor)
              .Where(p => p.IdTripTransaction == idTripTransaction)
              .AsEnumerable()
              .Select(p => mapper.Map<TripTransactionDto>(p))
              .FirstOrDefault();

    }
}
}

