using ServiceRequest.Common.RequestDTO;
using ServiceRequest.Common.ResponseDTO;

namespace ServiceRequest.BAL.IServiceRequest.BAL
{
    public interface IServiceRequestService
    {
        Task<List<ServiceRequestDTO>> GetAll();
        Task<ServiceRequestDTO> Get(Guid id);

        Task<Guid> Create(ServiceRequestCreate serviceRequest);

        Task<bool> Delete(Guid id);

        Task<ServiceRequestDTO> Update(ServiceRequestUpdateDTO updateDTO);

    }
}
