using AutoMapper;
using tcc_mypet_back.Data.Dtos;
using tcc_mypet_back.Data.Models;
using tcc_mypet_back.Data.Request;

namespace tcc_mypet_back.Helpers
{
    public class AdministratorMappingProfile : Profile
    {
        public AdministratorMappingProfile()
        {
            CreateMap<Administrator, AdministratorDto>();

            CreateMap<AdministratorRequest, Administrator>();
        }
    }
}
