using Microsoft.AspNetCore.Mvc;
using ServiceRequest.BAL.IServiceRequest.BAL;
using ServiceRequest.Common.RequestDTO;
using ServiceRequest.Common.ResponseDTO;
using System.Net;

namespace ServiceRequestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestController : ControllerBase
    {
        public readonly IServiceRequestService _serviceRequestService;
        private readonly ILogger<ServiceRequestController> _logger;
        public ServiceRequestController(IServiceRequestService serviceRequestService, ILogger<ServiceRequestController> logger)
        {
            _serviceRequestService = serviceRequestService;
            _logger = logger;
        }

        /// <summary>
        ///  Get All
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GetAll Request");
            var serviceRequests = await _serviceRequestService.GetAll();

            var response = new ResponseDTO<List<ServiceRequestDTO>>
            {
                Message = "successful",
                Code = HttpStatusCode.OK,
                Response = new Response<List<ServiceRequestDTO>>
                {
                    Data = serviceRequests
                }
            };
            _logger.LogInformation($"Total Records Reterived {serviceRequests.Count}");
            _logger.LogInformation("Get All Service Request Completed");
            return Ok(response);
        }

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation($"Get Request received for ID: {id}");
            if (id == Guid.Empty)
            {
                _logger.LogWarning($"Bad Request - Id can't be null: {id}");
                var res = new ResponseDTO<ServiceRequestDTO>
                {
                    Message = "Id can't be null",
                    Code = HttpStatusCode.BadRequest
                };
                return BadRequest(res);
            }
            var serviceRequest = await _serviceRequestService.Get(id);

            var response = new ResponseDTO<ServiceRequestDTO>();
            if (serviceRequest.Id == Guid.Empty)
            {
                _logger.LogWarning($"Data not found for ID: {id}");
                response = new ResponseDTO<ServiceRequestDTO>
                {
                    Message = "Data Not Found For Id" + id,
                    Code = HttpStatusCode.NoContent
                };
            }
            else
            {
                _logger.LogInformation($"Service Request found for ID: {id}");
                response = new ResponseDTO<ServiceRequestDTO>
                {
                    Message = "successful",
                    Code = HttpStatusCode.OK,
                    Response = new Response<ServiceRequestDTO>
                    {
                        Data = serviceRequest
                    }
                };
            }

            return Ok(response);
        }

        /// <summary>
        ///  Create Service Request
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceRequestCreate serviceRequest)
        {
            _logger.LogInformation("Create Request received");

            if (serviceRequest == null)
            {
                _logger.LogWarning("Bad Request - Model can't be null");
                var res = new ResponseDTO<Guid>
                {
                    Message = "Model can't be null",
                    Code = HttpStatusCode.BadRequest
                };
                return BadRequest(res);
            }

            _logger.LogInformation("Creating service request...");
            var createdServiceRequest = await _serviceRequestService.Create(serviceRequest);

            _logger.LogInformation($"Service Request created with ID: {createdServiceRequest}");

            var response = new ResponseDTO<Guid>
            {
                Message = "successful",
                Code = HttpStatusCode.Created,
                Response = new Response<Guid>
                {
                    Data = createdServiceRequest
                }
            };

            return Ok(response);
        }

        /// <summary>
        /// Update Service Request
        /// </summary>
        /// <param name="serviceRequest"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ServiceRequestUpdateDTO serviceRequest)
        {
            _logger.LogInformation("Update Request received");
            if (serviceRequest == null)
            {
                _logger.LogWarning("Bad Request - Model can't be null");
                var res = new ResponseDTO<ServiceRequestDTO>
                {
                    Message = "Model can't be null",
                    Code = HttpStatusCode.BadRequest
                };
                return BadRequest(res);
            }

            if (serviceRequest.Id == Guid.Empty)
            {
                _logger.LogWarning($"Bad Request - Id can't be null: {serviceRequest.Id}");
                var res = new ResponseDTO<ServiceRequestDTO>
                {
                    Message = "Id can't be null",
                    Code = HttpStatusCode.BadRequest
                };
                return BadRequest(res);
            }

            _logger.LogInformation($"Updating service request with ID: {serviceRequest.Id}");
            var updatedServiceRequest = await _serviceRequestService.Update(serviceRequest);

            var response = new ResponseDTO<ServiceRequestDTO>();
            if (serviceRequest.Id == Guid.Empty)
            {
                _logger.LogWarning($"No data found for ID: {serviceRequest.Id}");
                response = new ResponseDTO<ServiceRequestDTO>
                {
                    Message = "Data Not Found For Id" + serviceRequest.Id,
                    Code = HttpStatusCode.NoContent
                };
            }
            else
            {
                _logger.LogInformation($"Service Request updated with ID: {serviceRequest.Id}");
                response = new ResponseDTO<ServiceRequestDTO>
                {
                    Message = "successful",
                    Code = HttpStatusCode.OK,
                    Response = new Response<ServiceRequestDTO>
                    {
                        Data = updatedServiceRequest
                    }
                };
            }

            return Ok(response);
        }

        /// <summary>
        /// Delete Service Request
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            _logger.LogInformation($"Delete Request received for ID: {id}");
            if (id == Guid.Empty)
            {
                _logger.LogWarning($"Bad Request - Id can't be null: {id}");
                var res = new ResponseDTO<bool>
                {
                    Message = "Id can't be null",
                    Code = HttpStatusCode.BadRequest
                };
                return BadRequest(res);
            }

            _logger.LogInformation($"Attempting to delete service request with ID: {id}");
            var result = await _serviceRequestService.Delete(id);

            var response = new ResponseDTO<bool>();
            if (!result)
            {
                _logger.LogWarning($"Data not found for ID: {id}");
                response = new ResponseDTO<bool>
                {
                    Message = "Data Not Found For Id" + id,
                    Code = HttpStatusCode.NotFound
                };
            }
            else
            {
                _logger.LogInformation($"Service Request with ID {id} successfully deleted.");
                response = new ResponseDTO<bool>
                {
                    Message = "successful",
                    Code = HttpStatusCode.OK,
                    Response = new Response<bool>
                    {
                        Data = result
                    }
                };
            }

            return Ok(response);
        }
    }
}
