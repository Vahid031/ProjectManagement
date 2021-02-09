using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using ViewModels.ProjectDevision.ProjectSectionFinanceEvaluationViewModel;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionFinanceEvaluationService : GenericService<ProjectSectionFinanceEvaluation>, IProjectSectionFinanceEvaluationService
    {
        public ProjectSectionFinanceEvaluationService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionFinanceEvaluationViewModel> GetAll(ListProjectSectionFinanceEvaluationViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionFinanceEvaluationViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionFinanceEvaluation>()
                .Join(_uow.Set<Contractor>(), ProjectSectionFinanceEvaluation => ProjectSectionFinanceEvaluation.ContractorId, Contractor => Contractor.Id, (ProjectSectionFinanceEvaluation, Contractor) => new { ProjectSectionFinanceEvaluation, Contractor });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionFinanceEvaluationViewModel()
            {
                ProjectSectionFinanceEvaluation = new ProjectSectionFinanceEvaluation() { Id = Result.ProjectSectionFinanceEvaluation.Id, ContractorId = Result.ProjectSectionFinanceEvaluation.ContractorId, Description = Result.ProjectSectionFinanceEvaluation.Description, ProjectSectionId = Result.ProjectSectionFinanceEvaluation.ProjectSectionId, ScoreEvaluation = Result.ProjectSectionFinanceEvaluation.ScoreEvaluation },
                Contractor = new Contractor() { Id = Result.Contractor.Id, Address = Result.Contractor.Address, AgentFirstName = Result.Contractor.AgentFirstName, AgentLastName = Result.Contractor.AgentLastName, AgentMobileNumber = Result.Contractor.AgentMobileNumber, CompanyName = Result.Contractor.CompanyName, Email = Result.Contractor.Email, FaxNumber = Result.Contractor.FaxNumber, IsActive = Result.Contractor.IsActive, ManagerFirstName = Result.Contractor.ManagerFirstName, ManagerLastName = Result.Contractor.ManagerLastName, ManagerMobileNumber = Result.Contractor.ManagerMobileNumber, ManagerNationalCode = Result.Contractor.ManagerNationalCode, PhoneNumber = Result.Contractor.PhoneNumber, Website = Result.Contractor.Website }
            });
        }
    }
}
