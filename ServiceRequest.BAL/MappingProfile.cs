using AutoMapper;
using ServiceRequest.Common.RequestDTO;
using ServiceRequest.Common.ResponseDTO;
using ServiceRequest.DAL.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequest.BAL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ServiceRequestEntity, ServiceRequestDTO>();

            CreateMap<ServiceRequestCreate, ServiceRequestEntity>();
            CreateMap<ServiceRequestUpdateDTO, ServiceRequestEntity>();
        }
    }
}
