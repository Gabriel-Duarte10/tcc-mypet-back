using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class UserProductChat : BaseEntity
    {
        // Foreign Keys
        public int SenderUserId { get; set; }
        public int ReceiverUserId { get; set; }
        public int ProductId { get; set; }

        public string Text { get; set; }
        public string Image64 { get; set; }

        // Navigation Properties
        public User SenderUser { get; set; }
        public User ReceiverUser { get; set; }
        public Product Product { get; set; }
    }

}