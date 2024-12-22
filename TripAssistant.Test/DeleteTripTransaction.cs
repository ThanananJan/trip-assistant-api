
using TripAssistant.Library.Services;
using TripAssistant.Test.Utility;

namespace TripAssistant.Test
{
    [TestFixture]
    internal class DeleteTripTransaction
    {
        [TestCase(0)]
        public void DeleteTripTransaction_SetInvalidTripTransaction_ReturnInvalidData(int idTripTransaction)
        {
            //Arrange
            var expected = "invalid data";

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.DeleteTripTransaction(idTripTransaction));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [TestCase(1)]
        public void DeleteTripTransaction_SetValidTripTransaction_ReturnSuccess(int idTripTransaction)
        {
            //Arrange
            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.DeleteTripTransaction(idTripTransaction);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}
