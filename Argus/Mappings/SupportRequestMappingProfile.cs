using AutoMapper;
using Argus.Models;
using Argus.Dtos.SupportRequest;
using Argus.Dtos.SupportRequest.Comments;

namespace Argus.Mappings
{
    public class SupportRequestMappingProfile : Profile
    {
        public SupportRequestMappingProfile()
        {
            CreateMap<CreateSupportRequestDto, SupportRequest>();
            CreateMap<SupportRequest, SupportRequestDetailDto>();
            CreateMap<SupportRequest, SupportRequestListDto>()
                .ForMember(
                    dest => dest.RoomNumber,
                    opt => opt.MapFrom(src => src.Auditorium!.RoomNumber));
            CreateMap<SupportRequestComment, SupportRequestCommentDto>()
                .ForMember(
                    dest => dest.AuthorName,
                    opt => opt.MapFrom(src => src.Author!.FullName));
        }
    }
}