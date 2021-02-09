using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
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
using ViewModels.ProjectDevision.ProjectSectionDraftViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionDraftListController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionStatementConfirmService _projectSectionStatementConfirmService;
        IPermissionService _permissionService;

        public ProjectSectionDraftListController(IUnitOfWork uow, 
            IProjectSectionStatementConfirmService projectSectionStatementConfirmService,
            IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionStatementConfirmService = projectSectionStatementConfirmService; 
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index(int id)
        {
            //FormType: 1= صدور فرم حواله , FormType: 2 = تایید فرم حواله
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionDraft/_Index", Member.RoleId);
            ViewBag.FormType = id;

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
        public JsonResult _List(ListProjectSectionDraftListViewModel lpsavm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionStatementConfirmService.GetConfirmDraft(lpsavm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion
    }
}