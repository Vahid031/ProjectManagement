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
using ViewModels.ProjectDevision.ProjectSectionStatementConfirmViewModel;
using ViewModels.ProjectDevision.ProjectSectionStatementViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionStatementConfirmController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionStatementService _ProjectSectionStatementService;
        IProjectSectionStatementConfirmService _ProjectSectionStatementConfirmService;
        IProjectSectionStatementConfirmFileService _ProjectSectionStatementConfirmFileService;
        IFileService _fileService;
        IPermissionService _permissionService;
        IOpinionService _opinionService;

        public ProjectSectionStatementConfirmController(
            IUnitOfWork uow, 
            IProjectSectionStatementService projectSectionStatementService, 
            IProjectSectionStatementConfirmService ProjectSectionStatementConfirmService,
            IProjectSectionStatementConfirmFileService ProjectSectionStatementConfirmFileService,
            IFileService fileService,
            IPermissionService permissionService,
            IOpinionService opinionService
            )
        {
            _uow = uow;
            _ProjectSectionStatementConfirmService = ProjectSectionStatementConfirmService;
            _ProjectSectionStatementConfirmFileService = ProjectSectionStatementConfirmFileService;
            _ProjectSectionStatementService = projectSectionStatementService;
            _fileService = fileService;
            _permissionService = permissionService;
            _opinionService = opinionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionStatementConfirm/_Index", Member.RoleId);
            var test = ViewBag.Permission;
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
        public JsonResult _List(ListProjectSectionStatementConfirmViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionStatementConfirmService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            CreateProjectSectionStatementConfirmViewModel cpsivm = new CreateProjectSectionStatementConfirmViewModel();
            cpsivm.ProjectSectionStatementConfirm = new DomainModels.Entities.ProjectDevision.ProjectSectionStatementConfirm();

            cpsivm.Opinions = new List<Opinion>() { new Opinion() { Title = "انتخاب کنید..." } };
            cpsivm.Opinions.AddRange(_opinionService.Get().ToList());
            cpsivm.ProjectSectionStatementConfirm.Date = ConvertDate.GetShamsi(DateTime.Now);

            ViewBag.Id = "-1";
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionStatementConfirmViewModel cpsivm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                if (cpsivm.ProjectSectionStatementConfirm.Description == null)
                    cpsivm.ProjectSectionStatementConfirm.Description = "";

                long? isInsert = cpsivm.ProjectSectionStatementConfirm.Id;

                if (isInsert == -1)
                    _ProjectSectionStatementConfirmService.Insert(cpsivm.ProjectSectionStatementConfirm);
                else
                    _ProjectSectionStatementConfirmService.Update(cpsivm.ProjectSectionStatementConfirm);


                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
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

        [AllowAnonymous]
        public JsonResult SaveFiles(CreateProjectSectionStatementConfirmViewModel cpsivm)
        {
            try
            {
                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _ProjectSectionStatementConfirmService.Save(cpsivm, Member.Id, paths);

                if (result.Key > 0)
                {
                    foreach (string item in paths)
                    {
                        CheckUploadFile.DeleteFile(Server.MapPath(item));
                    }

                    return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessInsert) });
                }
                else
                {
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorInsert) });
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
                CreateProjectSectionStatementConfirmViewModel cpsivm = new CreateProjectSectionStatementConfirmViewModel();
                cpsivm.ProjectSectionStatementConfirm = _ProjectSectionStatementConfirmService.Find(id);

                cpsivm.Opinions = new List<Opinion>() { new Opinion() { Title = "انتخاب کنید..." } };
                cpsivm.Opinions.AddRange(_opinionService.Get().ToList());

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionStatementConfirmViewModel());
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

                //foreach (var item in _ProjectSectionStatementConfirmFileService.Get(i => i.ProjectSectionStatementConfirmId == id).ToList())
                //{
                //    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                //    _fileService.Delete(item.FileId);
                //    _ProjectSectionStatementConfirmFileService.Delete(item);
                //}
                
                _ProjectSectionStatementConfirmService.Delete(id);

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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionStatementConfirm"), "ProjectSectionStatementConfirm", "/ProjectDevision/ProjectSectionStatementConfirm/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionStatementConfirmFiles));

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
        public JsonResult GetProjectSectionStatementConfirmFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionStatementConfirmFileService.GetByProjectSectionStatementConfirmId(Member.Id, projectSectionId)) });
        }


        #endregion
    }
}