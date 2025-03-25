using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ServiceRequest.BAL.IServiceRequest.BAL;
using ServiceRequest.Common.Enum;
using ServiceRequest.Common.RequestDTO;
using ServiceRequest.Common.ResponseDTO;
using ServiceRequestApi.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace ServiceRequestManager.UnitTest
{

    public class ServiceRequestControllerTest
    {
        private readonly Mock<IServiceRequestService> _serviceRequestServiceMock;
        private readonly Mock<ILogger<ServiceRequestController>> _loggerMock;
        private readonly ServiceRequestController _controller;

        public ServiceRequestControllerTest()
        {
            _serviceRequestServiceMock = new Mock<IServiceRequestService>();
            _loggerMock = new Mock<ILogger<ServiceRequestController>>();
            _controller = new ServiceRequestController(_serviceRequestServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfServiceRequests()
        {
            // Arrange
            var mockServiceRequests = new List<ServiceRequestDTO>
             {
                 new ServiceRequestDTO { Id = Guid.NewGuid(), Description = "Test Request 1", BuildingCode = "B001", CurrentStatus = "Pending", CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now },
                 new ServiceRequestDTO { Id = Guid.NewGuid(), Description = "Test Request 2", BuildingCode = "B002", CurrentStatus = "Completed", CreatedDate = DateTime.Now, LastModifiedDate = DateTime.Now }
             };

            // Setup the mock service to return the list of service requests
            _serviceRequestServiceMock.Setup(s => s.GetAll()).ReturnsAsync(mockServiceRequests);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseDTO<List<ServiceRequestDTO>>>(okResult.Value);
            Assert.Equal(HttpStatusCode.OK, response.Code);
            Assert.Equal("successful", response.Message);
            Assert.NotNull(response.Response.Data);
            Assert.Equal(2, response.Response.Data.Count);
        }

        [Fact]
        public async Task Get_ReturnsExpectedResult()
        {
            var emptyGuid = Guid.Empty;
            var serviceRequestId = Guid.NewGuid();
            var mockServiceRequest = new ServiceRequestDTO { Id = Guid.Empty };

            _serviceRequestServiceMock.Setup(s => s.Get(serviceRequestId)).ReturnsAsync(mockServiceRequest);

            var resultEmptyGuid = await _controller.Get(emptyGuid);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultEmptyGuid);
            var responseBadRequest = Assert.IsType<ResponseDTO<ServiceRequestDTO>>(badRequestResult.Value);
            Assert.Equal(HttpStatusCode.BadRequest, responseBadRequest.Code);
            Assert.Equal("Id can't be null", responseBadRequest.Message);

            var resultNotFound = await _controller.Get(serviceRequestId);
            var okResultNotFound = Assert.IsType<OkObjectResult>(resultNotFound);
            var responseNotFound = Assert.IsType<ResponseDTO<ServiceRequestDTO>>(okResultNotFound.Value);
            Assert.Equal(HttpStatusCode.NoContent, responseNotFound.Code);
            Assert.Equal("Data Not Found For Id" + serviceRequestId, responseNotFound.Message);

            var mockValidServiceRequest = new ServiceRequestDTO
            {
                Id = serviceRequestId,
                Description = "Test Request",
                BuildingCode = "B001",
                CurrentStatus = "Pending",
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };
            _serviceRequestServiceMock.Setup(s => s.Get(serviceRequestId)).ReturnsAsync(mockValidServiceRequest);

            var resultValidRequest = await _controller.Get(serviceRequestId);
            var okResultValid = Assert.IsType<OkObjectResult>(resultValidRequest);
            var responseValid = Assert.IsType<ResponseDTO<ServiceRequestDTO>>(okResultValid.Value);
            Assert.Equal(HttpStatusCode.OK, responseValid.Code);
            Assert.Equal("successful", responseValid.Message);
            Assert.NotNull(responseValid.Response.Data);
            Assert.Equal(serviceRequestId, responseValid.Response.Data.Id);
        }

        [Fact]
        public async Task Create_ReturnsExpectedResult()
        {
            var emptyServiceRequest = (ServiceRequestCreate)null;
            var serviceRequest = new ServiceRequestCreate
            {
                Description = "Test Request",
                BuildingCode = "B001",
                CurrentStatus = CurrentStatus.InProgress,
                CreatedBy = "User1"
            };

            var createdServiceRequestId = Guid.NewGuid();
            _serviceRequestServiceMock.Setup(s => s.Create(serviceRequest)).ReturnsAsync(createdServiceRequestId);

            var resultNullModel = await _controller.Create(emptyServiceRequest);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultNullModel);
            var responseBadRequest = Assert.IsType<ResponseDTO<Guid>>(badRequestResult.Value);
            Assert.Equal(HttpStatusCode.BadRequest, responseBadRequest.Code);
            Assert.Equal("Model can't be null", responseBadRequest.Message);

            var resultValidRequest = await _controller.Create(serviceRequest);
            var okResult = Assert.IsType<OkObjectResult>(resultValidRequest);
            var responseValid = Assert.IsType<ResponseDTO<Guid>>(okResult.Value);
            Assert.Equal(HttpStatusCode.Created, responseValid.Code);
            Assert.Equal("successful", responseValid.Message);
            Assert.NotNull(responseValid.Response.Data);
            Assert.Equal(createdServiceRequestId, responseValid.Response.Data);
        }

        [Fact]
        public async Task Update_ReturnsExpectedResult()
        {
            var emptyServiceRequest = (ServiceRequestUpdateDTO)null;
            var serviceRequestWithEmptyId = new ServiceRequestUpdateDTO { Id = Guid.Empty };
            var validServiceRequest = new ServiceRequestUpdateDTO
            {
                Id = Guid.NewGuid(),
                Description = "Updated Request",
                BuildingCode = "B002",
                CurrentStatus = CurrentStatus.Created
            };

            var updatedServiceRequest = new ServiceRequestDTO
            {
                Id = validServiceRequest.Id,
                Description = validServiceRequest.Description,
                BuildingCode = validServiceRequest.BuildingCode,
                CurrentStatus = validServiceRequest.CurrentStatus.ToString()
            };

            _serviceRequestServiceMock.Setup(s => s.Update(validServiceRequest)).ReturnsAsync(updatedServiceRequest);

            var resultNullModel = await _controller.Update(emptyServiceRequest);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultNullModel);
            var responseBadRequest = Assert.IsType<ResponseDTO<ServiceRequestDTO>>(badRequestResult.Value);
            Assert.Equal(HttpStatusCode.BadRequest, responseBadRequest.Code);
            Assert.Equal("Model can't be null", responseBadRequest.Message);

            var resultEmptyId = await _controller.Update(serviceRequestWithEmptyId);
            var badRequestResultEmptyId = Assert.IsType<BadRequestObjectResult>(resultEmptyId);
            var responseBadRequestEmptyId = Assert.IsType<ResponseDTO<ServiceRequestDTO>>(badRequestResultEmptyId.Value);
            Assert.Equal(HttpStatusCode.BadRequest, responseBadRequestEmptyId.Code);
            Assert.Equal("Id can't be null", responseBadRequestEmptyId.Message);

            var updatedValidRequest = new ServiceRequestUpdateDTO { Id = validServiceRequest.Id, Description = "Updated Request" };
            _serviceRequestServiceMock.Setup(s => s.Update(updatedValidRequest)).ReturnsAsync(updatedServiceRequest);

            var resultValidRequest = await _controller.Update(updatedValidRequest);
            var okResultValid = Assert.IsType<OkObjectResult>(resultValidRequest);
            var responseValid = Assert.IsType<ResponseDTO<ServiceRequestDTO>>(okResultValid.Value);
            Assert.Equal(HttpStatusCode.OK, responseValid.Code);
            Assert.Equal("successful", responseValid.Message);
            Assert.NotNull(responseValid.Response.Data);
            Assert.Equal(updatedServiceRequest.Id, responseValid.Response.Data.Id);
        }

        [Fact]
        public async Task Delete_ReturnsExpectedResult()
        {
            var emptyGuid = Guid.Empty;
            var validGuid = Guid.NewGuid();

            _serviceRequestServiceMock.Setup(s => s.Delete(validGuid)).ReturnsAsync(true);
            _serviceRequestServiceMock.Setup(s => s.Delete(emptyGuid)).ReturnsAsync(false);

            var resultEmptyGuid = await _controller.Delete(emptyGuid);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(resultEmptyGuid);
            var responseBadRequest = Assert.IsType<ResponseDTO<bool>>(badRequestResult.Value);
            Assert.Equal(HttpStatusCode.BadRequest, responseBadRequest.Code);
            Assert.Equal("Id can't be null", responseBadRequest.Message);

            var resultDeleteSuccess = await _controller.Delete(validGuid);
            var okResultSuccess = Assert.IsType<OkObjectResult>(resultDeleteSuccess);
            var responseSuccess = Assert.IsType<ResponseDTO<bool>>(okResultSuccess.Value);
            Assert.Equal(HttpStatusCode.OK, responseSuccess.Code);
            Assert.Equal("successful", responseSuccess.Message);
            Assert.True(responseSuccess.Response.Data);
        }
    }
}
