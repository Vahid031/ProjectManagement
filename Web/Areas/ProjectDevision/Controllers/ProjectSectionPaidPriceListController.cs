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
using ViewModels.ProjectDevision.ProjectSectionDraftViewModel;
using ViewModels.ProjectDevision.ProjectSectionPaidPriceViewModel;
using ViewModels.ProjectDevision.ProjectSectionStatementViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionPaidPriceListController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionDraftService _projectSectionDraftService;
        IPermissionService _permissionService;
        IProjectSectionPaidPriceService _projectSectionPaidPriceService;

        public ProjectSectionPaidPriceListController(
            IUnitOfWork uow, 
            IProjectSectionStatementService projectSectionStatementService, 
            IPermissionService permissionService,
            IProjectSectionDraftService projectSectionDraftService,
            IProjectSectionPaidPriceService projectSectionPaidPriceService
            )
        {
            _uow = uow;
            _permissionService = permissionService;
            _projectSectionDraftService = projectSectionDraftService;
            _projectSectionPaidPriceService = projectSectionPaidPriceService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionPaidPriceList/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectSectionDraftViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionDraftService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }


        #endregion



        [HttpGet]
        [AllowAnonymous]
        public PartialViewResult _Create(long Id)
        {
            CalculateProjectSectionPaidPriceViewModel cpspvm = _projectSectionPaidPriceService.CalculatePadePriceList(Id).FirstOrDefault();

            return PartialView(cpspvm);
        }


    }
}