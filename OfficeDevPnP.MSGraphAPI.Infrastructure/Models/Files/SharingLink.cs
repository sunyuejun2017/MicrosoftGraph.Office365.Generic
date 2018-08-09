using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Models
{
    public class SharingLink
    {
        public Identity Application { get; set; }
        public SharingLinkType Type { get; set; }
        public String WebUrl { get; set; }
    }
}