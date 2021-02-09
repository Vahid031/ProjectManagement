using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDevision.ProjectSectionSupervisorVisitViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionSupervisorVisitsReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionSupervisorVisitService _projectSectionSupervisorVisitService;
        ICityService _cityService;
        IMemberService _memberService;

        public ProjectSectionSupervisorVisitsReportController(IUnitOfWork uow, IProjectSectionSupervisorVisitService projectSectionSupervisorVisitService, ICityService cityService, IMemberService memberService)
        {
            _uow = uow;

            _projectSectionSupervisorVisitService = projectSectionSupervisorVisitService;
            _cityService = cityService;
            _memberService = memberService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionSupervisorVisitsViewModel parameter = new ReportParameterProjectSectionSupervisorVisitsViewModel();
            parameter.Cities = new List<City>() { new City() { Title = "انتخاب کنید..." } };
            parameter.Cities.AddRange(_cityService.Get().ToList());

            parameter.Members = new List<Member>() { new Member()};
            parameter.Members.AddRange(_memberService.Get().ToList());

            TempData["AlertMessage"] = "ssss";

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionSupervisorVisitsViewModel parameter)
        {
            TempData["ReportParameterProjectSectionSupervisorVisit"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionSupervisorVisitsViewModel parameter = (ReportParameterProjectSectionSupervisorVisitsViewModel)TempData["ReportParameterProjectSectionSupervisorVisit"];

            var ProjectSectionSupervisorVisit = _projectSectionSupervisorVisitService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionSupervisorVisits.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionSupervisorVisits", ProjectSectionSupervisorVisit);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}