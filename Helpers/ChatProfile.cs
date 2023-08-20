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
    public class ChatProfile : Profile
    {
        public ChatProfile()
        {
            CreateMap<UserPetChatSession, UserPetChatSessionDTO>().ReverseMap();
            CreateMap<UserPetChat, UserPetChatDTO>().ReverseMap();
            CreateMap<UserProductChatSession, UserProductChatSessionDTO>().ReverseMap();
            CreateMap<UserProductChat, UserProductChatDTO>().ReverseMap();
            CreateMap<UserPetChatSessionRequest, UserPetChatSession>().ReverseMap();
            CreateMap<UserProductChatSessionRequest, UserProductChatSession>().ReverseMap();
        }
    }
}