using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDefine.ProjectAgreementViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDefine.Controllers
{
    public class ProjectProgrammingController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectService _projectService;
        IStateService _stateService;
        ICityService _cityService;
        IProjectTypeService _projectTypeService;
        IProjectStateService _projectStateService;
        IPermissionService _permissionService;

        public ProjectProgrammingController(IUnitOfWork uow, IProjectService projectService, IStateService stateService, ICityService cityService, IProjectTypeService projectTypeService, IProjectStateService projectStateService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectService = projectService;
            _stateService = stateService;
            _cityService = cityService;
            _projectTypeService = projectTypeService;
            _projectStateService = projectStateService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/ProjectProgramming/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Serach

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _Search()
        {
            SearchProjectAgreementViewModel spavm = new SearchProjectAgreementViewModel();
            spavm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            spavm.States.AddRange(_stateService.Get().ToList());

            spavm.Cities = new List<City>();

            spavm.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            spavm.ProjectTypes.AddRange(_projectTypeService.Get().ToList());

            spavm.ProjectStates = new List<ProjectState>() { new ProjectState() { Title = "انتخاب کنید..." } };
            spavm.ProjectStates.AddRange(_projectStateService.Get().ToList());

            ViewBag.Id = "-1";
            return PartialView(spavm);
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
        public JsonResult _List(ListProjectAgreementViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectService.GetProjectAggrement(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion
    }
}