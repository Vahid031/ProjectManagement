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
using ViewModels.ProjectDevision.ProjectSectionWinnerViewModel;
using DomainModels.Entities.BaseInformation;
using System.Data.SqlClient;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionWinnerService : GenericService<ProjectSectionWinner>, IProjectSectionWinnerService
    {
        public ProjectSectionWinnerService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionWinnerViewModel> GetAll(ListProjectSectionWinnerViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionWinnerViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionWinner>()
                .Join(_uow.Set<Rank>(), ProjectSectionWinner => ProjectSectionWinner.RankId, Rank => Rank.Id, (ProjectSectionWinner, Rank) => new { ProjectSectionWinner, Rank })
                .Join(_uow.Set<Contractor>(), ProjectSectionWinner => ProjectSectionWinner.ProjectSectionWinner.ContractorId, Contractor => Contractor.Id, (ProjectSectionWinner, Contractor) => new { ProjectSectionWinner.ProjectSectionWinner, ProjectSectionWinner.Rank, Contractor });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionWinnerViewModel()
            {
                ProjectSectionWinner = new ProjectSectionWinner() { Id = Result.ProjectSectionWinner.Id, ProjectSectionId = Result.ProjectSectionWinner.ProjectSectionId, ContractorId = Result.ProjectSectionWinner.ContractorId, RankId = Result.ProjectSectionWinner.RankId },
                Rank = new Rank() { Id = Result.Rank.Id, Title = Result.Rank.Title },
                Contractor = new Contractor() { Id = Result.Contractor.Id, Address = Result.Contractor.Address, AgentFirstName = Result.Contractor.AgentFirstName, AgentLastName = Result.Contractor.AgentLastName, AgentMobileNumber = Result.Contractor.AgentMobileNumber, CompanyName = Result.Contractor.CompanyName, Email = Result.Contractor.Email, FaxNumber = Result.Contractor.FaxNumber, IsActive = Result.Contractor.IsActive, ManagerFirstName = Result.Contractor.ManagerFirstName, ManagerLastName = Result.Contractor.ManagerLastName, ManagerMobileNumber = Result.Contractor.ManagerMobileNumber, ManagerNationalCode = Result.Contractor.ManagerNationalCode, PhoneNumber = Result.Contractor.PhoneNumber, Website = Result.Contractor.Website }
            });
        }







        public IEnumerable<ListProjectSection4648ViewModel> Get4648(long contractorId, ref Paging pg)
        {
            var param = new SqlParameter {
                ParameterName = "contractorId",
                Value = contractorId
            };

            IEnumerable<ListProjectSection4648ViewModel> GetList = _uow.GetInstance().Database.SqlQuery<ListProjectSection4648ViewModel>("exec GetMadeh4648 " + contractorId)
            .Select(Result => new ListProjectSection4648ViewModel()
            {
                Date_ = Result.Date_,
                Type4648 = Result.Type4648,
                EndDate_ = Result.EndDate_,
                StartDate_ = Result.StartDate_,
                ProjectSectionTitle = Result.ProjectSectionTitle,
                ProjectTitle = Result.ProjectTitle,
                ProjectNumber = Result.ProjectNumber 
            });

            return GetList.ToList();
        }





    }
}
