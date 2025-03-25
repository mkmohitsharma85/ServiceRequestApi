using ServiceRequest.Common.Enum;

namespace ServiceRequest.Common.RequestDTO
{
    public class ServiceRequestUpdateDTO
    {
        public Guid Id { get; set; }    
        public string? BuildingCode { get; set; }
        public string? Description { get; set; }
        public CurrentStatus CurrentStatus { get; set; }
    }
}
