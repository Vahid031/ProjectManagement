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
using ViewModels.ProjectDevision.ProjectSectionDepositeLiberalizationViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionDepositeLiberalizationsReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionDepositeLiberalizationService _projectSectionDepositeLiberalizationService;

        public ProjectSectionDepositeLiberalizationsReportController(IUnitOfWork uow, IProjectSectionDepositeLiberalizationService projectSectionDepositeLiberalizationService)
        {
            _uow = uow;

            _projectSectionDepositeLiberalizationService = projectSectionDepositeLiberalizationService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionDepositeLiberalizationsViewModel parameter = new ReportParameterProjectSectionDepositeLiberalizationsViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionDepositeLiberalizationsViewModel parameter)
        {
            TempData["ReportParameterProjectSectionDepositeLiberalization"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionDepositeLiberalizationsViewModel parameter = (ReportParameterProjectSectionDepositeLiberalizationsViewModel)TempData["ReportParameterProjectSectionDepositeLiberalization"];

            var ProjectSectionDepositeLiberalization = _projectSectionDepositeLiberalizationService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/DepositeLiberalizations.mrt"));
            report.Compile();
            report.RegBusinessObject("DepositeLiberalizations", ProjectSectionDepositeLiberalization);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}