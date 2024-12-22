using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Services;
using TripAssistant.Test.Utility;

namespace TripAssistant.Test
{
    [TestFixture]
    internal class DeleteTrip
    {
        [TestCase(0)]
        public void DeleteTrip_SetInvalidTrip_ReturnInvalidData(int idTrip)
        {
            //Arrange
            var expected = "invalid data";

            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.DeleteTrip(idTrip));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [TestCase(1)]
        public void DeleteTripSetValidTrip_ReturnSuccess(int idTrip)
        {
            //Arrange
            var mockContext = AddTripTransactionInitial.MockContextTripTransactionUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.DeleteTrip(idTrip);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}
