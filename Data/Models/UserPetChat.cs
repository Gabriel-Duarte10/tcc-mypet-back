using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class UserPetChat : BaseEntity
    {
        public int UserPetChatSessionId { get; set; }  // Foreign Key
        public int SenderUserId { get; set; }

        public string? Text { get; set; }
        public string? Image64 { get; set; }

        // Navigation Properties
        public User SenderUser { get; set; }
        public UserPetChatSession UserPetChatSession { get; set; }
    }

}