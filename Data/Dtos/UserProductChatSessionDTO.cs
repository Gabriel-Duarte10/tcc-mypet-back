using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Dtos
{
    public class UserProductChatSessionDTO
    {
        public UserDto User1 { get; set; }
        public UserDto User2 { get; set; }
        public ProductDTO Product { get; set; }
    }

    public class UserProductChatDTO
    {
        public int UserProductChatSessionId { get; set; }
        public UserDto SenderUser{ get; set; }
        public string? Text { get; set; }
        public string? Image64 { get; set; }
    }
}