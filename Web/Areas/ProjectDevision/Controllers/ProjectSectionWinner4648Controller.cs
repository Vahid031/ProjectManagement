using DatabaseContext.Context;
using Infrastracture.AppCode;
using Services.Interfaces.ProjectDevision;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.ProjectDevision.ProjectSectionWinnerViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;
using DomainModels.Enums;
using Services.Interfaces.BaseInformation;
using DomainModels.Entities.BaseInformation;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionWinner4648Controller : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionWinnerService _projectSectionWinnerService;
        IPermissionService _permissionService;

        public ProjectSectionWinner4648Controller(IUnitOfWork uow, 
            IProjectSectionWinnerService projectSectionWinnerService,
            IPermissionService permissionService)
        {
            _uow = uow;
            _projectSectionWinnerService = projectSectionWinnerService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionWinner/_Index" , Member.RoleId);

            return PartialView();
        }

        #endregion


        #region List

        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult _List()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        [AllowAnonymous]
        public JsonResult _List(long contractorId, ListProjectSectionWinnerViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionWinnerService.Get4648(contractorId, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion



	}
}