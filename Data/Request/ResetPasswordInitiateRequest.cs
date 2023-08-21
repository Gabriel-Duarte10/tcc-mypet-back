using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tcc_mypet_back.Data.Request
{
    public class ResetPasswordInitiateRequest
    {
        public string Email { get; set; }
    }

    public class ResetPasswordCompleteRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public int CellphoneCode { get; set; }
    }
}