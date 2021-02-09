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
using ViewModels.ProjectDevision.ProjectSectionFixedViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionFixedController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;
        IStateService _stateService;
        ICityService _cityService;
        IPermissionService _permissionService;

        public ProjectSectionFixedController(IUnitOfWork uow, IProjectSectionService projectSectionService, IStateService stateService, ICityService cityService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
            _stateService = stateService;
            _cityService = cityService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionFixed/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Search

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _Search()
        {
            SearchProjectSectionFixedViewModel spsavm = new SearchProjectSectionFixedViewModel();
            spsavm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            spsavm.States.AddRange(_stateService.Get().ToList());

            spsavm.Cities = new List<City>();

            ViewBag.Id = "-1";
            return PartialView(spsavm);
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
        public JsonResult _List(ListProjectSectionFixedViewModel lpsavm, Paging _Pg)
        {
            try
            {
                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                Int64 MemberId = 0;
                if (Member.RoleId == 3 || Member.RoleId == 4)
                    MemberId = Member.Id;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionService.GetProjectFixedDelivery(lpsavm, ref _Pg, MemberId)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion
    }
}