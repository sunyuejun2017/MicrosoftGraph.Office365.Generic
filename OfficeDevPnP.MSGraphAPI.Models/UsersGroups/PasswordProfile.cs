using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Models
{
    public class PasswordProfile
    {
        public String Password { get; set; }

        public Boolean ForceChangePasswordNextSignIn { get; set; }
    }
}