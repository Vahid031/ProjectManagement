using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionFinalParticipantViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionFinalParticipantService : GenericService<ProjectSectionFinalParticipant>, IProjectSectionFinalParticipantService
    {
        public ProjectSectionFinalParticipantService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionFinalParticipantViewModel> GetAll(ListProjectSectionFinalParticipantViewModel latvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionFinalParticipantViewModel>(latvm, ref pg);

            var a =
            _uow.Set<ProjectSectionFinalParticipant>()
                .Join(_uow.Set<Contractor>(), ProjectSectionFinalParticipant => ProjectSectionFinalParticipant.ContractorId, Contractor => Contractor.Id, (ProjectSectionFinalParticipant, Contractor) => new { ProjectSectionFinalParticipant, Contractor });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionFinalParticipantViewModel()
            {
                ProjectSectionFinalParticipant = new ProjectSectionFinalParticipant() { Id = Result.ProjectSectionFinalParticipant.Id, ContractorId = Result.ProjectSectionFinalParticipant.ContractorId, ProjectSectionId = Result.ProjectSectionFinalParticipant.ProjectSectionId, SuggestPrice = Result.ProjectSectionFinalParticipant.SuggestPrice, WarrantyDate = Result.ProjectSectionFinalParticipant.WarrantyDate, WarrantyNumber = Result.ProjectSectionFinalParticipant.WarrantyNumber },
                Contractor = new Contractor() { Id = Result.Contractor.Id, Address = Result.Contractor.Address, AgentFirstName = Result.Contractor.AgentFirstName, AgentLastName = Result.Contractor.AgentLastName, AgentMobileNumber = Result.Contractor.AgentMobileNumber, CompanyName = Result.Contractor.CompanyName, Email = Result.Contractor.Email, FaxNumber = Result.Contractor.FaxNumber, IsActive = Result.Contractor.IsActive, ManagerFirstName = Result.Contractor.ManagerFirstName, ManagerLastName = Result.Contractor.ManagerLastName, ManagerMobileNumber = Result.Contractor.ManagerMobileNumber, ManagerNationalCode = Result.Contractor.ManagerNationalCode, PhoneNumber = Result.Contractor.PhoneNumber, Website = Result.Contractor.Website }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionFinalParticipantViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionFinalParticipantService _ProjectSectionFinalParticipantService = new ProjectSectionFinalParticipantService(_uow);
            ProjectSectionFinalParticipantFileService _ProjectSectionFinalParticipantFileService = new ProjectSectionFinalParticipantFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionFinalParticipant.Id;

            if (isInsert == -1)
            {
                if (!_ProjectSectionFinalParticipantService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionFinalParticipant.ProjectSectionId.Value).Any())
                {
                    var projectSection = _projectSectionService.Get(i => i.Id == cpsivm.ProjectSectionFinalParticipant.ProjectSectionId.Value).FirstOrDefault();
                    projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionOpenDate);
                }

                _ProjectSectionFinalParticipantService.Insert(cpsivm.ProjectSectionFinalParticipant);
            }
            else
            {
                _ProjectSectionFinalParticipantService.Update(cpsivm.ProjectSectionFinalParticipant);

                foreach (var item in _ProjectSectionFinalParticipantFileService.Get(i => i.ProjectSectionFinalParticipantId == cpsivm.ProjectSectionFinalParticipant.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionFinalParticipantFileService.Delete(item);
                    }
                }
            }

            if (cpsivm.Files != null)
            {
                foreach (string item in cpsivm.Files.Split(','))
                {
                    if (item != "")
                    {
                        Int64 fileId = Convert.ToInt64(item);
                        if (_fileService.Get(i => i.Id == fileId).First().FileState.Value == 1)
                        {
                            ProjectSectionFinalParticipantFile ProjectSectionFinalParticipantFile = new ProjectSectionFinalParticipantFile();
                            ProjectSectionFinalParticipantFile.ProjectSectionFinalParticipantId = cpsivm.ProjectSectionFinalParticipant.Id;
                            ProjectSectionFinalParticipantFile.FileId = fileId;

                            _ProjectSectionFinalParticipantFileService.Insert(ProjectSectionFinalParticipantFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionFinalParticipant.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }
        public IEnumerable<ReportProjectSectionFinalParticipantViewModel> GetAllReport(ReportParameterProjectSectionFinalParticipantViewModel parameters)
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

            if (parameters.Title != null)
            {
                tempFilter += and;
                tempFilter += "Result.Title Like N'%" + parameters.Title + "%'";
                and = " AND ";
            }


            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";

            query += @"	Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPSFP.WarrantyDate, BIC.CompanyName, PDPSFP.SuggestPrice
                        	From ProjectDevision.ProjectSectionFinalParticipants PDPSFP
                        		   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPSFP.ProjectSectionId
                        		   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                        		   inner join BaseInformation.Contractors BIC on BIC.Id = PDPSFP.ContractorId 
                        
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionFinalParticipantViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionFinalParticipantViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionFinalParticipantViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    WarrantyDate = Result.WarrantyDate,
                    CompanyName = Result.CompanyName,
                    SuggestPrice = Result.SuggestPrice
                });

            return result;
        }
    }
}
