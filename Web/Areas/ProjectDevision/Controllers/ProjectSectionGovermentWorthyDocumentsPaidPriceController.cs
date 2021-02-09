using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using DatabaseContext.Context;
using ViewModels.Other;
using ViewModels.ProjectDevision.ProjectSectionGovermentWorthyDocumentsPaidPriceViewModel;
using System.Web.Script.Serialization;
using Web.Utilities.AppCode;


namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionGovermentWorthyDocumentsPaidPriceController : Controller
    {
        IUnitOfWork _uow;
        IPermissionService _permissionService;
        IProjectSectionGovermentWorthyDocumentsPaidPriceService _ProjectSectionGovermentWorthyDocumentsPaidPriceService;


        #region Index & crtor
        public ProjectSectionGovermentWorthyDocumentsPaidPriceController(IUnitOfWork uow, IPermissionService permissionService, IProjectSectionGovermentWorthyDocumentsPaidPriceService ProjectSectionGovermentWorthyDocumentsPaidPriceService)
        {
            _uow = uow;
            _permissionService = permissionService;
            _ProjectSectionGovermentWorthyDocumentsPaidPriceService = ProjectSectionGovermentWorthyDocumentsPaidPriceService;
        }

        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionGovermentWorthyDocumentsPaidPrice/_Index" , Member.RoleId);
            var test = ViewBag.Permission;
            return PartialView();
        }

        #endregion

        [HttpGet]
        public PartialViewResult _Create(long Id)
        {

            return PartialView();
        }
        
        [HttpPost]
        public JsonResult _Create(CreateProjectSectionGovermentWorthyDocumentsPaidPriceViewModel cpsgwdpvm, Paging _Pg)
        {
            try
            {
                return Json(new {Values= new JavaScriptSerializer().Serialize(""),RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch(Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = _Pg._rowCount, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpGet]
        public PartialViewResult _List(long Id)
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult _List(ListProjectSectionGovermentWorthyDocumentsPaidPriceViewModel lpsgwdpvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = _Pg._rowCount, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        public PartialViewResult _Delete(long Id)
        {
            return PartialView();
        }

        [HttpPost]
        public PartialViewResult _Update(long Id)
        {
            return PartialView();
        }
    }
}