using Microsoft.AspNetCore.Mvc;
using Moq;
using TripAssistant.Api.Controllers;
using TripAssistant.Library.Model.Dto.Transaction;
using TripAssistant.Library.Services;
using TripAssistant.Test.Utility;

namespace TripAssistant.Test
{
    [TestFixture]
    internal class UpdateTripTransaction
    {
        [TestCase("", 1, "required dscTransaction")]
        [TestCase("น้ำมัน", -1, "required amount morethan 0")]
        [TestCase("น้ำมัน", 0, "required amount morethan 0")]

        public void UpdateTransaction_SetRequiredNull_Return400BadRequest(string dscTransaction, decimal amount, string errorMsg)
        {
            //Arrange
            var expected = 400;
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = 1,
                DscTransaction = dscTransaction,
                Amount = amount,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = 1 } }
            };
            var mockTripService = new Mock<ITripService>();
            var controller = new TripTransactionController(mockTripService.Object);
            controller.ModelState.AddValidationErrors(request);
            //Act
            var result = controller.UpdateTransaction(request) as BadRequestObjectResult;

            //Assert
            Assert.That(result?.StatusCode, Is.EqualTo(expected), errorMsg);
        }
        [TestCase(0)]
        public void UpdateTransaction_SetInvalidTripTransaction_Return400BadRequest(int idTripTransaction)
        {
            //Arrange
            var expected = "invalid data";
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = idTripTransaction,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = 1 } }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTripTransaction(request));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [TestCase(1)]
        public void UpdateTransaction_SetValidTripTransaction_ReturnSuccess(int idTripTransaction)
        {
            //Arrange
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = idTripTransaction,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = 1 } }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTripTransaction(request);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
        [TestCase(0)]
        public void UpdateTransaction_SetInvalidTrip_Return400BadRequest(int idTrip)
        {
            //Arrange
            var expected = "invalid data";
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = idTrip,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = 1 } }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionCreate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTripTransaction(request));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [TestCase(1)]
        public void UpdateTransaction_SetValidTrip_ReturnSuccess(int idTrip)
        {
            //Arrange
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = idTrip,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = 1 } }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTripTransaction(request);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }

        [TestCase(0)]
        public void UpdateTransaction_SetInvalidPayer_Return400BadRequest(int idPayer)
        {
            //Arrange
            var expected = "invalid data";
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = idPayer }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTripTransaction(request));
            Assert.That(result.Message, Is.EqualTo(expected));
        }

        [TestCase(1)]
        public void UpdateTransaction_SetValidPayer_ReturnSuccess(int idPayer)
        {
            //Arrange
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = idPayer },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = 1 } }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTripTransaction(request);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
        [TestCase(0)]
        public void UpdateTransaction_SetInvalidDebtor_Return400BadRequest(int idDebtor)
        {
            //Arrange
            var expected = "invalid data";
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = idDebtor } }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTripTransaction(request));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [TestCase(1)]
        public void UpdateTransaction_SetValid1Debtor_ReturnSuccess(int idDebtor)
        {
            //Arrange
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] { new TransactionDebtorRequestDto() { IdDebtor = idDebtor } }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTripTransaction(request);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void UpdateTransaction_Set3DebtorOver2TripMember_Return400BadRequest()
        {
            //Arrange
            var expected = "invalid data";
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] {
          new TransactionDebtorRequestDto() { IdDebtor = 1 },
          new TransactionDebtorRequestDto() { IdDebtor = 2 },
          new TransactionDebtorRequestDto() { IdDebtor = 3 }
        }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTripTransaction(request));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [Test]
        public void UpdateTransaction_Set2DebtorEqual2TripMember_ReturnSuccess()
        {
            //Arrange
            var request = new TripTransactionUpdateRequestDto()
            {
                IdTripTransaction = 1,
                IdTrip = 1,
                DscTransaction = "น้ำมัน",
                Amount = 100,
                TripPayer = new TransactionPayerRequestDto() { IdPayer = 1 },
                TripDebtors = new TransactionDebtorRequestDto[] {
          new TransactionDebtorRequestDto() { IdDebtor = 1 },
          new TransactionDebtorRequestDto() { IdDebtor = 2 }
        }
            };

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTripTransaction(request);
            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}
