using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDefine.ProjectFundingViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDefine.Controllers
{
    public class ProjectFundingController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectFundingService _projectFundingService;
        IProjectFundingFileService _projectFundingFileService;
        IProjectService _projectService;
        IFinantialYearService _finantialYearService;
        IResourceService _resourceService;
        IResourceTypeService _resourceTypeService;
        IProjectPlanService _projectPlanService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectFundingController(IUnitOfWork uow, 
            IProjectFundingService projectFundingService,
            IProjectFundingFileService projectFundingFileService,
            IFinantialYearService finantialYearService, 
            IResourceService resourceService, 
            IResourceTypeService resourceTypeService, 
            IPermissionService permissionService,
            IProjectPlanService projectPlanService,
            IProjectService projectService,
            IFileService fileService
            )
        {
            _uow = uow;

            _projectFundingService = projectFundingService;
            _projectFundingFileService = projectFundingFileService;
            _finantialYearService = finantialYearService;
            _resourceService = resourceService;
            _resourceTypeService = resourceTypeService;
            _projectService = projectService;
            _projectPlanService = projectPlanService;
            _permissionService = permissionService;
            _fileService = fileService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/ProjectFunding/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectFundingViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectFundingService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create(Int64 projectId)
        {
            CreateProjectFundingViewModel cpftvm = new CreateProjectFundingViewModel();
            cpftvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            cpftvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());

            cpftvm.Resources = new List<Resource>() { new Resource() { Title = "انتخاب کنید..." } };
            cpftvm.Resources.AddRange(_resourceService.Get().ToList());

            cpftvm.ProjectPlans = new List<BaseDropdownViewModel>() { new BaseDropdownViewModel() { Title = "انتخاب کنید..." } };
            cpftvm.ProjectPlans.AddRange(_projectPlanService.Get(i => i.ProjectId == projectId).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.CreditProvidePlace.Title + " / " + i.ProgramPlan.PlanTitle }).ToList());

            cpftvm.ResourceTypes = new List<ResourceType>();

            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cpftvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectFundingViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectFunding.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _projectFundingService.Save(catvm, Member.Id, paths);

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
                CreateProjectFundingViewModel cpftvm = new CreateProjectFundingViewModel();
                cpftvm.ProjectFunding = _projectFundingService.Find(id);

                cpftvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
                cpftvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());

                cpftvm.ResourceId = _resourceTypeService.Get(i => i.Id == cpftvm.ProjectFunding.ResourceTypeId).First().ResourceId.Value;

                cpftvm.Resources = new List<Resource>() { new Resource() { Title = "انتخاب کنید..." } };
                cpftvm.Resources.AddRange(_resourceService.Get().ToList());

                cpftvm.ResourceTypes = new List<ResourceType>() { new ResourceType() { Title = "انتخاب کنید..." } };
                cpftvm.ResourceTypes.AddRange(_resourceTypeService.Get(i => i.ResourceId == cpftvm.ResourceId));


                cpftvm.ProjectPlans = new List<BaseDropdownViewModel>() { new BaseDropdownViewModel() { Title = "انتخاب کنید..." } };
                cpftvm.ProjectPlans.AddRange(_projectPlanService.Get(i => i.ProjectId == cpftvm.ProjectFunding.ProjectId).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.CreditProvidePlace.Title + " / " + i.ProgramPlan.PlanTitle }).ToList());

                ReloadFiles();

                foreach (var item in _projectFundingFileService.Get(i => i.ProjectFundingId == id).ToList())
                {
                    cpftvm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpftvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectFundingViewModel());
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

                foreach (var item in _projectFundingFileService.Get(i => i.ProjectFundingId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _projectFundingFileService.Delete(item);
                }

                _projectFundingService.Delete(id);

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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectFunding"), "ProjectFunding", "/ProjectDefine/ProjectFunding/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectFundingFiles));

            if (result.Key == 3)
            {
                JsonResult r = Json(new { files = statuses });
                r.ContentType = "text/plain";

                return r;
            }

            return Json(new { files = "{'name': 'تصویر'}, {'size': 0}, {'error': '" + result.Value + "'}" });
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
        public JsonResult GetProjectFundingFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectFundingFileService.GetByProjectFundingId(Member.Id, projectSectionId)) });
        }


        #endregion
    }
}