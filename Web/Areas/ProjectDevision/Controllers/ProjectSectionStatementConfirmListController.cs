using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDevision.ProjectSectionStatementViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionStatementConfirmListController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionStatementService _ProjectSectionStatementService;
        IPermissionService _permissionService;

        public ProjectSectionStatementConfirmListController(
            IUnitOfWork uow, 
            IProjectSectionStatementService projectSectionStatementService, 
            IPermissionService permissionService
            )
        {
            _uow = uow;
            _ProjectSectionStatementService = projectSectionStatementService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionStatementConfirm/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region List


        [HttpGet]
        public PartialViewResult _List()
        {
            return PartialView();
        }



        [HttpPost]
        [ValidateInput(false)]
        public JsonResult _List(ListProjectSectionStatementViewModel lpptvm, Paging _Pg)
        {
            try
            {
                var Values = new JavaScriptSerializer().Serialize(_ProjectSectionStatementService.GetAll(lpptvm, ref _Pg));
                var test = _ProjectSectionStatementService.GetAll(lpptvm, ref _Pg);
                return Json(new {value = Values, RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }


   
        #endregion

    }
}