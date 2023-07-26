using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class Size : BaseEntity
    {
        public string Name { get; set; }

        // Foreign Key
        public int AdministratorId { get; set; }

        // Navigation Property
        public Administrator Administrator { get; set; }
    }

}