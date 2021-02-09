using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.ProjectDevision;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectSectionContractAndCostsReportViewModel;

namespace Web.Areas.General.Controllers
{
    public class ProjectSectionContractAndCostsReportController : Controller
    {
        //
        // GET: /General/ProjectSectionContractAndCostsReport/
        IUnitOfWork _uow;
        ICityService _cityService;
        IContractorService _contractorService;
        IContractTypeService _contractTypeService;
        ICreditProvidePlaceService _creditProvidePlaceService;
        IProjectSectionContractAndCostsService _projectSectionContractAndCostsService;
        public ProjectSectionContractAndCostsReportController(IUnitOfWork uow, ICityService cityService, IContractorService contractorService, IContractTypeService _contractTypeService, ICreditProvidePlaceService creditProvidePlaceService, IProjectSectionContractAndCostsService projectSectionContractAndCostsService)
        {
            _uow = uow;
            _cityService = cityService;
            _contractorService = contractorService;
            _creditProvidePlaceService = creditProvidePlaceService;
            _projectSectionContractAndCostsService = projectSectionContractAndCostsService;
        }
        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }
        [AllowAnonymous]
        public PartialViewResult _Search()
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            ReportParameterProjectSectionContractAndCostsViewModel parameter = new ReportParameterProjectSectionContractAndCostsViewModel();
            parameter.Citeies = new List<City>() { new City() { Title = "انتخاب کنید..." } };
            parameter.Citeies.AddRange(_cityService.Get().ToList());

            parameter.Contractors = new List<Contractor>() { new Contractor() { CompanyName = "انتخاب کنید..." } };
            parameter.Contractors.AddRange(_contractorService.Get().ToList());


            parameter.ContractTypes = new List<ContractType>() { new ContractType() { Title = "انتخاب کنید..." } };
            parameter.ContractTypes =db.ContractTypes.ToList();

            parameter.CreditProvidePlaces = new List<CreditProvidePlace>() { new CreditProvidePlace() { Title = "انتخاب کنید..." } };
            parameter.CreditProvidePlaces.AddRange(_creditProvidePlaceService.Get().ToList());
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
        public PartialViewResult _Report(ReportParameterProjectSectionContractAndCostsViewModel parameter)
        {
            TempData["ReportParameterProjectSectionContractAndCosts"] = parameter;
            return PartialView();
        }
        [AllowAnonymous]
        public ActionResult GetReportSnapshot()
        {
            ReportParameterProjectSectionContractAndCostsViewModel parameter = (ReportParameterProjectSectionContractAndCostsViewModel)TempData["ReportParameterProjectSectionContractAndCosts"];

            var ProjectSectionContractAndCostsService = _projectSectionContractAndCostsService.GetAllReport(parameter).ToList();

            var report = new StiReport();
            report.Load(Server.MapPath("~/Content/Reports/ProjectSectionContractAndCostsService.mrt"));
            report.Compile();
            report.RegBusinessObject("ProjectSectionContractAndCostsService", ProjectSectionContractAndCostsService);
            return StiMvcViewer.GetReportSnapshotResult(report);
        }

        [AllowAnonymous]
        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
	}
}