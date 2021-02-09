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
using ViewModels.ProjectDevision.ProjectSectionStatementViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionStatementController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IOpinionService _opinionService;
        ITempFixedService _tempFixedService;
        IStatementTypeService _statementTypeService;
        IStatementNumberService _statementNumberService;
        IProjectSectionService _projectSectionService;
        IProjectSectionStatementService _ProjectSectionStatementService;
        IProjectSectionStatementFileService _ProjectSectionStatementFileService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionStatementController(IUnitOfWork uow, IStatementNumberService statementNumberService, IStatementTypeService statementTypeService, ITempFixedService tempFixedService, IOpinionService opinionService, IDefectService defectService, IProjectSectionService projectSectionService, IProjectSectionStatementService ProjectSectionStatementService, IProjectSectionStatementFileService ProjectSectionStatementFileService, IFileService fileService, IPermissionService permissionService)
        {
            _uow = uow;

            _statementTypeService = statementTypeService;
            _statementNumberService = statementNumberService;
            _opinionService = opinionService;
            _tempFixedService = tempFixedService;
            _projectSectionService = projectSectionService;
            _ProjectSectionStatementService = ProjectSectionStatementService;
            _ProjectSectionStatementFileService = ProjectSectionStatementFileService;
            _fileService = fileService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionStatement/_Index", Member.RoleId);
            var test = ViewBag.Permission; 
            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult CodeValidation([Bind(Prefix = "ProjectSectionStatement.StatementNumberId")]long value, [Bind(Prefix = "ProjectSectionStatement.Id")]long id, [Bind(Prefix = "ProjectSectionStatement.ProjectSectionId")]long ProjectSectionId, [Bind(Prefix = "ProjectSectionStatement.StatementTypeId")]long StatementTypeId)
        {
            var item = _ProjectSectionStatementService.Get(c => c.StatementNumberId == value && c.ProjectSectionId == ProjectSectionId && c.StatementTypeId == StatementTypeId && c.Id != id).ToList();

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
        public JsonResult _List(ListProjectSectionStatementViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionStatementService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetStatementNumbersByTempFixedId(Int64 tempFixedId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_statementNumberService.Get(i => i.TempFixedId == tempFixedId).Select(i => new { i.Id, i.Title }).ToList()) });
        }


        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateProjectSectionStatementViewModel cpsivm = new CreateProjectSectionStatementViewModel();

            cpsivm.Opinions = new List<Opinion>() { new Opinion() { Title = "انتخاب کنید..." } };
            cpsivm.Opinions.AddRange(_opinionService.Get().ToList());

            cpsivm.TempFixeds = new List<TempFixed>() { new TempFixed() { Title = "انتخاب کنید..." } };
            cpsivm.TempFixeds.AddRange(_tempFixedService.Get().ToList());

            cpsivm.StatementTypes = new List<StatementType>() { new StatementType() { Title = "انتخاب کنید..." } };
            cpsivm.StatementTypes.AddRange(_statementTypeService.Get().ToList());

            cpsivm.StatementNumbers = new List<StatementNumber>();

            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionStatementViewModel cpsivm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = cpsivm.ProjectSectionStatement.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _ProjectSectionStatementService.Save(cpsivm, Member.Id, paths);

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
                    if (result.Key == -1)
                        return Json(new { type = AlarmType.danger.ToString(), message = result.Value });

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
                CreateProjectSectionStatementViewModel cpsivm = new CreateProjectSectionStatementViewModel();
                cpsivm.ProjectSectionStatement = _ProjectSectionStatementService.Find(id);

                cpsivm.Opinions = new List<Opinion>() { new Opinion() { Title = "انتخاب کنید..." } };
                cpsivm.Opinions.AddRange(_opinionService.Get().ToList());

                cpsivm.TempFixeds = new List<TempFixed>() { new TempFixed() { Title = "انتخاب کنید..." } };
                cpsivm.TempFixeds.AddRange(_tempFixedService.Get().ToList());
                cpsivm.TempFixedId = _statementNumberService.Get(i => i.Id == cpsivm.ProjectSectionStatement.StatementNumberId).FirstOrDefault().TempFixedId.Value;

                cpsivm.StatementTypes = new List<StatementType>() { new StatementType() { Title = "انتخاب کنید..." } };
                cpsivm.StatementTypes.AddRange(_statementTypeService.Get().ToList());

                cpsivm.StatementNumbers = new List<StatementNumber>() { new StatementNumber() { Title = "انتخاب کنید..." } };
                cpsivm.StatementNumbers.AddRange(_statementNumberService.Get(i => i.TempFixedId == cpsivm.TempFixedId).ToList());

                ReloadFiles();

                foreach (var item in _ProjectSectionStatementFileService.Get(i => i.ProjectSectionStatementId == id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionStatementViewModel());
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

                foreach (var item in _ProjectSectionStatementFileService.Get(i => i.ProjectSectionStatementId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _ProjectSectionStatementFileService.Delete(item);
                }
                
                _ProjectSectionStatementService.Delete(id);

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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionStatement"), "ProjectSectionStatement", "/ProjectDevision/ProjectSectionStatement/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionStatementFiles));

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
        public JsonResult GetProjectSectionStatementFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionStatementFileService.GetByProjectSectionStatementId(Member.Id, projectSectionId)) });
        }


        #endregion
    }
}