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
using ViewModels.ProjectDefine.ProgramPlanViewModel;

namespace Web.Areas.General
{
    public class ProgramPlansReportController : Controller
    {
        IUnitOfWork _uow;
        IProgramPlanService _programPlanService;
        IProgramService _programService;
        IPlanTypeService _planTypeService;

        public ProgramPlansReportController(IUnitOfWork uow, IProgramPlanService programPlanService, IProgramService programService, IPlanTypeService planTypeService)
        {
            _uow = uow;

            _programPlanService = programPlanService;
            _programService = programService;
            _planTypeService = planTypeService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterProgramPlanViewModel parameter = new ReportParameterProgramPlanViewModel();
            parameter.Programs = new List<Program>() { new Program() { Title = "انتخاب کنید..." } };
            parameter.Programs.AddRange(_programService.Get().ToList());

            parameter.PlanTypes = new List<PlanType>() { new PlanType() { Title = "انتخاب کنید..." } };
            parameter.PlanTypes.AddRange(_planTypeService.Get().ToList());

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterProgramPlanViewModel parameter)
        {
            TempData["ReportParameterProgramPlan"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProgramPlanViewModel parameter = (ReportParameterProgramPlanViewModel)TempData["ReportParameterProgramPlan"];

            var ProgramPlan = _programPlanService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProgramPlans.mrt"));
            report.Compile();
            report.RegBusinessObject("ProgramPlans", ProgramPlan);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}