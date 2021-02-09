using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Areas.General.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /General/Report/
        public PartialViewResult _Index()
        {
            return PartialView();
        }
        public PartialViewResult _Search()
        {
            return PartialView();
        }
        public PartialViewResult _Report()
        {
            return PartialView();
        }
	}
}