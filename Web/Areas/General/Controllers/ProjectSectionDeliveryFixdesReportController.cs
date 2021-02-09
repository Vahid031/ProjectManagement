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
using ViewModels.ProjectDevision.ProjectSectionDeliveryFixdViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionDeliveryFixdesReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionDeliveryFixdService _projectSectionDeliveryFixdService;
        
        public ProjectSectionDeliveryFixdesReportController(IUnitOfWork uow, IProjectSectionDeliveryFixdService projectSectionDeliveryFixdService)
        {
            _uow = uow;

            _projectSectionDeliveryFixdService = projectSectionDeliveryFixdService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionDeliveryFixdViewModel parameter = new ReportParameterProjectSectionDeliveryFixdViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionDeliveryFixdViewModel parameter)
        {
            TempData["ReportParameterProjectSectionDeliveryFixd"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionDeliveryFixdViewModel parameter = (ReportParameterProjectSectionDeliveryFixdViewModel)TempData["ReportParameterProjectSectionDeliveryFixd"];

            var ProjectSectionDeliveryFixd = _projectSectionDeliveryFixdService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionDeliveryFixdes.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionDeliveryFixdes", ProjectSectionDeliveryFixd);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}