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
using ViewModels.ProjectDevision.ProjectSectionFinalParticipantViewModel;

namespace Web.Areas.General
{
    public class ProjectSectionFinalParticipantsReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectSectionFinalParticipantService _projectSectionFinalParticipantService;

        public ProjectSectionFinalParticipantsReportController(IUnitOfWork uow, IProjectSectionFinalParticipantService projectSectionFinalParticipantService)
        {
            _uow = uow;

            _projectSectionFinalParticipantService = projectSectionFinalParticipantService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectSectionFinalParticipantViewModel parameter = new ReportParameterProjectSectionFinalParticipantViewModel();

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectSectionFinalParticipantViewModel parameter)
        {
            TempData["ReportParameterProjectSectionFinalParticipant"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionFinalParticipantViewModel parameter = (ReportParameterProjectSectionFinalParticipantViewModel)TempData["ReportParameterProjectSectionFinalParticipant"];

            var ProjectSectionFinalParticipant = _projectSectionFinalParticipantService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionFinalParticipants.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionFinalParticipants", ProjectSectionFinalParticipant);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}