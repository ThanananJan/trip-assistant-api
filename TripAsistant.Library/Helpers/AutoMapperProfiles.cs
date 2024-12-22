

using AutoMapper;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Model.Dto.Transaction;
using TripAssistant.Library.Model.Dto.Trip;

namespace TripAssistant.Library
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            AutoMappingTrip();
            AutoMappingTripTransaction();
        }

        private void AutoMappingTripTransaction()
        {
            CreateMap<TripTransactionCreateRequestDto, TripTransaction>()
              .ForMember(p => p.IdPayer, p => p.MapFrom(o => o.TripPayer.IdPayer))
              .ForMember(p => p.TripDebtors, p => p.Ignore());

            CreateMap<Trip, TripDto>()
              .ForMember(p => p.Members, p => p.MapFrom(p => p.Members));
            CreateMap<TripMember, TripMemberDto>();
            CreateMap<TripTransaction, TripTransactionDto>()
              .ForMember(p => p.TripPayer, p => p.MapFrom(o => o.Payer))
              .ForMember(p => p.TripDebtors, p => p.MapFrom(o => o.TripDebtors));
            CreateMap<TripDebtor, TripDebtorDto>()
              .ForMember(p => p.NamDebtor, p => p.MapFrom(o => o.Debtor.NamMember));
            CreateMap<TripMember, TripPayerDto>()
              .ForMember(p => p.IdPayer, p => p.MapFrom(o => o.IdTripMember))
              .ForMember(p => p.NamPayer, p => p.MapFrom(o => o.NamMember));

        }

        private void AutoMappingTrip()
        {
            CreateMap<TripCreateRequestDto, Trip>()
              .ForMember(p => p.Members, p => p.Ignore());
            CreateMap<Trip, TripSummary>()
              .ForMember(p => p.TripMembers, p => p.MapFrom(o => o.Members))
              .ForMember(p => p.Members, p => p.Ignore());



        }
    }
}
