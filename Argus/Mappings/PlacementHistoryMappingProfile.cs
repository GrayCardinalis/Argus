using AutoMapper;
using Argus.Models;
using Argus.Dtos.PlacementHistory;

namespace Argus.Mappings
{
    public class PlacementHistoryMappingProfile : Profile
    {
        public PlacementHistoryMappingProfile()
        {
            CreateMap<PlacementHistory, PlacementHistoryDto>()
                .ForMember(dest => dest.RoomNumber, opt => opt.MapFrom(src => src.Auditorium!.RoomNumber));
        }
    }
}
