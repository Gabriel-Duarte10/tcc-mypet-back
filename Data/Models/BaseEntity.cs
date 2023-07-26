using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Models
{
    public class BaseEntity
    {
        public virtual int Id { get; set; }
        public virtual DateTime? CreatedAt { get; set; }
        public virtual DateTime? UpdatedAt { get; set; }
        public virtual DateTime? DeleteAt { get; set; }
    }
}