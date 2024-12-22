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
    public class AddTrip
    {
        [TestCase("", "เจ้ายศ", "requird NamTrip")]
        [TestCase("เชียงใหม่", "", "requird member name")]

        public void AddTrip_SetRequiredNull_Return400BadRequest(string NamTrip, string namMember, string errorMsg)
        {
            //Arrange
            var expected = 400;
            var request = new TripCreateRequestDto()
            {
                NamTrip = NamTrip,
                Members = new[] { new TripMemeberCreateRequestDto() { NamMember = namMember } }
            };
            var userInfo = new JwtUserInfo() { IdUser = 1 };
            var mockTripService = new Mock<ITripService>();
            var mockHelper = new Mock<IJwtAuthenticationHelper>();
            mockHelper.Setup(m => m.GetUserInfoByClaimsIdentity()).Returns(userInfo);
            var controller = new TripController(mockTripService.Object);
            controller.ModelState.AddValidationErrors(request);
            controller.ModelState.AddValidationErrors(request.Members.First());
            //Act
            var result = controller.CreateTrip(request, mockHelper.Object) as BadRequestObjectResult;

            //Assert
            Assert.That(result?.StatusCode, Is.EqualTo(expected), errorMsg);

        }
        [Test]
        public void AddTrip_SetEmptyMember_Return400BadRequest()
        {
            //Arrange
            var expected = 400;
            var request = new TripCreateRequestDto()
            {
                NamTrip = "เชียงใหม่"
            };
            var mockTripService = new Mock<ITripService>();
            var mockHelper = new Mock<IJwtAuthenticationHelper>();
            mockHelper.Setup(p => p.GetUserInfoByClaimsIdentity()).Returns(new JwtUserInfo() { IdUser = 1 });
            var controller = new TripController(mockTripService.Object);
            controller.ModelState.AddValidationErrors(request);

            //Act
            var result = controller.CreateTrip(request, mockHelper.Object) as BadRequestObjectResult;

            //Assert
            Assert.That(result?.StatusCode, Is.EqualTo(expected));
        }
        [Test]
        public void AddTrip_SetValidMember_ReturnSuccess()
        {
            //Arrange
            var request = new TripCreateRequestDto()
            {
                NamTrip = "เชียงใหม่",
                Members = new[] { new TripMemeberCreateRequestDto() { NamMember = "ใจดี" } }
            };
            var idUser = 1;

            var mockContext = AddTripInitial.MockContextTripCreate();
            var servic = new TripService(mockContext.Object, ServiceConfig.Mapper, ServiceConfig.Logger);

            //Act
            var result = servic.AddTrip(request, idUser);

            //Assert
            Assert.That(result.IsSuccess, Is.True);
        }

    }
}

