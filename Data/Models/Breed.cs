using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class Breed : BaseEntity
    {
        public string Name { get; set; }

        // Foreign Keys
        public int AdministratorId { get; set; }
        public int AnimalTypeId { get; set; }

        // Navigation Properties
        public Administrator Administrator { get; set; }
        public AnimalType AnimalType { get; set; }
    }

}