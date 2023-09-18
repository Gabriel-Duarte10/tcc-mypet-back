using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Helpers
{
    public class PetProfile : Profile
    {
        public PetProfile()
        {
            CreateMap<Pet, PetDTO>()
            .ForMember(dest => dest.AnimalTypeId, opt => opt.MapFrom(src => src.Breed.AnimalTypeId))
            .ReverseMap();
            CreateMap<PetImage, PetImageDTO>().ReverseMap();
            CreateMap<FavoritePet, FavoritePetDto>().ReverseMap();
            CreateMap<ReportedPet, ReportedPetDto>().ReverseMap();
            CreateMap<PetRequest, Pet>().ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<FavoritePetRequest, FavoritePet>().ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }

}