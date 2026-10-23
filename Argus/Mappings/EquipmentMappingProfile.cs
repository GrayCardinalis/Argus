using AutoMapper;
using Argus.Models;
using Argus.Dtos.Equipment;

namespace Argus.Mappings
{
    public class EquipmentMappingProfile : Profile
    {
        public EquipmentMappingProfile()
        {
            CreateMap<Equipment, EquipmentListDto>();
            CreateMap<Equipment, EquipmentDetailDto>();
            CreateMap<CreateEquipmentDto, Equipment>();
            CreateMap<UpdateEquipmentDto, Equipment>();
        }   
    }
}
