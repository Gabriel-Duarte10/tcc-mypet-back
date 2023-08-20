using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Dtos
{
    public class UserPetChatSessionDTO
    {
        public int Id { get; set; }
        public UserDto User1 { get; set; }
        public UserDto User2 { get; set; }
        public PetDTO Pet { get; set; }
    }

    public class UserPetChatDTO
    {
        public int Id { get; set; }
        public int UserPetChatSessionId { get; set; }
        public UserDto SenderUser { get; set; }
        public string? Text { get; set; }
        public string? Image64 { get; set; }
    }
}