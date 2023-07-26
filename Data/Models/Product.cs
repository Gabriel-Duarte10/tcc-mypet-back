using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        // Foreign Key
        public int UserId { get; set; }

        // Navigation Property
        public User User { get; set; }
    }

}