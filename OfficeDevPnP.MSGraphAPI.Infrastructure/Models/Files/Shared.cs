﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Models
{
    public class Shared
    {
        public IdentitySet Owner { get; set; }

        public String Scope { get; set; }
    }
}