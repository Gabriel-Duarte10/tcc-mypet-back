using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class UserProductChatSession: BaseEntity
    {
        // Foreign Keys
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public int ProductId { get; set; }     // Pode ser nulo se for um chat de pet

        // Navigation Properties
        public User User1 { get; set; }
        public User User2 { get; set; }
        public Product Product { get; set; }
    }
}