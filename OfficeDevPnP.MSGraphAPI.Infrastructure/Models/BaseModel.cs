using System;

namespace OfficeDevPnP.MSGraphAPI.Infrastructure
{
    /// <summary>
    /// Base class for any entity in the model
    /// </summary>
    public abstract class BaseModel
    {
        /// <summary>
        /// The unique ID of the entity
        /// </summary>
        public String Id { get; set; }
    }
}