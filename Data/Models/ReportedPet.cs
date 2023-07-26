using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class ReportedPet : BaseEntity
    {
        // Foreign Key
        public int PetId { get; set; }

        public int Counter { get; set; }

        // Navigation Property
        public Pet Pet { get; set; }
    }

}