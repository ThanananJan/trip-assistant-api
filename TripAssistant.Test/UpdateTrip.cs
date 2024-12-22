using JWTAuthentication.Library.Helpers;
using JWTAuthentication.Library.Interfaces;
using JWTAuthentication.Library.Model.Dto;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripAssistant.Api.Controllers;
using TripAssistant.Library.Model.DB;
using TripAssistant.Library.Model.Dto.Trip;
using TripAssistant.Library.Services;
using TripAssistant.Test.Utility;

namespace TripAssistant.Test
{
    [TestFixture]
    public class UpdateTrip
    {

        [TestCase("", "requird member name")]
        public void UpdateTripMember_SetRequiredNull_Return400BadRequest(string namMember, string errorMsg)
        {
            //Arrange
            var expected = 400;
            var request = new TripUpdateRequestDto()
            {
                Members = new[] { new TripMemberUpdateRequestDto() { NamMember = namMember, } }
            };
            var userInfo = new JwtUserInfo() { IdUser = 1 };
            var mockTripService = new Mock<ITripService>();
            var mockHelper = new Mock<IJwtAuthenticationHelper>();
            mockHelper.Setup(m => m.GetUserInfoByClaimsIdentity()).Returns(userInfo);
            var controller = new TripController(mockTripService.Object);
            controller.ModelState.AddValidationErrors(request);
            controller.ModelState.AddValidationErrors(request.Members.First());
            //Act
            var result = controller.UpdateTrip(request, mockHelper.Object) as BadRequestObjectResult;

            //Assert
            Assert.That(result?.StatusCode, Is.EqualTo(expected), errorMsg);
        }

        [TestCase(1)]
        public void UpdateTripMember_SetEmptyMember_Return400BadRequest(int IdTrip)
        {
            //Arrange
            var expected = 400;
            var request = new TripUpdateRequestDto()
            {
                IdTrip = IdTrip
            };
            var mockTripService = new Mock<ITripService>();
            var mockHelper = new Mock<IJwtAuthenticationHelper>();
            mockHelper.Setup(p => p.GetUserInfoByClaimsIdentity()).Returns(new JwtUserInfo() { IdUser = 1 });
            var controller = new TripController(mockTripService.Object);
            controller.ModelState.AddValidationErrors(request);

            //Act
            var result = controller.UpdateTrip(request, mockHelper.Object) as BadRequestObjectResult;

            //Assert
            Assert.That(result?.StatusCode, Is.EqualTo(expected));
        }
        [TestCase(0)]
        public void UpdateTripMember_SetInvalidTrip_ReturnInvalidData(int IdTrip)
        {
            //Arrange
            var expected = "invalid data";
            var request = new TripUpdateRequestDto()
            {
                IdTrip = IdTrip,
                Members = new[] { new TripMemberUpdateRequestDto() { NamMember = "เจ้ายศ" } }
            };
            var idUser = 1;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTrip(request, idUser));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [Test]
        public void UpdateTripMember_SetValid1NewMember_ReturnSuccess()
        {
            //Arrange
            var request = new TripUpdateRequestDto()
            {
                IdTrip = 1,
                Members = new[] { new TripMemberUpdateRequestDto() { NamMember = "เจ้ายศ" } }
            };
            var idUser = 1;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTrip(request, idUser);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
        [Test]
        public void UpdateTripMember_SetValidAll3ExistMember_ReturnSuccess()
        {
            //Arrange
            var request = new TripUpdateRequestDto()
            {
                IdTrip = 2,
                Members = new[] {
         new TripMemberUpdateRequestDto(){IdTripMember=1,NamMember="เจ้าหนี้"},
         new TripMemberUpdateRequestDto(){IdTripMember=2,NamMember="ลูกหนี้"},
         new TripMemberUpdateRequestDto(){IdTripMember=3,NamMember="คนธรรมดา"}}
            };
            var idUser = 1;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTrip(request, idUser);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
        [Test]
        public void UpdateTripMember_SetValidRemoveExistMemberNoPayer_ReturnSuccess()
        {
            //Arrange
            var idMemberNopayer = 3;
            var request = new TripUpdateRequestDto()
            {
                IdTrip = 2,
                Members = AddTripInitial.GetTripMemberUpdate().Where(p => p.IdTripMember != idMemberNopayer).ToArray()
            };
            var idUser = 1;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTrip(request, idUser);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
        [Test]
        public void UpdateTripMember_SetValidRemove1ExistMemberBePayer_ReturnInvalidData()
        {
            //Arrange
            var expected = "invalid data";
            var idMemberPayer = 1;
            var request = new TripUpdateRequestDto()
            {
                IdTrip = 2,
                Members = AddTripInitial.GetTripMemberUpdate().Where(p => p.IdTripMember != idMemberPayer).ToArray()
            };
            var idUser = 1;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTrip(request, idUser));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [Test]
        public void UpdateTripMember_SetValid1NewMemberRemove1ExistMemberNoDebtor_ReturnSuccess()
        {
            //Arrange
            var idMemberNodebtor = 3;
            var request = new TripUpdateRequestDto()
            {
                IdTrip = 2,
                Members = AddTripInitial.GetTripMemberUpdate().Where(p => p.IdTripMember != idMemberNodebtor).ToArray()
            };
            var idUser = 1;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.UpdateTrip(request, idUser);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }
        [Test]
        public void UpdateTripMember_SetValid1NewMemberRemove1ExistMemberBeDebtor_ReturnInvalidData()
        {
            //Arrange
            var expected = "invalid data";
            var idMemberDebtor = 2;
            var request = new TripUpdateRequestDto()
            {
                IdTrip = 2,
                Members = AddTripInitial.GetTripMemberUpdate().Where(p => p.IdTripMember != idMemberDebtor).ToArray()
            };
            var idUser = 1;
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTrip(request, idUser));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
        [Test]
        public void UpdateTripMember_NotOwnerTripRole_ReturnInvalid()
        {
            //Arrange
            var expected = "invalid data";
            var idUserGuest = 2;
            var request = new TripUpdateRequestDto()
            {
                IdTrip = 2,
                Members = AddTripInitial.GetTripMemberUpdate()
            };
            var mockContext = AddTripInitial.MockContextTripUpdate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act

            //Assert
            var result = Assert.Throws<InvalidDataException>(() => servic.UpdateTrip(request, idUserGuest));
            Assert.That(result.Message, Is.EqualTo(expected));
        }
    }
}

