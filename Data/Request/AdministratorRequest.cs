using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Request
{
    public class AdministratorRequest
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}