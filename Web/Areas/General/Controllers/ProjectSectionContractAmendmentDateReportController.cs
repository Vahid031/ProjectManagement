using DatabaseContext.Context;
using Services.Interfaces.ProjectDevision;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectSectionContractAmendmentDateReport;

namespace Web.Areas.General.Controllers
{
    public class ProjectSectionContractAmendmentDateReportController : Controller
    {
        //
        // GET: /General/ProjectSectionContractAmendmentDateReport/
         IUnitOfWork _uow;
         IProjectSectionComplementarityDateService _projectSectionComplementarityDateService;
         public ProjectSectionContractAmendmentDateReportController(IUnitOfWork uow, IProjectSectionComplementarityDateService projectSectionComplementarityDateService)
        {
            _uow = uow;

            _projectSectionComplementarityDateService = projectSectionComplementarityDateService;
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
        public PartialViewResult _Report(ReportParameterProjectSectionContractAmendmentDateViewModel parameter)
        {
            TempData["ReportParameterProjectSectionContractAmendmentDateViewModel"] = parameter;

            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionContractAmendmentDateViewModel parameter = (ReportParameterProjectSectionContractAmendmentDateViewModel)TempData["ReportParameterProjectSectionContractAmendmentDateViewModel"];
            //ReportParameterProjectViewModel parameter = (ReportParameterProjectViewModel)TempData["ReportParameterProject"];
            var ProjectSectionContractAmendmentDate = _projectSectionComplementarityDateService.GetAllReport(parameter).ToList();
            //var Project = _projectService.GetAllReport(parameter).ToList();
            var report = new StiReport();
            //var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionContractAmendmentDate.mrt"));
            //report.Load(Server.MapPath("~/Content/Reports/Projects.mrt"));
            report.Compile();
            //report.Compile();
            report.RegBusinessObject("ProjectSectionContractAmendmentDate", ProjectSectionContractAmendmentDate);
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