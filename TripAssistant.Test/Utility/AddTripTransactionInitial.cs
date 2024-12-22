using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Library.Model.DB;

namespace TripAssistant.Test.Utility
{
    internal static class AddTripTransactionInitial
    {
        /// <summary>
        /// 1-trip have 2 member
        /// </summary>
        /// <returns></returns>
        internal static Mock<TripAssistantDbContext> MockContextTripTransactionCreate()
        {
            Mock<TripAssistantDbContext> mock = new(ServiceConfig.ContextOptions);
            mock.Setup(m => m.Trips).Returns(GetMockTrip().Object);
            mock.Setup(m => m.TripMembers).Returns(GetMockTripMember().Object);
            mock.Setup(m => m.Transactions).Returns(GetMockTripTransaction().Object);
            mock
              .SetupGet(x => x.Database)
              .Returns(new MockDatabaseFacade(mock.Object));
            return mock;
        }

        internal static Mock<TripAssistantDbContext> MockContextTripTransactionUpdate()
        {
            Mock<TripAssistantDbContext> mock = new(ServiceConfig.ContextOptions);
            mock.Setup(m => m.Trips).Returns(GetMockTrip().Object);
            mock.Setup(m => m.Users).Returns(GetMockUser().Object);
            mock.Setup(m => m.TripMembers).Returns(GetMockTripMember().Object);
            mock.Setup(m => m.Transactions).Returns(GetMockTripTransactionUpdate().Object);
            mock.Setup(m => m.TripDebtors).Returns(GetMockTripDebtorUpdate().Object);
            mock
              .SetupGet(x => x.Database)
              .Returns(new MockDatabaseFacade(mock.Object));
            return mock;
        }



        private static Mock<DbSet<Trip>> GetMockTrip()
        {
            Mock<DbSet<Trip>> mock = new();
            var data = new List<Trip>() { new Trip() { IdTrip = 1 ,
        Members=new List<TripMember>(){
          new TripMember() { IdTrip = 1, IdTripMember = 1 },
          new TripMember() { IdTrip = 1, IdTripMember = 2 }
        } } }.AsQueryable();
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
        new TripUser() { IdTrip = 1, IdUser = 1, Role = Library.Model.Utilities.Enums.TripRole.Owner }
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
            var data = new List<TripMember>() {
        new TripMember() { IdTrip = 1, IdTripMember = 1 },
      new TripMember() { IdTrip = 1, IdTripMember = 2 } }.AsQueryable();
            mock.As<IQueryable<TripMember>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripMember>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripMember>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripMember>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripTransaction>> GetMockTripTransaction()
        {
            Mock<DbSet<TripTransaction>> mock = new();
            var data = new List<TripTransaction>() { }.AsQueryable();
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripTransaction>> GetMockTripTransactionUpdate()
        {
            Mock<DbSet<TripTransaction>> mock = new();
            var data = new List<TripTransaction>() {
        new TripTransaction(){IdTripTransaction =1,
          IdTrip = 1,
       IdPayer=1 ,
        TripDebtors = new List<TripDebtor>(){
        new TripDebtor(){IdTripTransaction =1 ,IdDebtor = 1 } } }
       }.AsQueryable();
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripTransaction>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }
        private static Mock<DbSet<TripDebtor>> GetMockTripDebtorUpdate()
        {
            Mock<DbSet<TripDebtor>> mock = new();
            var data = new List<TripDebtor>() {
        new TripDebtor(){IdTripTransaction =1 ,IdDebtor = 1 }
       }.AsQueryable();
            mock.As<IQueryable<TripDebtor>>().Setup(m => m.Provider).Returns(data.Provider);
            mock.As<IQueryable<TripDebtor>>().Setup(m => m.Expression).Returns(data.Expression);
            mock.As<IQueryable<TripDebtor>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mock.As<IQueryable<TripDebtor>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());
            return mock;
        }

    }
}
