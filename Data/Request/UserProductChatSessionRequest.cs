using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Request
{
    public class UserProductChatSessionRequest
    {
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public int ProductId { get; set; }
    }
}