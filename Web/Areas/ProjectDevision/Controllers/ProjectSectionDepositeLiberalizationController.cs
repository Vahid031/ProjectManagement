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
using ViewModels.ProjectDevision.ProjectSectionDepositeLiberalizationViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionDepositeLiberalizationController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;
        IProjectSectionDepositeLiberalizationService _ProjectSectionDepositeLiberalizationService;
        IProjectSectionDepositeLiberalizationFileService _ProjectSectionDepositeLiberalizationFileService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionDepositeLiberalizationController(IUnitOfWork uow, IProjectSectionService projectSectionService, IProjectSectionDepositeLiberalizationService ProjectSectionDepositeLiberalizationService, IProjectSectionDepositeLiberalizationFileService ProjectSectionDepositeLiberalizationFileService, IFileService fileService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
            _ProjectSectionDepositeLiberalizationService = ProjectSectionDepositeLiberalizationService;
            _ProjectSectionDepositeLiberalizationFileService = ProjectSectionDepositeLiberalizationFileService;
            _fileService = fileService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionLiberalization/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectSectionDepositeLiberalizationViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionDepositeLiberalizationService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            CreateProjectSectionDepositeLiberalizationViewModel cpsivm = new CreateProjectSectionDepositeLiberalizationViewModel();

            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionDepositeLiberalizationViewModel cpsivm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = cpsivm.ProjectSectionDepositeLiberalization.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _ProjectSectionDepositeLiberalizationService.Save(cpsivm, Member.Id, paths);

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
                CreateProjectSectionDepositeLiberalizationViewModel cpsivm = new CreateProjectSectionDepositeLiberalizationViewModel();
                cpsivm.ProjectSectionDepositeLiberalization = _ProjectSectionDepositeLiberalizationService.Find(id);

                ReloadFiles();

                foreach (var item in _ProjectSectionDepositeLiberalizationFileService.Get(i => i.ProjectSectionDepositeLiberalizationId == id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionDepositeLiberalizationViewModel());
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

                foreach (var item in _ProjectSectionDepositeLiberalizationFileService.Get(i => i.ProjectSectionDepositeLiberalizationId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _ProjectSectionDepositeLiberalizationFileService.Delete(item);
                }
                
                _ProjectSectionDepositeLiberalizationService.Delete(id);

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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionDepositeLiberalization"), "ProjectSectionDepositeLiberalization", "/ProjectDevision/ProjectSectionDepositeLiberalization/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionDepositeLiberalizationFiles));

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
        public JsonResult GetProjectSectionDepositeLiberalizationFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionDepositeLiberalizationFileService.GetByProjectSectionDepositeLiberalizationId(Member.Id, projectSectionId)) });
        }


        #endregion
    }
}