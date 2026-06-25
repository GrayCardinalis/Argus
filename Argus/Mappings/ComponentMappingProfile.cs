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
            CreateMap<UpdateComponentStockDto, Component>()
                //Если в пришедшем DTO какое - то поле равно null, проигнорируй его и не перезаписывай им старое значение в базе данных
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 
        }
    }
}
