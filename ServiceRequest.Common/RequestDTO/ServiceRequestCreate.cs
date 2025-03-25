using ServiceRequest.Common.Enum;

namespace ServiceRequest.Common.RequestDTO
{
    public class ServiceRequestCreate
    {
        public string? BuildingCode { get; set; }
        public string? Description { get; set; }
        public CurrentStatus CurrentStatus { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
