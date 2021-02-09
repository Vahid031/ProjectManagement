using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.ProjectDevision;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDevision.ProjectSectionViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionsReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;

        public ProjectSectionsReportController(IUnitOfWork uow, IProjectSectionService projectSectionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionViewModel parameter = new ReportParameterProjectSectionViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionViewModel parameter)
        {
            TempData["ReportParameterProjectSection"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionViewModel parameter = (ReportParameterProjectSectionViewModel)TempData["ReportParameterProjectSection"];

            var ProjectSection = _projectSectionService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSections.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSections", ProjectSection);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}