﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Models
{
    /// <summary>
    /// Defines a list of email message folders
    /// </summary>
    public class MailFolderList
    {
        /// <summary>
        /// The list of email message folders
        /// </summary>
        [JsonProperty("value")]
        public List<MailFolder> Folders { get; set; }
    }
}