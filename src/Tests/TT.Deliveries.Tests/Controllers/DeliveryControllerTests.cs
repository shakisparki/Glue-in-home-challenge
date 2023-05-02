using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading.Tasks;
using TT.Deliveries.Core.Enums;
using TT.Deliveries.Data.Dto;
using TT.Deliveries.Services.Contracts;
using TT.Deliveries.Services.Responses;
using TT.Deliveries.Web.Api.Controllers;
using TT.Deliveries.Web.Api.Models;
using Xunit;

namespace TT.Deliveries.Web.Api.Tests.Controllers
{
    public class DeliveriesControllerTests
    {
        private readonly DeliveriesController _controller;
        private readonly Mock<IDeliveryService> _deliveryServiceMock;
        private readonly Mock<ILogger<DeliveriesController>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public DeliveriesControllerTests()
        {
            _deliveryServiceMock = new Mock<IDeliveryService>();
            _loggerMock = new Mock<ILogger<DeliveriesController>>();
            _mapperMock = new Mock<IMapper>();

            _controller = new DeliveriesController(
                _deliveryServiceMock.Object,
                _loggerMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task GetById_Should_Return_404_If_Delivery_Doesnt_Exist()
        {
            // Arrange
            var id = Guid.NewGuid();
            var response = new Response<DeliveryDto>(Errors.NotFound);
            _deliveryServiceMock.Setup(x => x.ReadAsync(id)).ReturnsAsync(response).Verifiable();

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
            var notFoundResult = (NotFoundResult)result.Result;
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Mock.VerifyAll();
        }

        [Fact]
        public async Task GetById_Should_Return_Delivery_Details()
        {
            // Arrange
            var id = Guid.NewGuid();
            var deliveryDto = new DeliveryDto
            {
                Id = id,
                Order = new OrderDto
                {
                    OrderNumber = "12345"
                }
                // Set other properties
            };
            var delivery = new Delivery
            {
                Order = new Order
                {
                    OrderNumber = "12345"
                }
                // Set other properties
            };
            var response = new Response<DeliveryDto>(deliveryDto);
            _deliveryServiceMock.Setup(x => x.ReadAsync(id)).ReturnsAsync(response).Verifiable();
            _mapperMock.Setup(x => x.Map<Delivery>(deliveryDto)).Returns(delivery).Verifiable();

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = (OkObjectResult)result.Result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(delivery, okResult.Value);
            Mock.VerifyAll();
        }

        ///Many more tests for each controller methods here, then
        /// Test projects for other projects eg. TT.Deliveries.Auth, TT.Deliveries.Services etc.
        /// and even more test classes and methods for each of them.
    }
}
