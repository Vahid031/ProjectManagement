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
using ViewModels.ProjectDefine.ProjectPlanViewModel;

namespace Web.Areas.General
{
    public class ProjectPlansReportController : Controller
    {
        IUnitOfWork _uow;
        IProjectPlanService _projectPlanService;
        IProgramService _programService;
        ICreditProvidePlaceService _creditProvidePlaceService;

        public ProjectPlansReportController(IUnitOfWork uow, IProjectPlanService projectPlanService, IProgramService programService, ICreditProvidePlaceService creditProvidePlaceService)
        {
            _uow = uow;

            _projectPlanService = projectPlanService;
            _programService = programService;
            _creditProvidePlaceService = creditProvidePlaceService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProjectPlanViewModel parameter = new ReportParameterProjectPlanViewModel();
            parameter.Programs = new List<Program>() { new Program() { Title = "انتخاب کنید..." } };
            parameter.Programs.AddRange(_programService.Get().ToList());

            parameter.CreditProvidePlaces = new List<CreditProvidePlace>() { new CreditProvidePlace() { Title = "انتخاب کنید..." } };
            parameter.CreditProvidePlaces.AddRange(_creditProvidePlaceService.Get().ToList());

            TempData["AlertMessage"] = "ssss";

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProjectPlanViewModel parameter)
        {
            TempData["ReportParameterProjectPlan"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectPlanViewModel parameter = (ReportParameterProjectPlanViewModel)TempData["ReportParameterProjectPlan"];

            var ProjectPlan = _projectPlanService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectPlans.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectPlans", ProjectPlan);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}