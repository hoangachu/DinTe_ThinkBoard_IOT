using DINTEIOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Helpers;

namespace DINTEIOT.Models.Account
{
    public class Account
    {
        public int accountID { get; set; }
        public string fullName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int roleid { get; set; }
        public Organ organ { get; set; }
        public string position { get; set; }
        public int phoneNumber { get; set; }
        public string email { get; set; }
        public int totalrecord { get; set; }
        public long rownumber { get; set; }
        public int status { get; set; }
        public bool rememberme { get; set; }
    }
    public class AccountFilter : OptionFilter
    {
        public int organID { get; set; }
    }
}
