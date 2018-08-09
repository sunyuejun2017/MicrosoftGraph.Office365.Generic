﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Models
{
    public class IdentitySet
    {
        public Identity Application { get; set; }
        public Identity Device { get; set; }
        public Identity User { get; set; }
    }
}