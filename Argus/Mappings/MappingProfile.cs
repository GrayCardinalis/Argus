using AutoMapper;
using Argus.Dtos;
using Argus.Models;
using Argus.Dtos.Auditoriums;

namespace Argus.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Auditorium, AuditoriumDto>();
            CreateMap<CreateAuditoriumDto, Auditorium>();
        }
    }
}
