﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Models
{
    /// <summary>
    /// This type defines a new member to add to a group
    /// </summary>
    public class GroupMemberToAdd
    {
        [JsonProperty("@odata.id")]
        public String ObjectId { get; set; }
    }
}