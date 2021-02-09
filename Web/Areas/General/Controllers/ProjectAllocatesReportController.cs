using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.ProjectDefine;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectAllocateViewModel;

namespace Web.Areas.General
{
    public class ProjectAllocatesReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectAllocateService _projectAllocateService;
        ICityService _cityService;
        IProjectTypeService _projectTypeService;

        public ProjectAllocatesReportController(IUnitOfWork uow, IProjectAllocateService projectAllocateService, ICityService cityService, IProjectTypeService projectTypeService)
        {
            _uow = uow;

            _projectAllocateService = projectAllocateService;
            _cityService = cityService;
            _projectTypeService = projectTypeService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectAllocateViewModel parameter = new ReportParameterProjectAllocateViewModel();
            parameter.Cities = new List<City>() { new City() { Title = "انتخاب کنید..." } };
            parameter.Cities.AddRange(_cityService.Get().ToList());

            parameter.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            parameter.ProjectTypes.AddRange(_projectTypeService.Get().ToList());

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectAllocateViewModel parameter)
        {
            TempData["ReportParameterProjectAllocate"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectAllocateViewModel parameter = (ReportParameterProjectAllocateViewModel)TempData["ReportParameterProjectAllocate"];

            var ProjectAllocate = _projectAllocateService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectAllocates.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectAllocates", ProjectAllocate);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}