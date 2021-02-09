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
using ViewModels.ProjectDevision.ProjectSectionContractViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionContractsReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionContractService _projectSectionContractService;

        public ProjectSectionContractsReportController(IUnitOfWork uow, IProjectSectionContractService projectSectionContractService)
        {
            _uow = uow;

            _projectSectionContractService = projectSectionContractService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionContractViewModel parameter = new ReportParameterProjectSectionContractViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionContractViewModel parameter)
        {
            TempData["ReportParameterProjectSectionContract"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionContractViewModel parameter = (ReportParameterProjectSectionContractViewModel)TempData["ReportParameterProjectSectionContract"];

            var ProjectSectionContract = _projectSectionContractService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionContracts.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionContracts", ProjectSectionContract);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}