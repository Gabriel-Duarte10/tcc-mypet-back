using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class ReportedProduct : BaseEntity
    {
        // Foreign Key
        public int ProductId { get; set; }

        public int Counter { get; set; }

        // Navigation Property
        public Product Product { get; set; }
    }

}