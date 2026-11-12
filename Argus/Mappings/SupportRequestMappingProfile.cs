using AutoMapper;
using Argus.Models;
using Argus.Dtos.SupportRequest;

namespace Argus.Mappings
{
    public class SupportRequestMappingProfile : Profile
    {
        public SupportRequestMappingProfile()
        {
            CreateMap<CreateSupportRequestDto, SupportRequest>();
            CreateMap<SupportRequest, SupportRequestDetailDto>();
        }
    }
}
