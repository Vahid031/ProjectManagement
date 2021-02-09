using DatabaseContext.Context;
using Services.Interfaces.ProjectDevision;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectSectionProjectProgressReportViewModel;

namespace Web.Areas.General.Controllers
{
    public class ProjectSectionProjectProgressReportController : Controller
    {
        //
        // GET: /General/ProjectSectionProjectProgressReport/
        IUnitOfWork _uow;
        IProjectSectionSupervisorVisitService _projectSectionSupervisorVisitService;
        public ProjectSectionProjectProgressReportController(IUnitOfWork uow, IProjectSectionSupervisorVisitService projectSectionSupervisorVisitService)
        {
            _uow = uow;
            _projectSectionSupervisorVisitService = projectSectionSupervisorVisitService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionProjectProgressViewModel parameter)
        {
            TempData["ReportParameterProjectSectionProjectProgress"] = parameter;

            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionProjectProgressViewModel parameter = (ReportParameterProjectSectionProjectProgressViewModel)TempData["ReportParameterProjectSectionProjectProgress"];
            //ReportParameterProjectViewModel parameter = (ReportParameterProjectViewModel)TempData["ReportParameterProject"];
            //var ProjectStatement = _projectSectionStatementService.GetAllReport(parameter).ToList();
            var ProjectSectionProjectProgress = _projectSectionSupervisorVisitService.GetAllReportb(parameter).ToList();
            var report = new StiReport();
            //var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionProjectProgress.mrt"));
            //report.Load(Server.MapPath("~/Content/Reports/Projects.mrt"));
            report.Compile();
            //report.Compile();
            report.RegBusinessObject("ProjectSectionProjectProgress", ProjectSectionProjectProgress);
            //report.RegBusinessObject("Projects", Project);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }
        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}