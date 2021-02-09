using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDefine.ProjectViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDefine.Controllers
{
    public class ProjectController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectService _projectService;
        IProgramPlanService _programPlanService;
        IProgramService _programService;
        IProjectTypeService _projectTypeService;
        IExecutionTypeService _executionTypeService;
        IStateService _stateService;
        ICityService _cityService;
        ISectionService _sectionService;
        IVillageService _villageService;
        IRuralDistrictService _ruralDistrictService;
        IFinantialYearService _finantialYearService;
        IOwnershipTypeService _ownershipTypeService;
        IUnitService _unitService;
        IProjectStateService _projectStateService;
        ICreditProvidePlaceService _creditProvidePlaceService;
        IProjectFileService _projectFileService;
        IProjectPlanService _projectPlanService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectController(IUnitOfWork uow, IProjectService projectService, IProgramPlanService programPlanService, IProgramService programService, IProjectTypeService projectTypeService, IExecutionTypeService executionTypeService, IStateService stateService, ICityService cityService, ISectionService sectionService, IVillageService villageService, IRuralDistrictService ruralDistrictService, IFinantialYearService finantialYearService, IOwnershipTypeService ownershipTypeService, IUnitService unitService, IProjectStateService projectStateService, ICreditProvidePlaceService creditProvidePlaceService, IProjectPlanService projectPlanService, IPermissionService permissionService, IFileService fileService, IProjectFileService projectFileService)
        {
            _uow = uow;

            _projectService = projectService;
            _programPlanService = programPlanService;
            _programService = programService;
            _projectTypeService = projectTypeService;
            _executionTypeService = executionTypeService;
            _stateService = stateService;
            _cityService = cityService;
            _sectionService = sectionService;
            _villageService = villageService;
            _ruralDistrictService = ruralDistrictService;
            _finantialYearService = finantialYearService;
            _ownershipTypeService = ownershipTypeService;
            _unitService = unitService;
            _projectStateService = projectStateService;
            _creditProvidePlaceService = creditProvidePlaceService;
            _projectPlanService = projectPlanService;
            _permissionService = permissionService;
            _fileService = fileService;
            _projectFileService = projectFileService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/Project/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult ProgramPlanTitleValidation([Bind(Prefix = "ProgramPlanTitle")]string value, [Bind(Prefix = "ProgramPlanId")]Int64? programPlanId)
        {
            if (value != null && value != "انتخاب کنید..." && (programPlanId == null || programPlanId == -1))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }
        
        [AllowAnonymous]
        public JsonResult FileCodeValidation([Bind(Prefix = "Project.FileCode")]string value, [Bind(Prefix = "Project.Id")]Int64 id)
        {
            var item = _projectService.Get(c => c.FileCode == value && c.Id != id).ToList();

            if (item.Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult _List(ListProjectViewModel lptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectService.GetAll(lptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }


        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateProjectViewModel cptvm = new CreateProjectViewModel();

            cptvm.CreditProvidePlaces = new List<CreditProvidePlace>() { new CreditProvidePlace() { Title = "انتخاب کنید..." } };
            cptvm.CreditProvidePlaces.AddRange(_creditProvidePlaceService.Get().ToList());

            cptvm.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            cptvm.ProjectTypes.AddRange(_projectTypeService.Get().ToList());

            cptvm.ExecutionTypes = new List<ExecutionType>() { new ExecutionType() { Title = "انتخاب کنید..." } };
            cptvm.ExecutionTypes.AddRange(_executionTypeService.Get().ToList());

            cptvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cptvm.States.AddRange(_stateService.Get().ToList());

            cptvm.Cities = new List<City>();
            cptvm.Sections = new List<Section>();
            cptvm.Villages = new List<Village>();
            cptvm.RuralDistrictes = new List<RuralDistrict>();

            cptvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            cptvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());

            cptvm.ForecastEndFinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            cptvm.ForecastEndFinantialYears.AddRange(_finantialYearService.Get().ToList());

            cptvm.OwnershipTypes = new List<OwnershipType>() { new OwnershipType() { Title = "انتخاب کنید..." } };
            cptvm.OwnershipTypes.AddRange(_ownershipTypeService.Get().ToList());

            cptvm.Units = new List<Unit>() { new Unit() { Title = "انتخاب کنید..." } };
            cptvm.Units.AddRange(_unitService.Get().ToList());

            cptvm.ProjectStates = new List<ProjectState>() { new ProjectState() { Title = "انتخاب کنید..." } };
            cptvm.ProjectStates.AddRange(_projectStateService.Get().ToList());

            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cptvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectViewModel catvm)
        {
            try
            {
                if (catvm.Project.Id == -1)
                    catvm.Project.ProjectStateId = 1;

                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.Project.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _projectService.Save(catvm, Member.Id, paths);

                if (result.Key > 0)
                {
                    foreach (string item in paths)
                    {
                        CheckUploadFile.DeleteFile(Server.MapPath(item));
                    }

                    if (isInsert == -1)
                        return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessInsert) });
                    else
                        return Json(new { type = AlarmType.info.ToString(), message = Messages.GetMsg(MsgKey.SuccessUpdate) });
                }
                else if (result.Key == 0)
                {
                    return Json(new { type = AlarmType.danger.ToString(), message = result.Value });
                }
                else
                {
                    if (isInsert == -1)
                        return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorInsert) });
                    else
                        return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorUpdate) });
                }
            }
            catch (Exception)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }

        [HttpPost]
        public PartialViewResult _Update(Int64 id)
        {
            try
            {
                CreateProjectViewModel cptvm = new CreateProjectViewModel();
                cptvm.Project = _projectService.Find(id);

                cptvm.RuralDistrictes = new List<RuralDistrict>() { new RuralDistrict() { Title = "انتخاب کنید..." } };
                cptvm.RuralDistrictes.AddRange(_ruralDistrictService.Get(i => i.VillageId == cptvm.Project.VillageId).ToList());

                cptvm.Villages = new List<Village>() { new Village() { Title = "انتخاب کنید..." } };
                cptvm.Villages.AddRange(_villageService.Get(i => i.SectionId == cptvm.Project.SectionId).ToList());

                cptvm.Sections = new List<Section>() { new Section() { Title = "انتخاب کنید..." } };
                cptvm.Sections.AddRange(_sectionService.Get(i => i.CityId == cptvm.Project.CityId).ToList());

                cptvm.Cities = new List<City>() { new City() { Title = "انتخاب کنید..." } };
                cptvm.Cities.AddRange(_cityService.Get(i => i.StateId == cptvm.Project.StateId).ToList());

                cptvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
                cptvm.States.AddRange(_stateService.Get().ToList());

                cptvm.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
                cptvm.ProjectTypes.AddRange(_projectTypeService.Get().ToList());

                cptvm.ExecutionTypes = new List<ExecutionType>() { new ExecutionType() { Title = "انتخاب کنید..." } };
                cptvm.ExecutionTypes.AddRange(_executionTypeService.Get().ToList());

                cptvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
                cptvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());

                cptvm.ForecastEndFinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
                cptvm.ForecastEndFinantialYears.AddRange(_finantialYearService.Get().ToList());

                cptvm.OwnershipTypes = new List<OwnershipType>() { new OwnershipType() { Title = "انتخاب کنید..." } };
                cptvm.OwnershipTypes.AddRange(_ownershipTypeService.Get().ToList());

                cptvm.Units = new List<Unit>() { new Unit() { Title = "انتخاب کنید..." } };
                cptvm.Units.AddRange(_unitService.Get().ToList());

                cptvm.ProjectStates = new List<ProjectState>() { new ProjectState() { Title = "انتخاب کنید..." } };
                cptvm.ProjectStates.AddRange(_projectStateService.Get().ToList());

                ReloadFiles();

                foreach (var item in _projectFileService.Get(i => i.ProjectId == id).ToList())
                {
                    cptvm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cptvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectViewModel());
            }

        }

        #endregion


        #region Delete

        [HttpPost]
        public JsonResult _Delete(Int64 id)
        {
            try
            {
                if (!ModelState.IsValidField("Id") || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });

                List<string> paths = new List<string>();

                foreach (var item in _projectFileService.Get(i => i.ProjectId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _projectFileService.Delete(item);
                }

                foreach (var item in _projectPlanService.Get(i => i.ProjectId == id))
                {
                    _projectPlanService.Delete(item);
                }

                _projectService.Delete(id);

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
                    foreach (string item in paths)
                    {
                        Utilities.AppCode.CheckUploadFile.DeleteFile(Server.MapPath(item));    
                    }
                    
                    return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessDelete) });
                }
                else
                {
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorDelete) });
                }
            }
            catch (Exception)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }

        #endregion


        #region Map

        [AllowAnonymous]
        public PartialViewResult _Map()
        {

            return PartialView();
        }

        #endregion


        #region FileUpload

        [AllowAnonymous]
        public void ReloadFiles()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            _fileService.DeleteFiles(Member.Id);
        }

        [AllowAnonymous]
        public PartialViewResult _FileUpload()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            List<UploadFilesResultViewModel> statuses = new List<UploadFilesResultViewModel>();
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectDefines"), "ProjectDefines", "/ProjectDefine/Project/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectFiles));

            if (result.Key == 3)
            {
                JsonResult r = Json(new { files = statuses });
                r.ContentType = "text/plain";

                return r;
            }

            return Json(new { files = "{'name': 'تصویر'}, {'size': 0}, {'error': '" + result.Value + "'}"});
        }

        [AllowAnonymous]
        public JsonResult DeleteFile(Int64 Id)
        {
            var file = _fileService.Get(i => i.Id == Id).FirstOrDefault();
            if (file.FileState == 1)
            {
                Utilities.AppCode.CheckUploadFile.DeleteFile(Server.MapPath(file.Address));
                _fileService.Delete(file);
            }
            else
            {
                file.FileState = 3;
                _fileService.Update(file);
            }
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            _uow.SaveChanges(Member.Id);

            return Json(new { files = new JavaScriptSerializer().Serialize("{'" + file.Title + "': true}") });
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetProjectFiles(Int64 projectId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectFileService.GetByProjectId(Member.Id, projectId)) });
        }


        #endregion


        #region ProjectPlan

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _Plan()
        {
            CreateProjectViewModel cptvm = new CreateProjectViewModel();

            cptvm.Programs = new List<Program>() { new Program() { Title = "انتخاب کنید..." } };
            cptvm.Programs.AddRange(_programService.Get().ToList());

            cptvm.CreditProvidePlaces = new List<CreditProvidePlace>() { new CreditProvidePlace() { Title = "انتخاب کنید..." } };
            cptvm.CreditProvidePlaces.AddRange(_creditProvidePlaceService.Get().ToList());

            cptvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            cptvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());

            return PartialView(cptvm);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetProjectPlanByProjectId(Int64 projectId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectPlanService.Get(i => i.ProjectId == projectId).Select(i => new { i.Id, i.ProjectId, i.ProgramId, ProgramTitle = i.Program.Title, i.ProgramPlanId, ProgramPlanTitle = i.ProgramPlan.PlanTitle, CreditProvidePlaceId = i.CreditProvidePlaceId.HasValue ? i.CreditProvidePlaceId.ToString() : "", CreditProvidePlaceTitle = i.CreditProvidePlace.Title, FromFinantialYearId = i.FromFinantialYearId.HasValue ? i.FromFinantialYearId.ToString() : "", FromFinantialYearTitle = i.FinantialYearFromFinantialYearId.Title, ToFinantialYearId = i.ToFinantialYearId.HasValue ? i.ToFinantialYearId.ToString() : "", ToFinantialYearTitle = i.FinantialYearToFinantialYearId.Title }).ToList()) });
        }

        #endregion
    }
}