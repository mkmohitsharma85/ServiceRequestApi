using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using ServiceRequest.Common.Enum;
using ServiceRequest.DAL.EntityModel;
using ServiceRequest.DAL.ServiceRequestRepo;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ServiceRequestManager.UnitTest
{
    public class ServiceRequestRepoTest
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly ServiceRequestRepo _repo;

        public ServiceRequestRepoTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                            .UseInMemoryDatabase(databaseName: "TestDatabase")
                            .Options;

            _dbContext = new ApplicationDBContext(options);
            _repo = new ServiceRequestRepo(_dbContext);

            SeedDatabase();
        }

        private void SeedDatabase()
        {
            var serviceRequests = new List<ServiceRequestEntity>
        {
            new ServiceRequestEntity { Id = Guid.NewGuid(), BuildingCode = "B001", Description = "Test Request 1", CurrentStatus = CurrentStatus.InProgress },
            new ServiceRequestEntity { Id = Guid.NewGuid(), BuildingCode = "B002", Description = "Test Request 2", CurrentStatus = CurrentStatus.InProgress }
        };

            _dbContext.ServiceRequest.AddRange(serviceRequests);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task Create_ReturnsNewServiceRequestId()
        {
            var serviceRequest = new ServiceRequestEntity
            {
                Id = Guid.NewGuid(),
                BuildingCode = "B003",
                Description = "Test Request 3",
                CurrentStatus = CurrentStatus.InProgress,
                CreatedBy = "User1",
                CreatedDate = DateTime.Now
            };

            var result = await _repo.Create(serviceRequest);

            Assert.NotEqual(Guid.Empty, result);
            Assert.Equal(serviceRequest.Id, result);
        }

        [Fact]
        public async Task Delete_ReturnsTrue_WhenServiceRequestExists()
        {
            var serviceRequest = await _dbContext.ServiceRequest.FirstAsync();

            var result = await _repo.Delete(serviceRequest.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task Get_ReturnsServiceRequestEntity_WhenFound()
        {
            var serviceRequest = await _dbContext.ServiceRequest.FirstAsync();

            var result = await _repo.Get(serviceRequest.Id);

            Assert.NotNull(result);
            Assert.Equal(serviceRequest.Id, result.Id);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfServiceRequests()
        {
            var result = await _repo.GetAll();

            Assert.NotEmpty(result);
            Assert.Equal(5, result.Count);
        }
    }


}
