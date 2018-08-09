using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeDevPnP.MSGraphAPI.Models
{
    public class Photo
    {
        public String CameraMake;
        public String CameraModel;
        public Double ExposureDenominator;
        public Double ExposureNumerator;
        public Double FocalLength;
        public Double FNumber;
        public DateTimeOffset TakenDateTime;
        public Int32 ISO;   
    }
}