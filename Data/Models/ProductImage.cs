using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class ProductImage : BaseEntity
    {
        // Foreign Key
        public int ProductId { get; set; }

        public string ImageName { get; set; }
        public string Image64 { get; set; }

        // Navigation Property
        public Product Product { get; set; }
    }

}