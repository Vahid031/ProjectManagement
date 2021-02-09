using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.BaseInformation;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectViewModel;

namespace Web.Areas.General
{
    public class ProjectsReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectService _projectService;
        IProjectTypeService _projectTypeService;
        IExecutionTypeService _executionTypeService;
        IFinantialYearService _finantialYearService;
        ICityService _cityService;
        IOwnershipTypeService _ownershipTypeService;

        public ProjectsReportController(IUnitOfWork uow, IProjectService projectService, IProjectTypeService projectTypeService, IExecutionTypeService executionTypeService, IFinantialYearService finantialYearService, ICityService cityService, IOwnershipTypeService ownershipTypeService)
        {
            _uow = uow;

            _projectService = projectService;
            _projectTypeService = projectTypeService;
            _executionTypeService = executionTypeService;
            _finantialYearService = finantialYearService;
            _cityService = cityService;
            _ownershipTypeService = ownershipTypeService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectViewModel parameter = new ReportParameterProjectViewModel();
            parameter.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            parameter.ProjectTypes.AddRange(_projectTypeService.Get().ToList());

            parameter.ExecutionTypes = new List<ExecutionType>() { new ExecutionType() { Title = "انتخاب کنید..." } };
            parameter.ExecutionTypes.AddRange(_executionTypeService.Get().ToList());

            parameter.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            parameter.FinantialYears.AddRange(_finantialYearService.Get().ToList());

            parameter.Citeis = new List<City>() { new City() { Title = "انتخاب کنید..." } };
            parameter.Citeis.AddRange(_cityService.Get().ToList());

            parameter.OwnershipTypes = new List<OwnershipType>() { new OwnershipType() { Title = "انتخاب کنید..." } };
            parameter.OwnershipTypes.AddRange(_ownershipTypeService.Get().ToList());

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectViewModel parameter)
        {
            TempData["ReportParameterProject"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectViewModel parameter = (ReportParameterProjectViewModel)TempData["ReportParameterProject"];

            var Project = _projectService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/Projects.mrt"));
            report.Compile();
            report.RegBusinessObject("Projects", Project);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}