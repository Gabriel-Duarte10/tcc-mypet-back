using AutoMapper;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Helpers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserImages, opt => opt.MapFrom(src => src.UserImage))
                .ReverseMap();
            CreateMap<UserImage, UserImageDto>().ReverseMap();
            CreateMap<UserCreateRequest, User>();
            CreateMap<UserUpdateRequest, User>();
            CreateMap<UserImageRequest, UserImage>().ReverseMap();
        }
    }
}
