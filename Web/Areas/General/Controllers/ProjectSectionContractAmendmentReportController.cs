using DatabaseContext.Context;
using Services.Interfaces.ProjectDevision;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectSectionContractAmendmentReport;

namespace Web.Areas.General.Controllers
{
    public class ProjectSectionContractAmendmentReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionComplementarityPriceService _projectSectionComplementarityPriceService;
        public ProjectSectionContractAmendmentReportController(IUnitOfWork uow, IProjectSectionComplementarityPriceService projectSectionComplementarityPriceService)
        {
            _uow = uow;
            _projectSectionComplementarityPriceService = projectSectionComplementarityPriceService;
        }

        //
        // GET: /General/ProjectSectionContractAmendmentReport/
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
        public PartialViewResult _Report(ReportParameterProjectSectionContractAmendmentViewModel parameter)
        {
            TempData["ReportParameterProjectSectionContractAmendmentViewModel"] = parameter;

            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionContractAmendmentViewModel parameter = (ReportParameterProjectSectionContractAmendmentViewModel)TempData["ReportParameterProjectSectionContractAmendmentViewModel"];
            //ReportParameterProjectViewModel parameter = (ReportParameterProjectViewModel)TempData["ReportParameterProject"];
            var ProjectSectionContractAmendment = _projectSectionComplementarityPriceService.GetAllReport(parameter).ToList();
            //var Project = _projectService.GetAllReport(parameter).ToList();
            var report = new StiReport();
            //var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionContractAmendment.mrt"));
            //report.Load(Server.MapPath("~/Content/Reports/Projects.mrt"));
            report.Compile();
            //report.Compile();
            report.RegBusinessObject("ProjectSectionContractAmendment", ProjectSectionContractAmendment);
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