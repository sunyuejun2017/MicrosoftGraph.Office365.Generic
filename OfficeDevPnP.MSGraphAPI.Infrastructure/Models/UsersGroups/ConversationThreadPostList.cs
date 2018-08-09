using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Models
{
    public class ConversationThreadPostsList
    {
        /// <summary>
        /// The list of threads
        /// </summary>
        [JsonProperty("value")]
        public List<ConversationThreadPost> Posts { get; set; }
    }
}