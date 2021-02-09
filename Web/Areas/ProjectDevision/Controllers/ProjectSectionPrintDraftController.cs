using DatabaseContext.Context;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.Other;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionPrintDraftController : Controller
    {
        

        #region Index And ctor

        IUnitOfWork _uow;
        IPermissionService _permissionService;


        public ProjectSectionPrintDraftController(
            IUnitOfWork uow, IPermissionService permissionService
            )
        {
            _uow = uow;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index(long Id)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionPrintDraft/_Index", Member.RoleId);
            return PartialView();
        }

        #endregion

        #region Report

        
        public PartialViewResult _Report()
        {
            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            //ReportParameterContractorViewModel parameter = (ReportParameterContractorViewModel)TempData["ReportParameterContractor"];

            //var contractor = _contractorService.GetAllReport(parameter).ToList();

            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/DraftReport.mrt"));
            report.Compile();
            //report.RegBusinessObject("Contractor", contractor);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }


        public ActionResult PrintReport()
        {
            //ReportParameterContractorViewModel parameter = (ReportParameterContractorViewModel)TempData["ReportParameterContractor"];

            //var contractor = _contractorService.GetAllReport(parameter).ToList();

            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/DraftReport.mrt"));
            report.Compile();
            //report.RegBusinessObject("Contractor", contractor);
            return StiMvcViewer.PrintReportResult(report);
        }
        #endregion
    }
}