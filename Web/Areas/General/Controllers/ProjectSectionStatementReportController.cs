using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using DatabaseContext.Context;
using ViewModels.ProjectDefine.ProjectStatementViewModel;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.ProjectDevision;

namespace Web.Areas.General
{
    public class ProjectSectionStatementReportController : Controller
    {
        IUnitOfWork _uow;
        IContractorService _contractorService;
        IProjectSectionStatementService _projectSectionStatementService;
        public ProjectSectionStatementReportController(IUnitOfWork uow, IContractorService contractorService, IProjectSectionStatementService projectSectionStatementService)
        {
            _uow = uow;
            _contractorService = contractorService;
            _projectSectionStatementService = projectSectionStatementService;
        }
        //
        // GET: /General/ProjectSectionStatementReport/
        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectStatementViewModel parameter = new ReportParameterProjectStatementViewModel();
            parameter.Contractors = new List<Contractor>() { new Contractor() { CompanyName = "انتخاب کنید..." } };
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            parameter.Contractors = db.Contractors.ToList();
            parameter.StatementNumbers = db.StatementNumbers.ToList();
            //ReportParameterProjectViewModel parameter = new ReportParameterProjectViewModel();
            //parameter.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            //parameter.ProjectTypes.AddRange(_projectTypeService.Get().ToList());

            //parameter.ExecutionTypes = new List<ExecutionType>() { new ExecutionType() { Title = "انتخاب کنید..." } };
            //parameter.ExecutionTypes.AddRange(_executionTypeService.Get().ToList());

            //parameter.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            //parameter.FinantialYears.AddRange(_finantialYearService.Get().ToList());

            //parameter.Citeis = new List<City>() { new City() { Title = "انتخاب کنید..." } };
            //parameter.Citeis.AddRange(_cityService.Get().ToList());

            //parameter.OwnershipTypes = new List<OwnershipType>() { new OwnershipType() { Title = "انتخاب کنید..." } };
            //parameter.OwnershipTypes.AddRange(_ownershipTypeService.Get().ToList());

            return PartialView(parameter);
        }
        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectStatementViewModel parameter)
        {
            TempData["ReportParameterProjectStatement"] = parameter;

            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectStatementViewModel parameter = (ReportParameterProjectStatementViewModel)TempData["ReportParameterProjectStatement"];
            //ReportParameterProjectViewModel parameter = (ReportParameterProjectViewModel)TempData["ReportParameterProject"];
            var ProjectStatement = _projectSectionStatementService.GetAllReport(parameter).ToList();
            //var Project = _projectService.GetAllReport(parameter).ToList();
            var report = new StiReport();
            //var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectStatment.mrt"));
            //report.Load(Server.MapPath("~/Content/Reports/Projects.mrt"));
            report.Compile();
            //report.Compile();
            report.RegBusinessObject("ProjectStatement", ProjectStatement);
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