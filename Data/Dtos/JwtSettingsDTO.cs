using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Dtos
{
    public class JwtSettingsDTO
    {
        public string SecurityKey { get; set; }
        public int HoursExpire { get; set; }
    }
}