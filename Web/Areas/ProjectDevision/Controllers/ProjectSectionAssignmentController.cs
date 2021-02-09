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
using ViewModels.ProjectDevision.ProjectSectionAssignmentViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionAssignmentController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;
        IStateService _stateService;
        ICityService _cityService;
        IProjectSectionAssignmentStateService _projectSectionAssignmentStateService;
        IPermissionService _permissionService;

        public ProjectSectionAssignmentController(IUnitOfWork uow, IProjectSectionService projectSectionService, IStateService stateService, ICityService cityService, IProjectSectionAssignmentStateService projectSectionAssignmentStateService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
            _stateService = stateService;
            _cityService = cityService;
            _projectSectionAssignmentStateService = projectSectionAssignmentStateService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionAssignment/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Search

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _Search()
        {
            SearchProjectSectionAssignmentViewModel spsavm = new SearchProjectSectionAssignmentViewModel();
            spsavm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            spsavm.States.AddRange(_stateService.Get().ToList());

            spsavm.Cities = new List<City>();

            spsavm.ProjectSectionAssignmentStates = new List<ProjectSectionAssignmentState>() { new ProjectSectionAssignmentState() { Title = "انتخاب کنید..." } };
            spsavm.ProjectSectionAssignmentStates.AddRange(_projectSectionAssignmentStateService.Get().ToList());

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
        public JsonResult _List(ListProjectSectionAssignmentViewModel lpsavm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionService.GetProjectAssignment(lpsavm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion
    }
}