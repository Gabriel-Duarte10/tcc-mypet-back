using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class FavoritePet : BaseEntity
    {
        // Foreign Keys
        public int PetId { get; set; }
        public int UserId { get; set; }

        // Navigation Properties
        public Pet Pet { get; set; }
        public User User { get; set; }
    }

}