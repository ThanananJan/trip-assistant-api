using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Model.Dto.Trip;

namespace TripAssistant.Test.Utility
{
    internal static class AddTripInitial
    {
        /// <summary>
        /// 1-trip have 1-member
        /// 1-user have 1-trip
        /// </summary>
        /// <returns></returns>
        internal static Mock<TripAssistantDbContext> MockContextTripCreate()
        {
            Mock<TripAssistantDbContext> mock = new(ServiceConfig.ContextOptions);
            mock.Setup(m => m.Trips).Returns(GetMockTrip().Object);
            mock.Setup(m => m.Users).Returns(GetMockUser().Object);
            mock.Setup(m => m.TripMembers).Returns(GetMockTripMember().Object);
            mock
              .SetupGet(x => x.Database)
              .Returns(new MockDatabaseFacade(mock.Object));
            return mock;
        }
        private static Mock<DbSet<Trip>> GetMockTrip()
        {
            Mock<DbSet<Trip>> mock = new();
            var data = new List<Trip>() { new() { IdTrip = 1 ,
        Members=[ new() { IdTrip = 1, IdTripMember = 1 }]  } }.AsQueryable();
            mock.As<IQueryable<Trip>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<Trip>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<Trip>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<Trip>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripUser>> GetMockUser()
        {
            Mock<DbSet<TripUser>> mock = new();
            var data = new List<TripUser>() {
        new () { IdTrip = 1, IdUser = 1, Role = Library.Model.Utilities.Enums.TripRole.Owner },
        new () { IdTrip = 2, IdUser = 1, Role = Library.Model.Utilities.Enums.TripRole.Owner }
      }.AsQueryable();
            mock.As<IQueryable<TripUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripMember>> GetMockTripMember()
        {
            Mock<DbSet<TripMember>> mock = new();
            var data = new List<TripMember>() { new() { IdTrip = 1, IdTripMember = 1 } }.AsQueryable();
            mock.As<IQueryable<TripMember>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripMember>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripMember>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripMember>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripTransaction>> GetMockTripTransactionUpdate()
        {
            Mock<DbSet<TripTransaction>> mock = new();
            var data = new List<TripTransaction>() {
        new () { IdTripTransaction = 2, IdPayer=1 ,IdTrip = 2,
        TripDebtors = [ new () { IdTripTransaction = 2, IdDebtor = 2 } ]
        }
      }.AsQueryable();
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<Trip>> GetMockTripUpdate()
        {
            Mock<DbSet<Trip>> mock = new();
            var data = new List<Trip>() {
        new () {IdTrip = 1 },
          new () {IdTrip = 2,
        Members=[
        new (){IdTrip=2,IdTripMember=1,NamMember="เจ้าหนี้"},
         new (){IdTrip=2,IdTripMember=2,NamMember="ลูกหนี้"},
         new (){IdTrip=2,IdTripMember=3,NamMember="คนธรรมดา"}]
          }}.AsQueryable();
            mock.As<IQueryable<Trip>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<Trip>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<Trip>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<Trip>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripMember>> GetMockTripMemberUpdate()
        {
            Mock<DbSet<TripMember>> mock = new();
            var data = new List<TripMember>() {
        new () { IdTrip = 2, IdTripMember = 1 },
      new () { IdTrip = 2, IdTripMember = 2 },
       new () { IdTrip = 2, IdTripMember = 3 },
      }.AsQueryable();
            mock.As<IQueryable<TripMember>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripMember>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripMember>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripMember>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        /// <summary>
        /// 1-trip have no member
        /// 2-trip have 3 members
        /// 1-member be 1-tran-payer
        /// 2-member be 1-tran-debtor
        /// 3-member be normal
        /// </summary>
        /// <returns></returns>
        internal static Mock<TripAssistantDbContext> MockContextTripUpdate()
        {
            Mock<TripAssistantDbContext> mock = new(ServiceConfig.ContextOptions);
            mock.Setup(m => m.Trips).Returns(GetMockTripUpdate().Object);
            mock.Setup(m => m.Users).Returns(GetMockUser().Object);
            mock.Setup(m => m.TripMembers).Returns(GetMockTripMemberUpdate().Object);
            mock.Setup(m => m.Transactions).Returns(GetMockTripTransactionUpdate().Object);
            mock
              .SetupGet(x => x.Database)
              .Returns(new MockDatabaseFacade(mock.Object));
            return mock;
        }
        /// <summary>
        /// 1-user have 2 trips
        /// 1-trip have 1-member
        /// 2-trip have 2 members
        /// </summary>
        /// <returns></returns>
        internal static Mock<TripAssistantDbContext> MockContextTripQuery()
        {
            Mock<TripAssistantDbContext> mock = new(ServiceConfig.ContextOptions);
            mock.Setup(m => m.Trips).Returns(GetMockTripQuery().Object);
            mock.Setup(m => m.Users).Returns(GetMockUserQuery().Object);
            mock.Setup(m => m.TripMembers).Returns(GetMockTripMemberQuery().Object);
            mock.Setup(m => m.Transactions).Returns(GetMockTripTransactionQuery().Object);
            return mock;
        }
        private static Mock<DbSet<Trip>> GetMockTripQuery()
        {
            Mock<DbSet<Trip>> mock = new();
            var data = new List<Trip>() {
        new () {IdTrip = 1,
         Members=[
        new (){IdTrip=2,IdTripMember=1,NamMember="เจ้าหนี้"} ] },
          new () {IdTrip = 2,
        Members=[
        new (){IdTrip=2,IdTripMember=2,NamMember="เจ้าหนี้"},
         new (){IdTrip=2,IdTripMember=3,NamMember="ลูกหนี้"}]
          }}.AsQueryable();
            mock.As<IQueryable<Trip>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<Trip>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<Trip>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<Trip>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripMember>> GetMockTripMemberQuery()
        {
            Mock<DbSet<TripMember>> mock = new();
            var data = new List<TripMember>() {
        new () { IdTrip = 1, IdTripMember = 1 },
      new () { IdTrip = 2, IdTripMember = 2 },
       new () { IdTrip = 2, IdTripMember = 3 },
      }.AsQueryable();
            mock.As<IQueryable<TripMember>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripMember>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripMember>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripMember>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        /// <summary>
        /// 1-trip have 1-transaction
        /// 2-trip have 2-transaction
        /// 2-trip have 2 debtors
        /// </summary>
        /// <returns></returns>
        private static Mock<DbSet<TripTransaction>> GetMockTripTransactionQuery()
        {
            Mock<DbSet<TripTransaction>> mock = new();
            var data = new List<TripTransaction>() {
        new () { IdTrip = 1,Amount=1,
                 IdPayer=1,
                  Payer  = new (){ IdTripMember=1,NamMember="เจ้าหนี้" } ,
                  TripDebtors = [
                    new () {  IdDebtor = 1,Debtor = new (){ IdTripMember=1,NamMember="เจ้าหนี้" }
                     }]},
        new () { IdTrip = 2,Amount =1,
                  IdPayer=2,
                  Payer  = new (){ IdTripMember=2,NamMember="เจ้าหนี้" }  ,
                  TripDebtors =[
                  new () {  IdDebtor = 2 ,Debtor = new (){ IdTripMember=2,NamMember="เจ้าหนี้" } },
                  new () {  IdDebtor = 3 ,Debtor = new (){ IdTripMember=3,NamMember="ลูกหนี้" } }]
        }
      }.AsQueryable();
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripUser>> GetMockUserQuery()
        {
            Mock<DbSet<TripUser>> mock = new();
            var data = new List<TripUser>() {
        new () { IdTrip = 1, IdUser = 1, Role = Library.Model.Utilities.Enums.TripRole.Owner },
        new () { IdTrip = 2, IdUser = 2, Role = Library.Model.Utilities.Enums.TripRole.Owner }
      }.AsQueryable();
            mock.As<IQueryable<TripUser>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripUser>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripUser>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripUser>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        internal static IEnumerable<TripMemberUpdateRequestDto> GetTripMemberUpdate()
        {
            return [
              new TripMemberUpdateRequestDto() { IdTripMember = 1, NamMember = "เจ้าหนี้" },
                new TripMemberUpdateRequestDto() { IdTripMember = 2, NamMember = "ลูกหนี้" },
                new TripMemberUpdateRequestDto() { IdTripMember = 3, NamMember = "คนธรรมดา" }
            ];
        }
    }
}
