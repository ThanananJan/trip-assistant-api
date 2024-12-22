using JWTAuthentication.Library.Model.DB;
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
    internal class AssignTrip
    {
        [TestCase(1)]
        public void AssignTrip_SetExistUser_ReturnInvalidData(int idUser)
        {
            //Arrange
            var idTrip = 1;
            var expected = "invalid data";
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.AssignTrip(idTrip, idUser));
            Assert.That(result.Message, Is.EqualTo(expected));
        }


        [TestCase(2)]
        public void AssignTrip_SetValidUser_ReturnSuccess(int idUser)
        {
            //Arrange
            var idTrip = 1;

            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.AssignTrip(idTrip, idUser);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
        [TestCase(0)]
        public void AssignTrip_SetInvalidTrip_ReturnInvalidData(int idTrip)
        {  //Arrange
            var idUser = 2;
            var expected = "invalid data";
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.AssignTrip(idTrip, idUser));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [TestCase(1)]
        public void AssignTrip_SetValidTrip_ReturnSuccess(int idTrip)
        {
            //Arrange
            var idUser = 2;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.AssignTrip(idTrip, idUser);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
    }
}
