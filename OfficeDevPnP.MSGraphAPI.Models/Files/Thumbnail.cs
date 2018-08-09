using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Models
{
    public class Thumbnail
    {
        public System.IO.Stream Content { get; set; }
        public Int32 Height { get; set; }
        public Int32 Width { get; set; }
        public String Url { get; set; }
    }
}