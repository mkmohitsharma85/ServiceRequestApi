using ServiceRequest.DAL.EntityModel;

namespace ServiceRequest.DAL.IServiceRequest
{
    public interface IServiceRequestRepo
    {
        Task<List<ServiceRequestEntity>> GetAll();

        Task<ServiceRequestEntity> Get(Guid id);

        Task<Guid> Create(ServiceRequestEntity serviceRequest);
        Task<bool> Delete(Guid id);

        Task<ServiceRequestEntity> Update(ServiceRequestEntity updateRequest);
    }
}
