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
using ViewModels.ProjectDevision.ProjectSectionMadeh46ViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionMadeh46sReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionMadeh46Service _projectSectionMadeh46Service;

        public ProjectSectionMadeh46sReportController(IUnitOfWork uow, IProjectSectionMadeh46Service projectSectionMadeh46Service)
        {
            _uow = uow;

            _projectSectionMadeh46Service = projectSectionMadeh46Service;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionMadeh46ViewModel parameter = new ReportParameterProjectSectionMadeh46ViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionMadeh46ViewModel parameter)
        {
            TempData["ReportParameterProjectSectionMadeh46"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionMadeh46ViewModel parameter = (ReportParameterProjectSectionMadeh46ViewModel)TempData["ReportParameterProjectSectionMadeh46"];

            var ProjectSectionMadeh46 = _projectSectionMadeh46Service.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionMadeh46s.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionMadeh46s", ProjectSectionMadeh46);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}