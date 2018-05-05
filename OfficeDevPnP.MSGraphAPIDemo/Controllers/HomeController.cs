using OfficeDevPnP.MSGraphAPIDemo.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeDevPnP.MSGraphAPIDemo.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            var result = MicrosoftGraphHelper.MakeGetRequestForString(MSGraphAPIDemoSettings.MicrosoftGraphResourceId + "/v1.0/me");
            return View();
        }
    }
}