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
using ViewModels.BaseInformation.ContractorViewModel;

namespace Web.Areas.General
{
    public class ContractorReportController : Controller
    {
        IUnitOfWork _uow;
        IContractorService _contractorService;
        ILevelTypeService _levelTypeService;
        IMajorTypeService _majorTypeService;

        public ContractorReportController(IUnitOfWork uow, IContractorService contractorService, IMajorTypeService majorTypeService, ILevelTypeService levelTypeService)
        {
            _uow = uow;

            _contractorService = contractorService;
            _levelTypeService = levelTypeService;
            _majorTypeService = majorTypeService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            ReportParameterContractorViewModel parameter = new ReportParameterContractorViewModel();
            parameter.LevelTypes = new List<LevelType>() { new LevelType() { Title = "انتخاب کنید..." } };
            parameter.LevelTypes.AddRange(_levelTypeService.Get().ToList());
            
            parameter.MajorTypes = new List<MajorType>() { new MajorType() { Title = "انتخاب کنید..." } };
            parameter.MajorTypes.AddRange(_majorTypeService.Get().ToList());

            TempData["AlertMessage"] = "ssss";

            return PartialView(parameter);
        }


        [AllowAnonymous]
        public PartialViewResult _Report(ReportParameterContractorViewModel parameter)
        {
            TempData["ReportParameterContractor"] = parameter;

            return PartialView();
        }


        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterContractorViewModel parameter = (ReportParameterContractorViewModel)TempData["ReportParameterContractor"];

            var contractor = _contractorService.GetAllReport(parameter).ToList();
            
            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/Contractor.mrt"));
            report.Compile();
            report.RegBusinessObject("Contractor", contractor);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
       
	}
}