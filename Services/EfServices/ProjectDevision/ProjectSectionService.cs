using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using ViewModels.ProjectDevision.ProjectSectionViewModel;
using DomainModels.Entities.BaseInformation;
using ViewModels.ProjectDevision.ProjectSectionAssignmentViewModel;
using ViewModels.ProjectDevision.ProjectSectionOperationViewModel;
using ViewModels.ProjectDevision.ProjectSectionTemproryViewModel;
using ViewModels.ProjectDevision.ProjectSectionFixedViewModel;
using ViewModels.ProjectDevision.ProjectSectionLiberalizationViewModel;
using ViewModels.ProjectDevision.ProjectSection46ViewModel;
using ViewModels.ProjectDevision.ProjectSection48ViewModel;
using ViewModels.ProjectDevision.ProjectSectionLetterViewModel;
using ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel;
using ViewModels.ProjectDevision.ProjectSectionRecoupmentListViewModel;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionService : GenericService<ProjectSection>, IProjectSectionService
    {
        public ProjectSectionService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionViewModel> GetAll(ListProjectSectionViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSection>()
                .Join(_uow.Set<AssignmentType>(), ProjectSection => ProjectSection.AssignmentTypeId, AssignmentType => AssignmentType.Id, (ProjectSection, AssignmentType) => new { ProjectSection, AssignmentType })
                .Join(_uow.Set<ProjectSectionOperationState>(), ProjectSection => ProjectSection.ProjectSection.ProjectSectionOperationStateId, ProjectSectionOperationState => ProjectSectionOperationState.Id, (ProjectSection, ProjectSectionOperationState) => new { ProjectSection.ProjectSection, ProjectSection.AssignmentType, ProjectSectionOperationState })
                .Join(_uow.Set<ProjectSectionAssignmentState>(), ProjectSection => ProjectSection.ProjectSection.ProjectSectionAssignmentStateId, ProjectSectionAssignmentState => ProjectSectionAssignmentState.Id, (ProjectSection, ProjectSectionAssignmentState) => new { ProjectSection.ProjectSection, ProjectSection.AssignmentType, ProjectSection.ProjectSectionOperationState, ProjectSectionAssignmentState })
                ;

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionViewModel()
            {
                ProjectSection = new ProjectSection() { Id = Result.ProjectSection.Id, ProjectId = Result.ProjectSection.ProjectId, AssignmentTypeId = Result.ProjectSection.AssignmentTypeId, Title = Result.ProjectSection.Title, ForecastEndDate = Result.ProjectSection.ForecastEndDate, ForecastStartDate = Result.ProjectSection.ForecastStartDate },
                AssignmentType = new AssignmentType() { Id = Result.AssignmentType.Id, Title = Result.AssignmentType.Title },
                ProjectSectionOperationState = new ProjectSectionOperationState() { Id = Result.ProjectSectionOperationState.Id, Title = Result.ProjectSectionOperationState.Title, Color = Result.ProjectSectionOperationState.Color },
                ProjectSectionAssignmentState = new ProjectSectionAssignmentState() { Id = Result.ProjectSectionAssignmentState.Id, Title = Result.ProjectSectionAssignmentState.Title, Color = Result.ProjectSectionAssignmentState.Color },
                
            });
        }



        public IEnumerable<ListProjectSection2ViewModel> GetAll2(ListProjectSection2ViewModel lpsvm, ref Paging pg)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsvm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            query += @"
                    SELECT	PS.Id AS Id,
                            PS.ProjectId,
                            PS.Title AS ProjectSectionTitle,
		                    PS.ForecastStartDate,
		                    PS.ForecastEndDate,
		                    AT.Title AS AssignmentTypeTitle,
                            (SELECT CAST(ISNULL(SUM(PSP.EstimatesPrice), 0) AS BIGINT) FROM ProjectDevision.ProjectSectionProducts PSP WHERE PSP.ProjectSectionId = PS.Id) AS TotalProductServicePrice
                    FROM ProjectDevision.ProjectSections PS
	                    JOIN BaseInformation.AssignmentTypes AT
		                    ON PS.AssignmentTypeId = AT.Id
	                    JOIN BaseInformation.ProjectSectionOperationStates PSOS
		                    ON PS.ProjectSectionOperationStateId = PSOS.Id
	                    JOIN BaseInformation.ProjectSectionAssignmentStates PSAS
		                    ON PS.ProjectSectionAssignmentStateId = PSAS.Id
                    ) Result"
            + filter;

           

            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";
            string s = allField + query + orderQuery;

            IEnumerable<ListProjectSection2ViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSection2ViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSection2ViewModel()
                {
                    Id = Result.Id,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    ForecastStartDate = Result.ForecastStartDate,
                    ForecastEndDate = Result.ForecastEndDate,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    TotalProductServicePrice = Result.TotalProductServicePrice

                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }







        public IEnumerable<ListProjectSectionAssignmentViewModel> GetProjectAssignment(ListProjectSectionAssignmentViewModel lpsavm, ref Paging pg)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsavm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
	                       BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
	                       PDPS.ProjectSectionAssignmentStateId, BIPSAS.Title ProjectSectionAssignmentStateTitle, BIPSAS.Color
                    From ProjectDevision.ProjectSections PDPS
		                     inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
		                     inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
		                     inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
		                     inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
		                     inner join BaseInformation.ProjectSectionAssignmentStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionAssignmentStateId
                      ) Result"
            + filter;


            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSectionAssignmentViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSectionAssignmentViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSectionAssignmentViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ProjectSectionAssignmentStateId = Result.ProjectSectionAssignmentStateId,
                    ProjectSectionAssignmentStateTitle = Result.ProjectSectionAssignmentStateTitle,
                    Color = Result.Color,

                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ListProjectSectionOperationViewModel> GetProjectOperation(ListProjectSectionOperationViewModel lpsovm, ref Paging pg, Int64 MemberId)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            if (MemberId != 0)
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                    where PDPS.Id in (Select ProjectSectionId from ProjectDevision.ProjectSectionMembers Where MemberId = " + MemberId.ToString() + @")
                      ) Result"
                + filter;
