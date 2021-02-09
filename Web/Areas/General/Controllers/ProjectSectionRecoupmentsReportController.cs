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
using ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionRecoupmentsReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionRecoupmentService _projectSectionRecoupmentService;
        
        public ProjectSectionRecoupmentsReportController(IUnitOfWork uow, IProjectSectionRecoupmentService projectSectionRecoupmentService)
        {
            _uow = uow;

            _projectSectionRecoupmentService = projectSectionRecoupmentService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionRecoupmentViewModel parameter = new ReportParameterProjectSectionRecoupmentViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionRecoupmentViewModel parameter)
        {
            TempData["ReportParameterProjectSectionRecoupment"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionRecoupmentViewModel parameter = (ReportParameterProjectSectionRecoupmentViewModel)TempData["ReportParameterProjectSectionRecoupment"];

            var ProjectSectionRecoupment = _projectSectionRecoupmentService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionRecoupments.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionRecoupments", ProjectSectionRecoupment);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}