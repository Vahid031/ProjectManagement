using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using ViewModels.ProjectDefine.ProjectViewModel;
using DomainModels.Entities.ProjectDefine;
using Services.Interfaces.BaseInformation;
using DomainModels.Entities.General;
using Services.EfServices.General;
using DomainModels.Enums;
using ViewModels.ProjectDefine.ProjectAgreementViewModel;
using Services.EfServices.ProjectDefine;

namespace Services.EfServices.BaseInformation
{
    public class ProjectService : GenericService<Project>, IProjectService
    {
        public ProjectService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectViewModel> GetAll(ListProjectViewModel lpvm, ref Paging pg)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpvm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            query += @"Select PDP.Id, PDP.ProjectTitle, PDP.StateId, BIS.Title StateTitle, PDP.CityId, BIC.Title CityTitle, ISNULL(StartFinantialYearId, -1) StartFinantialYearId,
                       	   ISNULL(BISFY.Title, '') StartFinantialYearTitle, ISNULL(ForecastEndFinantialYearId, -1) ForecastEndFinantialYearId,
                       	   ISNULL(BIFEFY.Title, '') ForecastEndFinantialYearTitle, ISNULL(CAST(PDP.Area as nvarchar(20)), '') Area
                       
                       From ProjectDefine.Projects PDP
                       		 inner join BaseInformation.States BIS on BIS.Id = PDP.StateId
                       		 inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                       		 left  join BaseInformation.FinantialYears BISFY on BISFY.Id = PDP.StartFinantialYearId
                       		 left  join BaseInformation.FinantialYears BIFEFY on BIFEFY.Id = PDP.ForecastEndFinantialYearId
                      ) Result"
            + filter;


            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectViewModel()
                {
                    Id = Result.Id,
                    ProjectTitle = Result.ProjectTitle,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    StartFinantialYearId = Result.StartFinantialYearId,
                    StartFinantialYearTitle = Result.StartFinantialYearTitle,
                    ForecastEndFinantialYearId = Result.ForecastEndFinantialYearId,
                    ForecastEndFinantialYearTitle = Result.ForecastEndFinantialYearTitle,
                    Area = Result.Area

                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }


        public IEnumerable<ListProjectAgreementViewModel> GetProjectAggrement(ListProjectAgreementViewModel lpvm, ref Paging pg)
        {
            string tempFilter = DynamicFiltering.GetSqlFilterDynamic(lpvm, "Result");
            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string allField = @"SELECT * FROM (";
            string query = "";
            query += @"
                    Select PDP.Id,  ISNULL(PDP.FileCode, '') FileCode, PDP.ProjectTitle, PDP.ProjectTypeId, BIPT.Title ProjectTypeTitle,
	                       ISNULL(PDP.StartFinantialYearId, -1) StartFinantialYearId, ISNULL(BISFY.Title, '') StartFinantialYearTitle,
	                       ISNULL(PDP.ForecastEndFinantialYearId, -1) ForecastEndFinantialYearId, ISNULL(BIFEFY.Title, '') ForecastEndFinantialYearTitle, 
	                       PDP.StateId, BIST.Title StateTitle, PDP.CityId, BIC.Title CityTitle, ISNULL(PDP.SectionId, -1) SectionId,
	                       ISNULL(BISE.Title, '') SectionTitle, ISNULL(PDP.VillageId, -1) VillageId, ISNULL(BIV.Title, '') VillageTitle, 
	                       ISNULL(PDP.RuralDistrictId, -1) RuralDistrictId, ISNULL(BIRD.Title, '') RuralDistrictTitle, 
	                       ISNULL(CAST(PDP.Amount as nvarchar(10)), '') Amount, ISNULL(PDP.UnitId, -1) UnitId, ISNULL(BIU.Title, '') UnitTitle, 
	                       ISNULL(CAST(PDP.EstimateCost as nvarchar(10)), '') EstimateCost, PDP.ProjectStateId, 
	                       BIPS.Title ProjectStateTitle, BIPS.Color
                    From ProjectDefine.Projects PDP
	                       inner join BaseInformation.ProjectTypes BIPT on BIPT.Id = PDP.ProjectTypeId
	                       left join  BaseInformation.FinantialYears BISFY on BISFY.Id = PDP.StartFinantialYearId
	                       left join  BaseInformation.FinantialYears BIFEFY on BIFEFY.Id = PDP.ForecastEndFinantialYearId
	                       inner join BaseInformation.States BIST on BIST.Id = PDP.StateId
	                       inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
	                       left join BaseInformation.Sections BISE on BISE.Id = PDP.SectionId
	                       left join BaseInformation.Villages BIV on BIV.Id = PDP.VillageId
	                       left join BaseInformation.RuralDistricts BIRD on BIRD.Id = PDP.RuralDistrictId
	                       left join BaseInformation.Units BIU on BIU.Id = PDP.UnitId
	                       inner join BaseInformation.ProjectStates BIPS on BIPS.Id = PDP.ProjectStateId
                      ) Result"
            + filter;