//                query += @"
//                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
//                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
//                           PDPS.ProjectSectionOperationStateId, BIPSAS.Color,
//						   CASE PDPS.Id
//						   when (pdpspp.ProjectSectionId) then 'پرداخت شده'
//						   when (PDPSD.ProjectSectionId) then 'منتظر پرداخت'
//						   when (PDPSSC.ProjectSectionId) then 'منتظر صدور و تأیید فرم حواله'
//						   when (PDPSS.ProjectSectionId) then 'منتظر تأیید صورت وضعیت'
//						   else 'منتظر صدور صورت وضعیت' end as ProjectSectionOperationStateTitle,
//                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
//                    From ProjectDevision.ProjectSections PDPS
//                             INNER join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
//                             INNER join BaseInformation.States BIS on BIS.Id = PDP.StateId
//                             INNER join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
//                             INNER join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
//                             INNER join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
//                             INNER join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
//                             INNER join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
//                             INNER join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
//							 left join [ProjectDevision].[ProjectSectionPaidPrices] pdpspp on PDPS.Id=pdpspp.ProjectSectionId
//							 left join [ProjectDevision].[ProjectSectionDrafts] PDPSD ON PDPS.Id = PDPSD.ProjectSectionId
//							 LEFT JOIN [ProjectDevision].[ProjectSectionStatementConfirms] PDPSSC ON PDPS.Id = PDPSSC.ProjectSectionId
//							 LEFT JOIN [ProjectDevision].[ProjectSectionStatements] PDPSS ON PDPS.Id = PDPSS.ProjectSectionId
//                    where PDPS.Id in (Select ProjectSectionId from ProjectDevision.ProjectSectionMembers Where MemberId = " + MemberId.ToString() + @")
//                      ) Result"
//                + filter;
            }
            else
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
                + filter;
            
            }

            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSectionOperationViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSectionOperationViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSectionOperationViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                    //ProjectSectionOperationStateId = Result.ProjectSectionOperationStateId,
                    //ProjectSectionOperationStateTitle = Result.ProjectSectionOperationStateTitle,
                    Color = Result.Color,

                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ListProjectSectionTemproryViewModel> GetProjectTempDelivery(ListProjectSectionTemproryViewModel lpsovm, ref Paging pg, Int64 MemberId)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            if (MemberId != 0)
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                    where PDPS.Id in (Select ProjectSectionId from ProjectDevision.ProjectSectionMembers Where IsMainSupervisor = 1 and MemberId = " + MemberId.ToString() + @")
                      ) Result"
                + filter;
            }
            else
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
                + filter;
            }

            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSectionTemproryViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSectionTemproryViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSectionTemproryViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ListProjectSectionFixedViewModel> GetProjectFixedDelivery(ListProjectSectionFixedViewModel lpsovm, ref Paging pg, Int64 MemberId)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            if (MemberId != 0)
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                    where PDPS.Id in (Select ProjectSectionId from ProjectDevision.ProjectSectionMembers Where IsMainSupervisor = 1 and MemberId = " + MemberId.ToString() + @")
                      ) Result"
                + filter;
            }
            else
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
                + filter;
            
            }

            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSectionFixedViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSectionFixedViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSectionFixedViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ListProjectSectionLiberalizationViewModel> GetProjectLiberalization(ListProjectSectionLiberalizationViewModel lpsovm, ref Paging pg)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
            + filter;


            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSectionLiberalizationViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSectionLiberalizationViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSectionLiberalizationViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ListProjectSection46ViewModel> GetProjectMadeh46(ListProjectSection46ViewModel lpsovm, ref Paging pg, Int64 MemberId)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            if (MemberId != 0)
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                    where PDPS.Id in (Select ProjectSectionId from ProjectDevision.ProjectSectionMembers Where IsMainSupervisor = 1 and MemberId = " + MemberId.ToString() + @")
                      ) Result"
                + filter;
            }
            else
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
                + filter;
            }

            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSection46ViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSection46ViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSection46ViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ListProjectSection48ViewModel> GetProjectMadeh48(ListProjectSection48ViewModel lpsovm, ref Paging pg, Int64 MemberId)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            if (MemberId != 0)
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                    where PDPS.Id in (Select ProjectSectionId from ProjectDevision.ProjectSectionMembers Where IsMainSupervisor = 1 and MemberId = " + MemberId.ToString() + @")
                      ) Result"
                + filter;
            }
            else
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
                + filter;
            }

            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSection48ViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSection48ViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSection48ViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public IEnumerable<ListProjectSectionLetterViewModel> GetProjectLetter(ListProjectSectionLetterViewModel lpsovm, ref Paging pg, Int64 MemberId)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            if (MemberId != 0)
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                    where PDPS.Id in (Select ProjectSectionId from ProjectDevision.ProjectSectionMembers Where MemberId = " + MemberId.ToString() + @")
                      ) Result"
                + filter;
            }
            else 
            {
                query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
                + filter;
            
            }
            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSectionLetterViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSectionLetterViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSectionLetterViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }
        public IEnumerable<ListProjectSectionRecoupmentListViewModel> GetProjectRecoupment(ListProjectSectionRecoupmentListViewModel lpsovm, ref Paging pg)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpsovm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            query += @"
                    Select PDPS.Id, PDPS.ProjectId, PDP.ProjectTitle, PDP.FileCode, PDP.StateId, BIS.Title StateTitle, PDP.CityId, 
                           BIC.Title CityTitle, PDPS.Title ProjectSectionTitle, PDPS.AssignmentTypeId, BIA.Title AssignmentTypeTitle,
                           PDPS.ProjectSectionOperationStateId, BIPSAS.Title ProjectSectionOperationStateTitle, BIPSAS.Color,
                           PDPSC.ContractNumber, PDPSC.StartDate, PDPSC.EndDate, BICO.CompanyName
                    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                             inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                             inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                             inner join BaseInformation.AssignmentTypes BIA on BIA.Id = PDPS.AssignmentTypeId
                             inner join BaseInformation.ProjectSectionOperationStates BIPSAS on BIPSAS.Id = PDPS.ProjectSectionOperationStateId
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)
                             inner join BaseInformation.Contractors BICO on BICO.Id = PDPSW.ContractorId
                      ) Result"
            + filter;


            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectSectionRecoupmentListViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectSectionRecoupmentListViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectSectionRecoupmentListViewModel()
                {
                    Id = Result.Id,
                    ProjectId = Result.ProjectId,
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    ProjectSectionTitle = Result.ProjectSectionTitle,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ContractNumber = Result.ContractNumber,
                    CompanyName = Result.CompanyName,
                    StartDate = Result.StartDate,
                    EndDate = Result.EndDate,
                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }


        public IEnumerable<ReportProjectSectionViewModel> GetAllReport(ReportParameterProjectSectionViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";

            if (parameters.FileCode != null)
            {
                tempFilter += and;
                tempFilter += "Result.FileCode Like N'%" + parameters.FileCode + "%'";
                and = " AND ";
            }

            if (parameters.ProjectTitle != null)
            {
                tempFilter += and;
                tempFilter += "Result.ProjectTitle Like N'%" + parameters.ProjectTitle + "%'";
                and = " AND ";
            }


            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPS.AssignmentTypeId, BIAT.Title AssignmentTypeTitle,
                       	   PDPS.ForecastStartDate, PDPS.ForecastEndDate
                       From ProjectDevision.ProjectSections PDPS
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       	   inner join BaseInformation.AssignmentTypes BIAT on PDP.Id = PDPS.ProjectId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    AssignmentTypeId = Result.AssignmentTypeId,
                    AssignmentTypeTitle = Result.AssignmentTypeTitle,
                    ForecastStartDate = Result.ForecastStartDate,
                    ForecastEndDate = Result.ForecastEndDate
                });

            return result;
        }
    }




}
