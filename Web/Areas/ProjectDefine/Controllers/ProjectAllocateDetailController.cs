using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDefine;
using Services.Interfaces.ProjectDevision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDefine.ProjectAllocateViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDefine.Controllers
{
    public class ProjectAllocateDetailController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectAllocateService _projectAllocateService;
        IProjectAllocateFileService _projectAllocateFileService;
        IProjectService _projectService;
        IProjectPlanService _projectPlanService;
        IProjectSectionService _projectSectionService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectAllocateDetailController(
            IUnitOfWork uow, 
            IProjectAllocateService projectAllocateService, 
            IProjectAllocateFileService projectAllocateFileService,
            IProjectService projectService,
            IProjectPlanService projectPlanService,
            IProjectSectionService projectSectionService,
            IFileService fileService,
            IPermissionService permissionService
            )
        {
            _uow = uow;

            _projectAllocateService = projectAllocateService;
            _projectAllocateFileService = projectAllocateFileService;
            _projectService = projectService;
            _projectPlanService = projectPlanService;
            _projectSectionService = projectSectionService;
            _fileService = fileService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/ProjectAllocate/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectAllocateViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectAllocateService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetProjectPlanByProjectId(Int64 projectId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectPlanService.Get(i => i.ProjectId == projectId).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.CreditProvidePlace.Title + " / " + i.ProgramPlan.PlanTitle }).ToList()) });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateProjectAllocateViewModel cpavm = new CreateProjectAllocateViewModel();

            cpavm.ProjectPlans = new List<BaseDropdownViewModel>() { new BaseDropdownViewModel() { Title = "انتخاب کنید..." } };
            cpavm.ProjectSections = new List<ProjectSection>() { new ProjectSection() { Title = "انتخاب کنید..." } };
            
            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cpavm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectAllocateViewModel cpatvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = cpatvm.ProjectAllocate.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _projectAllocateService.Save(cpatvm, Member.Id, paths);

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
                CreateProjectAllocateViewModel cpavm = new CreateProjectAllocateViewModel();
                cpavm.ProjectAllocate = _projectAllocateService.Find(id);

                cpavm.ProjectPlans = new List<BaseDropdownViewModel>() { new BaseDropdownViewModel() { Title = "انتخاب کنید..." } };
                cpavm.ProjectPlans.AddRange(_projectPlanService.Get(i => i.ProjectId == cpavm.ProjectAllocate.ProjectId).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.CreditProvidePlace.Title + " / " + i.ProgramPlan.PlanTitle }).ToList());

                cpavm.ProjectSections = new List<ProjectSection>() { new ProjectSection() { Title = "انتخاب کنید..." } };
                cpavm.ProjectSections.AddRange(_projectSectionService.Get(i => i.ProjectId == cpavm.ProjectAllocate.ProjectId).ToList());

                ReloadFiles();

                foreach (var item in _projectAllocateFileService.Get(i => i.ProjectAllocateId == id).ToList())
                {
                    cpavm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpavm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectAllocateViewModel());
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

                foreach (var item in _projectAllocateFileService.Get(i => i.ProjectAllocateId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _projectAllocateFileService.Delete(item);
                }

                _projectAllocateService.Delete(id);

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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectAllocate"), "ProjectAllocate", "/ProjectDefine/ProjectAllocateDetail/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectAllocateFiles));

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
        public JsonResult GetProjectAllocateFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectAllocateFileService.GetByProjectAllocateId(Member.Id, projectSectionId)) });
        }


        #endregion
    }
}