            string orderQuery = @" ORDER BY " + orderby[0] + " " + orderby[1] + @" OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            IEnumerable<ListProjectAgreementViewModel> result = _uow.GetInstance().Database.SqlQuery<ListProjectAgreementViewModel>(allField + query + orderQuery)
                .Select(Result => new ListProjectAgreementViewModel()
                {
                    Id = Result.Id,
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    ProjectTypeId = Result.ProjectTypeId,
                    ProjectTypeTitle = Result.ProjectTypeTitle,
                    StartFinantialYearId = Result.StartFinantialYearId,
                    StartFinantialYearTitle = Result.StartFinantialYearTitle,
                    ForecastEndFinantialYearId = Result.ForecastEndFinantialYearId,
                    ForecastEndFinantialYearTitle = Result.ForecastEndFinantialYearTitle,
                    StateId = Result.StateId,
                    StateTitle = Result.StateTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    SectionId = Result.SectionId,
                    SectionTitle = Result.SectionTitle,
                    VillageId = Result.VillageId,
                    VillageTitle = Result.VillageTitle,
                    RuralDistrictId = Result.RuralDistrictId,
                    RuralDistrictTitle = Result.RuralDistrictTitle,
                    Amount = Result.Amount,
                    UnitId = Result.UnitId,
                    UnitTitle = Result.UnitTitle,
                    ProjectStateId = Result.ProjectStateId,
                    ProjectStateTitle = Result.ProjectStateTitle,
                    Color = Result.Color,

                });

