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
using ViewModels.ProjectDevision.ProjectSectionDeliveryTemproryViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionDeliveryTemproriesReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionDeliveryTemproryService _projectSectionDeliveryTemproryService;

        public ProjectSectionDeliveryTemproriesReportController(IUnitOfWork uow, IProjectSectionDeliveryTemproryService projectSectionDeliveryTemproryService)
        {
            _uow = uow;

            _projectSectionDeliveryTemproryService = projectSectionDeliveryTemproryService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionDeliveryTemproryViewModel parameter = new ReportParameterProjectSectionDeliveryTemproryViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionDeliveryTemproryViewModel parameter)
        {
            TempData["ReportParameterProjectSectionDeliveryTemprory"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionDeliveryTemproryViewModel parameter = (ReportParameterProjectSectionDeliveryTemproryViewModel)TempData["ReportParameterProjectSectionDeliveryTemprory"];

            var ProjectSectionDeliveryTemprory = _projectSectionDeliveryTemproryService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionDeliveryTemprories.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionDeliveryTemprories", ProjectSectionDeliveryTemprory);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}