using AutoMapper;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Helpers
{
    public class AnimalTypeProfile : Profile
    {
        public AnimalTypeProfile()
        {
            CreateMap<AnimalType, AnimalTypeDTO>().ReverseMap();
            CreateMap<AnimalTypeRequest, AnimalType>().ReverseMap();
        }
    }
}
