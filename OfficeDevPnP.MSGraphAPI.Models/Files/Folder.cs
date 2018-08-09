using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Models
{
    /// <summary>
    /// Defines a folder of OneDrive for Business
    /// </summary>
    public class Folder
    {
        public Nullable<Int32> ChildCount { get; set; }
    }
}