using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class FavoriteProduct : BaseEntity
    {
        // Foreign Keys
        public int ProductId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public Product Product { get; set; }
        public User User { get; set; }
    }

}