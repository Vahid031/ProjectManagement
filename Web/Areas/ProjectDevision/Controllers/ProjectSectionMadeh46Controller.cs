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
using ViewModels.ProjectDevision.ProjectSectionMadeh46ViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionMadeh46Controller : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;
        IProjectSectionMadeh46Service _ProjectSectionMadeh46Service;
        IProjectSectionMadeh46FileService _ProjectSectionMadeh46FileService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionMadeh46Controller(IUnitOfWork uow, IProjectSectionService projectSectionService, IProjectSectionMadeh46Service ProjectSectionMadeh46Service, IProjectSectionMadeh46FileService ProjectSectionMadeh46FileService, IFileService fileService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
            _ProjectSectionMadeh46Service = ProjectSectionMadeh46Service;
            _ProjectSectionMadeh46FileService = ProjectSectionMadeh46FileService;
            _fileService = fileService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSection46/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create(Int64 Id)
        {
            CreateProjectSectionMadeh46ViewModel cpsivm = new CreateProjectSectionMadeh46ViewModel();

            var result = _ProjectSectionMadeh46Service.Get(i => i.ProjectSectionId == Id).FirstOrDefault();

            if (result != null)
            {
                cpsivm.ProjectSectionMadeh46 = result;

                foreach (var item in _ProjectSectionMadeh46FileService.Get(i => i.ProjectSectionMadeh46Id == result.Id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                ViewBag.Id = result.Id;
            }
            else
            {
                ViewBag.Id = "-1";
            }

            ReloadFiles();
            
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionMadeh46ViewModel cpsivm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = cpsivm.ProjectSectionMadeh46.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _ProjectSectionMadeh46Service.Save(cpsivm, Member.Id, paths);

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
                CreateProjectSectionMadeh46ViewModel cpsivm = new CreateProjectSectionMadeh46ViewModel();
                cpsivm.ProjectSectionMadeh46 = _ProjectSectionMadeh46Service.Find(id);

                ReloadFiles();

                foreach (var item in _ProjectSectionMadeh46FileService.Get(i => i.ProjectSectionMadeh46Id == id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionMadeh46ViewModel());
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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionMadeh46"), "ProjectSectionMadeh46", "/ProjectDevision/ProjectSectionMadeh46/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionMadeh46Files));

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
        public JsonResult GetProjectSectionMadeh46Files(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionMadeh46FileService.GetByProjectSectionMadeh46Id(Member.Id, projectSectionId)) });
        }


        #endregion
    }
}