            string queryCount = @"SELECT CAST(COUNT(Result.Id) AS BIGINT) AS [Count] FROM (";

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount + query).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return result;
        }

        public KeyValuePair<Int64, string> Save(CreateProjectViewModel catvm, Int64 MemberId, List<string> paths)
        {
            ProjectService _projectService = new ProjectService(_uow);
            ProjectPlanService _projectPlanService = new ProjectPlanService(_uow);
            ProjectFileService _projectFileService = new ProjectFileService(_uow);
            ProjectAllocateService _projectAllocateService = new ProjectAllocateService(_uow);
            FileService _fileService = new FileService(_uow);
            
            long? isInsert = catvm.Project.Id;

            if (isInsert == -1)
            {
                _projectService.Insert(catvm.Project);
            }
            else
            {

                var projectAllocatePlans = _projectAllocateService.Get(i => i.ProjectId == catvm.Project.Id);

                if (projectAllocatePlans.Count() > 0)
                {
                    return new KeyValuePair<long, string>(0, "امکان ویرایش پروژه به دلیل ثبت طرح در تخصیص وجود ندارد");
                }

                _projectService.Update(catvm.Project);

                foreach (var item in _projectFileService.Get(i => i.ProjectId == catvm.Project.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _projectFileService.Delete(item);
                    }
                }

                foreach (var item in _projectPlanService.Get(i => i.ProjectId == catvm.Project.Id))
                {
                    _projectPlanService.Delete(item);
                }
            }

            if (!String.IsNullOrEmpty(catvm.ProjectPlans))
            {
                foreach (var item in catvm.ProjectPlans.Split('|'))
                {
                    if (!String.IsNullOrEmpty(item))
                    {
                        ProjectPlan pp = new ProjectPlan();
                        pp.ProjectId = catvm.Project.Id;
                        pp.ProgramId = Convert.ToInt64(item.Split(',')[0].Split(':')[1]);
                        pp.ProgramPlanId = Convert.ToInt64(item.Split(',')[2].Split(':')[1]);

                        if (!string.IsNullOrEmpty(item.Split(',')[4].Split(':')[1]))
                        {
                            pp.CreditProvidePlaceId = Convert.ToInt64(item.Split(',')[4].Split(':')[1]);
                        }

                        if (!string.IsNullOrEmpty(item.Split(',')[6].Split(':')[1]))
                        {
                            pp.FromFinantialYearId = Convert.ToInt64(item.Split(',')[6].Split(':')[1]);
                        }

                        if (!string.IsNullOrEmpty(item.Split(',')[8].Split(':')[1]))
                        {
                            pp.ToFinantialYearId = Convert.ToInt64(item.Split(',')[8].Split(':')[1]);
                        }

                        _projectPlanService.Insert(pp);
                    }
                }
            }


            if (catvm.Files != null)
            {
                foreach (string item in catvm.Files.Split(','))
                {
                    if (item != "")
                    {
                        Int64 fileId = Convert.ToInt64(item);
                        if (_fileService.Get(i => i.Id == fileId).First().FileState.Value == 1)
                        {
                            ProjectFile projectFile = new ProjectFile();
                            projectFile.ProjectId = catvm.Project.Id;
                            projectFile.FileId = fileId;

                            _projectFileService.Insert(projectFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(catvm.Project.Id.Value, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

        public IEnumerable<ReportProjectViewModel> GetAllReport(ReportParameterProjectViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";

            if (parameters.ProjectTypeId != null)
            {
                tempFilter += and;
                tempFilter += "Result.ProjectTypeId = " + parameters.ProjectTypeId;
                and = " AND ";
            }

            if (parameters.ExecutionTypeId != null)
            {
                tempFilter += and;
                tempFilter += "Result.ExecutionTypeId = " + parameters.ExecutionTypeId;
                and = " AND ";
            }

            if (parameters.StartFinantialYearId != null)
            {
                tempFilter += and;
                tempFilter += "Result.StartFinantialYearId = " + parameters.StartFinantialYearId;
                and = " AND ";
            }

            if (parameters.CityId != null)
            {
                tempFilter += and;
                tempFilter += "Result.CityId = " + parameters.CityId;
                and = " AND ";
            }

            if (parameters.ForecastEndFinantialYearId != null)
            {
                tempFilter += and;
                tempFilter += "Result.ForecastEndFinantialYearId = " + parameters.ForecastEndFinantialYearId;
                and = " AND ";
            }

            if (parameters.OwnershipTypeId != null)
            {
                tempFilter += and;
                tempFilter += "Result.OwnershipTypeId = " + parameters.OwnershipTypeId;
                and = " AND ";
            }


            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";

            query += @"Select PDP.ProjectTitle, PDP.FileCode, PDP.ProjectTypeId, BIPT.Title ProjectTypeTitle, PDP.ExecutionTypeId,
                       	   BIET.Title ExecutionTypeTitle, PDP.CityId, BIC.Title CityTitle, PDP.StartFinantialYearId,
                       	   BISFY.Title StartFinantialYearTitle, PDP.EstimateCost, PDP.ForecastEndFinantialYearId, BIFEFY.Title ForecastEndFinantialYearTitle,
                       	   PDP.OwnershipTypeId, BIOT.Title OwnershipTypeTitle, PDP.Area, PDP.CloseArea, PDP.OpenArea, PDP.[Address]
                       From ProjectDefine.Projects PDP
                       		 inner join BaseInformation.ProjectTypes  BIPT on BIPT.Id = PDP.ProjectTypeId
                       		 left join BaseInformation.ExecutionTypes BIET on BIET.Id = PDP.ExecutionTypeId
                       		 inner join BaseInformation.Cities BIC on BIC.Id = PDP.CityId
                       		 left join BaseInformation.FinantialYears BISFY on BISFY.Id = PDP.StartFinantialYearId
                       		 left join BaseInformation.FinantialYears BIFEFY on BIFEFY.Id = PDP.ForecastEndFinantialYearId
                       		 left join BaseInformation.OwnershipTypes BIOT on BIOT.Id = PDP.OwnershipTypeId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectViewModel>(allField + query)
                .Select(Result => new ReportProjectViewModel()
                {
                    ProjectTitle = Result.ProjectTitle,
                    FileCode = Result.FileCode,
                    ProjectTypeId = Result.ProjectTypeId,
                    ProjectTypeTitle = Result.ProjectTypeTitle,
                    ExecutionTypeId = Result.ExecutionTypeId,
                    ExecutionTypeTitle = Result.ExecutionTypeTitle,
                    CityId = Result.CityId,
                    CityTitle = Result.CityTitle,
                    StartFinantialYearId = Result.StartFinantialYearId,
                    StartFinantialYearTitle = Result.StartFinantialYearTitle,
                    EstimateCost = Result.EstimateCost,
                    ForecastEndFinantialYearId = Result.ForecastEndFinantialYearId,
                    ForecastEndFinantialYearTitle = Result.ForecastEndFinantialYearTitle,
                    OwnershipTypeId = Result.OwnershipTypeId,
                    OwnershipTypeTitle = Result.OwnershipTypeTitle,
                    Area = Result.Area,
                    CloseArea = Result.CloseArea,
                    OpenArea = Result.OpenArea,
                    Address = Result.Address
                });

            return result;
        }
    }
}
