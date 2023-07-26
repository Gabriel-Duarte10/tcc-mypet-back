using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class Pet : BaseEntity
    {
        public string Name { get; set; }
        public int BirthMonth { get; set; }
        public int BirthYear { get; set; }
        public bool Gender { get; set; }
        public string Description { get; set; }
        public bool IsNeutered { get; set; }
        public bool IsVaccinated { get; set; }
        public bool AdoptionStatus { get; set; }

        // Foreign Keys
        public int CharacteristicId { get; set; }
        public int BreedId { get; set; }
        public int SizeId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public Characteristic Characteristic { get; set; }
        public Breed Breed { get; set; }
        public Size Size { get; set; }
        public User User { get; set; }
    }

}