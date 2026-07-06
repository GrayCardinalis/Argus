using AutoMapper;
using Argus.Models;
using Argus.Dtos.Components;

namespace Argus.Mappings
{
    public class ComponentMappingProfile : Profile
    {
        public ComponentMappingProfile() 
        {
            CreateMap<Component, ComponentDto>();
            CreateMap<CreateComponentDto, Component>();
            CreateMap<UpdateComponentStockDto, Component>();
            CreateMap<UpdateComponentNameDto, Component>();
        }
    }
}
