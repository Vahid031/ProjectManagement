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
using ViewModels.ProjectDevision.ProjectSectionMadeh48ViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionMadeh48sReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionMadeh48Service _projectSectionMadeh48Service;

        public ProjectSectionMadeh48sReportController(IUnitOfWork uow, IProjectSectionMadeh48Service projectSectionMadeh48Service)
        {
            _uow = uow;

            _projectSectionMadeh48Service = projectSectionMadeh48Service;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionMadeh48ViewModel parameter = new ReportParameterProjectSectionMadeh48ViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionMadeh48ViewModel parameter)
        {
            TempData["ReportParameterProjectSectionMadeh48"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionMadeh48ViewModel parameter = (ReportParameterProjectSectionMadeh48ViewModel)TempData["ReportParameterProjectSectionMadeh48"];

            var ProjectSectionMadeh48 = _projectSectionMadeh48Service.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionMadeh48s.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionMadeh48s", ProjectSectionMadeh48);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}