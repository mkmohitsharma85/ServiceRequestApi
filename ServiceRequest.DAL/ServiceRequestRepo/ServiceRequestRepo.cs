using Microsoft.EntityFrameworkCore;
using ServiceRequest.DAL.EntityModel;
using ServiceRequest.DAL.IServiceRequest;

namespace ServiceRequest.DAL.ServiceRequestRepo
{
    public class ServiceRequestRepo : IServiceRequestRepo
    {
        public readonly ApplicationDBContext _db;
        public ServiceRequestRepo(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<Guid> Create(ServiceRequestEntity serviceRequest)
        {
            await _db.ServiceRequest.AddAsync(serviceRequest);
            await _db.SaveChangesAsync();

            return serviceRequest.Id;
        }

        public async Task<bool> Delete(Guid id)
        {
            var serviceRequest = await _db.ServiceRequest.FirstOrDefaultAsync(x => x.Id == id);

            if (serviceRequest == null)
            {
                return false;
            }

            _db.ServiceRequest.Remove(serviceRequest);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<ServiceRequestEntity> Get(Guid id)
        {
            return await _db.ServiceRequest.FirstOrDefaultAsync(x => x.Id == id) ?? new ServiceRequestEntity();
        }

        public async Task<List<ServiceRequestEntity>> GetAll()
        {
            return await _db.ServiceRequest.ToListAsync();
        }

        public async Task<ServiceRequestEntity> Update(ServiceRequestEntity updateRequest)
        {
            var serviceRequest = await _db.ServiceRequest.FirstOrDefaultAsync(x => x.Id == updateRequest.Id);

            if (serviceRequest == null)
            {
                return null;
            }

            if (serviceRequest.CurrentStatus != updateRequest.CurrentStatus)
            {
                serviceRequest.CurrentStatus = updateRequest.CurrentStatus;
            }
            if (serviceRequest.BuildingCode != updateRequest.BuildingCode)
            {
                serviceRequest.BuildingCode = updateRequest.BuildingCode;
            }
            if (serviceRequest.Description != updateRequest.Description)
            {
                serviceRequest.Description = updateRequest.Description;
            }
            serviceRequest.LastModifiedDate = DateTime.Now;


            await _db.SaveChangesAsync();

            return serviceRequest;
        }
    }
}
