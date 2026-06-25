using AutoMapper;
using Argus.Models;
using Argus.Dtos.Auditoriums;

namespace Argus.Mappings
{
    public class AuditoriumMappingProfile : Profile
    {
        public AuditoriumMappingProfile() 
        {
            CreateMap<Auditorium, AuditoriumDto>();
            CreateMap<CreateAuditoriumDto, Auditorium>();
        }
    }
}
