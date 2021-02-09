using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;
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
using ViewModels.ProjectDevision.ProjectSectionDraftViewModel;
using Web.Utilities.AppCode;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using Stimulsoft.Base;
using Stimulsoft.Report.Export;
using System.Text;
using System.IO;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionDraftController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IOpinionService _opinionService;
        ITempFixedService _tempFixedService;
        IStatementTypeService _statementTypeService;
        IStatementNumberService _statementNumberService;
        IProjectSectionService _projectSectionService;
        IProjectSectionDraftService _ProjectSectionDraftService;
        IProjectSectionDraftFileService _ProjectSectionDraftFileService;
        IProjectSectionStatementConfirmService _projectSectionStatementConfirmService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionDraftController(IUnitOfWork uow, IStatementNumberService statementNumberService, IStatementTypeService statementTypeService, ITempFixedService tempFixedService, IOpinionService opinionService, IDefectService defectService, IProjectSectionService projectSectionService, IProjectSectionDraftService ProjectSectionDraftService, IProjectSectionDraftFileService ProjectSectionDraftFileService, IFileService fileService, IPermissionService permissionService
            , IProjectSectionStatementConfirmService projectSectionStatementConfirmService)
        {
            _uow = uow;

            _statementTypeService = statementTypeService;
            _statementNumberService = statementNumberService;
            _opinionService = opinionService;
            _tempFixedService = tempFixedService;
            _projectSectionService = projectSectionService;
            _ProjectSectionDraftService = ProjectSectionDraftService;
            _ProjectSectionDraftFileService = ProjectSectionDraftFileService;
            _fileService = fileService;
            _permissionService = permissionService;
            _projectSectionStatementConfirmService = projectSectionStatementConfirmService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionDraft/_Index", Member.RoleId);
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
        public JsonResult _List(ListProjectSectionDraftViewModel lpptvm, Paging _Pg)
        {
            try
            {
                var test = _ProjectSectionDraftService.GetAll(lpptvm, ref _Pg).ToList();

                return Json(new { Values = new JavaScriptSerializer().Serialize(test), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
        public PartialViewResult _Create(long Id)
        {
            CreateProjectSectionDraftViewModel cpsivm = new CreateProjectSectionDraftViewModel();
            var item = _projectSectionStatementConfirmService.Get(c => c.Id == Id).FirstOrDefault();
            cpsivm.ProjectSectionDraft = new ProjectSectionDraft { DraftPrice = item.Price.Value };
            cpsivm.ProjectSectionDraft.DraftDate = DateTime.Now;
            cpsivm.ProjectSectionDraft.DraftDate_ = Infrastracture.AppCode.ConvertDate.GetShamsi(DateTime.Now);

            // ConfirmPrice - Sum (DraftPrice)

            Int64 projectSectionId = item.ProjectSectionId.Value;

            // به دست آوردن آخرین صورت وضعیت تایید شده
            //var lastProjectSectionConfirmId = _projectSectionStatementConfirmService.Get(i => i.ProjectSectionId == projectSectionId).Max(i => i.Id);

            // به دست آوردن جمع کل حواله های تایید شده
            var otherDraft = _ProjectSectionDraftService.Get(i => i.ProjectSectionStatementConfirm.Id == item.Id).ToList();

            // مبلغ آخرین صورت وضعیت تایید شده
            Int64 confirmPrice = _projectSectionStatementConfirmService.Get(i => i.Id == item.Id).FirstOrDefault().Price.Value;
            foreach (var value in otherDraft)
                confirmPrice -= (Int64)value.TempDraftPrice;

            //// به دست آوردن جمع کل حواله های تایید شده
            //Int64 draftPrice = 0;

            //var drafts = _ProjectSectionDraftService.Get(i => i.ProjectSectionStatementConfirm.ProjectSectionId == projectSectionId);

            //foreach (var draft in drafts)
            //{
            //    draftPrice += draft.DraftPrice.Value;
            //}


            cpsivm.ProjectSectionDraft.TempDraftPrice = confirmPrice;// -draftPrice;


            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cpsivm);
        }


        [HttpPost]
        public JsonResult _Create(CreateProjectSectionDraftViewModel cpsivm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = cpsivm.ProjectSectionDraft.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _ProjectSectionDraftService.Save(cpsivm, Member.Id, paths);

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
                else if (result.Key == -1)
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
                CreateProjectSectionDraftViewModel cpsivm = new CreateProjectSectionDraftViewModel();
                cpsivm.ProjectSectionDraft = _ProjectSectionDraftService.Find(id);
                cpsivm.ProjectSectionDraft.DraftDate_ = ConvertDate.GetShamsi(cpsivm.ProjectSectionDraft.DraftDate);
                cpsivm.ProjectSectionDraft.DraftPrice = cpsivm.ProjectSectionDraft.TempDraftPrice;

                ReloadFiles();

                foreach (var item in _ProjectSectionDraftFileService.Get(i => i.ProjectSectionDraftId == id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionDraftViewModel());
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

                foreach (var item in _ProjectSectionDraftFileService.Get(i => i.ProjectSectionDraftId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _ProjectSectionDraftFileService.Delete(item);
                }
                
                _ProjectSectionDraftService.Delete(id);

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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionDraft"), "ProjectSectionDraft", "/ProjectDevision/ProjectSectionDraft/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionDraftFiles));

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
        public JsonResult GetProjectSectionDraftFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionDraftFileService.GetByProjectSectionDraftId(Member.Id, projectSectionId)) });
        }


        #endregion


        #region Report

        //public PartialViewResult _Report(long Id)
        //{
        //    StiReport report = new StiReport();
        //    report.Load(@"D:\Report.mrt");
        //    report.Render(false);

        //    MemoryStream stream = new MemoryStream();

        //    StiPdfExportSettings settings = new StiPdfExportSettings();
        //    settings.AutoPrintMode = StiPdfAutoPrintMode.Dialog;

        //    StiPdfExportService service = new StiPdfExportService();
        //    service.ExportPdf(report, stream, settings);

        //    this.Response.Buffer = true;
        //    this.Response.ClearContent();
        //    this.Response.ClearHeaders();
        //    this.Response.ContentType = "application/pdf";
        //    //this.Response.AddHeader("Content-Disposition", "attachment; filename=\"report.pdf\"");
        //    this.Response.ContentEncoding = Encoding.UTF8;
        //    this.Response.AddHeader("Content-Length", stream.Length.ToString());
        //    this.Response.BinaryWrite(stream.ToArray());
        //    this.Response.End();

        //    return PartialView();

        //}
        //public ActionResult PrintReport()
        //{
        //    ////ReportParameterContractorViewModel parameter = (ReportParameterContractorViewModel)TempData["ReportParameterContractor"];

        //    ////var contractor = _contractorService.GetAllReport(parameter).ToList();

        //    //var report = new StiReport();
        //    //report.Load(Server.MapPath("~/Content/Reports/DraftReport.mrt"));
        //    //report.Compile();
        //    ////report.RegBusinessObject("Contractor", contractor);
        //    return StiMvcViewer.PrintReportResult();
        //}

        //public ActionResult GetReportSnapshot()
        //{
        //    //ReportParameterContractorViewModel parameter = (ReportParameterContractorViewModel)TempData["ReportParameterContractor"];

        //    //var contractor = _contractorService.GetAllReport(parameter).ToList();

        //    var report = new StiReport();
        //    report.Load(Server.MapPath("~/Content/Reports/DraftReport.mrt"));
        //    report.Compile();
        //    //report.RegBusinessObject("Contractor", contractor);
        //    return StiMvcViewer.GetReportSnapshotResult(report);
        //}

        //public ActionResult ViewerEvent()
        //{
        //    return StiMvcViewer.ViewerEventResult();
        //}

        #endregion
    }
}