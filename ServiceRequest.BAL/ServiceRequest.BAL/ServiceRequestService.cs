using AutoMapper;
using ServiceRequest.BAL.IServiceRequest.BAL;
using ServiceRequest.Common.Enum;
using ServiceRequest.Common.RequestDTO;
using ServiceRequest.Common.ResponseDTO;
using ServiceRequest.DAL.EntityModel;
using ServiceRequest.DAL.IServiceRequest;

namespace ServiceRequest.BAL.ServiceRequest.BAL
{
    public class ServiceRequestService : IServiceRequestService
    {
        private readonly IMapper _mapper;
        public readonly IServiceRequestRepo _repo;
        public readonly IEmailService _emailService;
        public ServiceRequestService(IServiceRequestRepo repo, IMapper mapper, IEmailService emailService)
        {
            _repo = repo;
            _mapper = mapper;
            _emailService = emailService;
        }
        public async Task<List<ServiceRequestDTO>> GetAll()
        {
            var result = await _repo.GetAll();

            return _mapper.Map<List<ServiceRequestDTO>>(result);
        }

        public async Task<ServiceRequestDTO> Get(Guid id)
        {
            var result = await _repo.Get(id);

            return _mapper.Map<ServiceRequestDTO>(result);
        }

        public async Task<Guid> Create(ServiceRequestCreate serviceRequest)
        {
            var serviceRequestEntity = _mapper.Map<ServiceRequestEntity>(serviceRequest);

            return await _repo.Create(serviceRequestEntity);  
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repo.Delete(id);
        }

        public async Task<ServiceRequestDTO> Update(ServiceRequestUpdateDTO updateDTO)
        {
            var serviceRequestEntity = _mapper.Map<ServiceRequestEntity>(updateDTO);

            var result = await _repo.Update(serviceRequestEntity);

            if (result.CurrentStatus == CurrentStatus.Complete || result.CurrentStatus == CurrentStatus.Canceled)
            {
                string emailBody = $"Dear {result.CreatedBy},\n\n" +
                                   $"Your service request with ID {result.Id} has been marked as {result.CurrentStatus}.";
                await _emailService.SendEmailAsync("admin@gmail.com", "Service Request Status Update", emailBody);
            }

            return _mapper.Map<ServiceRequestDTO>(result);
        }
    }
}
