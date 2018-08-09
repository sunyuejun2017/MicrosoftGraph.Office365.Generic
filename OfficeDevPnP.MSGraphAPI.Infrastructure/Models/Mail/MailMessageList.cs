using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure.Models
{
    /// <summary>
    /// Defines a list of email messages
    /// </summary>
    public class MailMessageList
    {
        /// <summary>
        /// The list of messages
        /// </summary>
        [JsonProperty("value")]
        public List<MailMessage> Messages { get; set; }
    }
}