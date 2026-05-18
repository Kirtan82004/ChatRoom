using AutoMapper;
using Connectify.DTOs;
using Connectify.Models;

namespace Connectify.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ChatMessage, ChatMessageDto>()

                .ForMember(
                    dest => dest.UserId,
                    opt => opt.MapFrom(src => src.UserId))

                .ForMember(
                    dest => dest.UserName,
                    opt => opt.MapFrom(src => src.User.FullName));
        }
    }
}