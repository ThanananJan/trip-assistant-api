using JWTAuthentication.Library.Model.DB;
using Microsoft.Extensions.DependencyModel.Resolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Library.Model.Dto.Trip;
using TripAssistant.Library.Services;
using TripAssistant.Test.Utility;

namespace TripAssistant.Test
{
    [TestFixture]
    internal class TripQuery
    {
        [Test]
        public void GetTrips_SetUserNoTrip_ReturnEmpty()
        {
            //Arrange
            var idUser = 0;
            var expected = 0;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTrips(idUser);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(expected));
        }
        [Test]
        public void GetTrips_SetUserHave1Trip_Retrun1Trip()
        {
            //Arrange
            var idUser = 1;
            var expected = 1;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTrips(idUser);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(expected));
        }
        [Test]
        public void GetTrips_SetTrip2Member_Return1Trip2Members()
        {
            //Arrange
            var idUser = 2;
            var expected = 2;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTrips(idUser);

            //Assert
            Assert.That(result.SelectMany(p => p.Members).Count(), Is.EqualTo(expected));
        }

        [Test]
        public void GetTripTransactions_SetInvalidTrip_RetrunEmpty()
        {
            //Arrange
            var idTrip = 0;
            var expected = 0;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTripTransactions(idTrip);

            //Assert
            Assert.That(result.Count(), Is.EqualTo(expected));
        }
        [Test]
        public void GetTripTransactions_Set1Transaction1Debtor_Return1Transaction1Debtor()
        {
            //Arrange
            var idTrip = 1;
            var expectedTransaction = 1;
            var expectedDebtor = 1;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTripTransactions(idTrip);

            //Assert
#pragma warning disable NUnit2045 // Use Assert.Multiple
            Assert.That(result.Count(), Is.EqualTo(expectedTransaction));
#pragma warning restore NUnit2045 // Use Assert.Multiple
            Assert.That(result.SelectMany(p => p.TripDebtors).Count(), Is.EqualTo(expectedDebtor));
            Assert.That(result.First().TripPayer.NamPayer, Is.Not.EqualTo(string.Empty));
            Assert.That(result.SelectMany(p => p.TripDebtors).First().NamDebtor, Is.Not.EqualTo(string.Empty));
        }

        [Test]
        public void GetTripTransactions_Set1Transaction2Debtor_Return1Transaction2Debtor()
        {
            //Arrange
            var idTrip = 2;
            var expectedTransaction = 1;
            var expectedDebtor = 2;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTripTransactions(idTrip);

            //Assert
            Assert.That(result.Count, Is.EqualTo(expectedTransaction));
            Assert.That(result.SelectMany(p => p.TripDebtors).Count(), Is.EqualTo(expectedDebtor));
        }
        [Test]
        public void GetTripSummary_SetInvalidTrip_RetrunNull()
        {
            //Arrange
            var idTrip = 0;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTripSummary(idTrip);

            //Assert
            Assert.That(result, Is.Null);
        }
        [Test]
        public void GetTripSummary_Set1Trip1Member1Transaction1Debtor_ReturnTrip1Member()
        {
            //Arrange
            var idTrip = 1;
            var expectedMember = 1;
            var expectedAmount = 1;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTripSummary(idTrip);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Members.Count, Is.EqualTo(expectedMember));
            var firstMember = result.Members.First();
            Assert.That(firstMember.PayerAmount, Is.EqualTo(expectedAmount));
            Assert.That(firstMember.DebtorAmount, Is.EqualTo(expectedAmount));
        }

        [Test]
        public void GetTripSummary_Set1Trip1Transactions2Member_ReturnTrip2Member()
        {
            //Arrange
            var idTrip = 2;
            var expectedMember = 2;
            var expected1PayerAmount = 1;
            var expected1DebtorAmount = 0.5;
            var expected2DebtorAmount = 0.5;
            var mockContext = AddTripInitial.MockContextTripQuery();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.GetTripSummary(idTrip);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Members.Count, Is.EqualTo(expectedMember));
            var firstMember = result.Members.First();
            Assert.That(firstMember.PayerAmount, Is.EqualTo(expected1PayerAmount));
            Assert.That(firstMember.DebtorAmount, Is.EqualTo(expected1DebtorAmount));
            var secondMember = result.Members.Last();
            Assert.That(secondMember.PayerAmount, Is.EqualTo(0));
            Assert.That(secondMember.DebtorAmount, Is.EqualTo(expected2DebtorAmount));
        }

    }
}
