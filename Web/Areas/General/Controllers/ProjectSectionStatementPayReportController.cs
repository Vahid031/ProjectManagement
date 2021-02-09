using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.ProjectDevision;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectSectionStatementPayReportViewModel;

namespace Web.Areas.General.Controllers
{
    public class ProjectSectionStatementPayReportController : Controller
    {
        //
        // GET: /General/ProjectSectionStatementPayReport/
         IUnitOfWork _uow;
         IProjectSectionPaidPriceService _projectSectionPaidPriceService;

         public ProjectSectionStatementPayReportController(IUnitOfWork uow, IProjectSectionPaidPriceService projectSectionPaidPriceService)
         {
             _uow = uow;
             _projectSectionPaidPriceService = projectSectionPaidPriceService;

         }
        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionStatementPayViewModel parameter = new ReportParameterProjectSectionStatementPayViewModel();
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            parameter.StatementNumbers = db.StatementNumbers.ToList();
            return PartialView(parameter);
        }
        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionStatementPayViewModel parameter)
        {
            TempData["ReportParameterProjectSectionStatementPay"] = parameter;

            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionStatementPayViewModel parameter = (ReportParameterProjectSectionStatementPayViewModel)TempData["ReportParameterProjectSectionStatementPay"];
            ////ReportParameterProjectViewModel parameter = (ReportParameterProjectViewModel)TempData["ReportParameterProject"];
            var ProjectSectionStatementPay = _projectSectionPaidPriceService.GetAllReport(parameter).ToList();
            //var ProjectSectionProjectProgress = _projectSectionSupervisorVisitService.GetAllReportb(parameter).ToList();
            //var report = new StiReport();
            ////var report = new StiReport();
            //report.Load(Server.MapPath("~/Content/Reports/ProjectSectionProjectProgress.mrt"));
            ////report.Load(Server.MapPath("~/Content/Reports/Projects.mrt"));
            //report.Compile();
            ////report.Compile();
            //report.RegBusinessObject("ProjectSectionProjectProgress", ProjectSectionProjectProgress);
            ////report.RegBusinessObject("Projects", Project);
            //return StiMvcViewer.GetReportSnapshotResult(report);

            

            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ReportProjectSectionStatementPay.mrt"));
            report.Compile();
            report.RegBusinessObject("ReportProjectSectionStatementPay", ProjectSectionStatementPay);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
        
	}
}