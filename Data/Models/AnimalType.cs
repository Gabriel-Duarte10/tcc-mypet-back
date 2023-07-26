using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class AnimalType : BaseEntity
    {
        public string Name { get; set; }
        public int AdministratorId { get; set; }
        public Administrator Administrator { get; set; }
    }

}