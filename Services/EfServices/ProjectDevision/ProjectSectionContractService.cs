using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionContractViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionContractService : GenericService<ProjectSectionContract>, IProjectSectionContractService
    {
        public ProjectSectionContractService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public KeyValuePair<Int64, string> Save(CreateProjectSectionContractViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionContractService _ProjectSectionContractService = new ProjectSectionContractService(_uow);
            ProjectSectionContractFileService _ProjectSectionContractFileService = new ProjectSectionContractFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            ProjectSectionContractDetailService _projectSectionContractDetail = new ProjectSectionContractDetailService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionContract.Id;

            if (isInsert == -1)
            {
                if (!_ProjectSectionContractService.Get(i => i.ProjectSectionId == cpsivm.ProjectSectionContract.ProjectSectionId.Value).Any())
                {
                    var projectSection = _projectSectionService.Get(i => i.Id == cpsivm.ProjectSectionContract.ProjectSectionId.Value).FirstOrDefault();
                    projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionFinalContract);
                }

                _ProjectSectionContractService.Insert(cpsivm.ProjectSectionContract);
            }
            else
            {
                _ProjectSectionContractService.Update(cpsivm.ProjectSectionContract);

                foreach (var item in _ProjectSectionContractFileService.Get(i => i.ProjectSectionContractId == cpsivm.ProjectSectionContract.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionContractFileService.Delete(item);
                    }
                }

                foreach (var item in _projectSectionContractDetail.Get(i => i.ProjectSectionContractId == cpsivm.ProjectSectionContract.Id).ToList())
                {
                    _projectSectionContractDetail.Delete(item);
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
                            ProjectSectionContractFile ProjectSectionContractFile = new ProjectSectionContractFile();
                            ProjectSectionContractFile.ProjectSectionContractId = cpsivm.ProjectSectionContract.Id;
                            ProjectSectionContractFile.FileId = fileId;

                            _ProjectSectionContractFileService.Insert(ProjectSectionContractFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (cpsivm.ContractDetails != null)
            {
                foreach (var item in cpsivm.ContractDetails.Split('#'))
                {
                    if (item != "")
                    {
                        string[] innerItem = item.Split('|');
                        ProjectSectionContractDetail pscd = new ProjectSectionContractDetail();
                        pscd.ProjectSectionContractId = cpsivm.ProjectSectionContract.Id;

                        if (!String.IsNullOrEmpty(innerItem[0]))
                            pscd.ProjectDetails = innerItem[0];

                        if (!String.IsNullOrEmpty(innerItem[1]))
                            pscd.TotalPrice = Convert.ToInt64(innerItem[1]);

                        if (!String.IsNullOrEmpty(innerItem[2]))
                            pscd.TotalAmonts = Convert.ToInt64(innerItem[2]);

                        if (innerItem[3] != "-1")
                            pscd.UnitId = Convert.ToInt64(innerItem[3]);

                        if (!String.IsNullOrEmpty(innerItem[4]))
                            pscd.UnitPrice = Convert.ToInt64(innerItem[4]);

                        _projectSectionContractDetail.Insert(pscd);
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionContract.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

        public IEnumerable<ReportProjectSectionContractViewModel> GetAllReport(ReportParameterProjectSectionContractViewModel parameters)
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

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPSC.EndDate, BIC.CompanyName, PDPSC.ContractNumber,
                       	   PDPSC.ContractTypeId, BICT.Title ContractTypeTitle
                       
                       From ProjectDevision.ProjectSectionContracts PDPSC
                       	   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPSC.ProjectSectionId
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       	   inner join ProjectDevision.ProjectSectionWinners PDPSW on (PDPSW.ProjectSectionId = PDPS.Id And (PDPSW.RankId = 1 Or PDPSW.RankId = 4))
                       	   inner join BaseInformation.Contractors BIC on BIC.Id = PDPSW.ContractorId 
                       	   inner join BaseInformation.ContractTypes BICT on BICT.Id = PDPSC.ContractTypeId 
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionContractViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionContractViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionContractViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    EndDate = Result.EndDate,
                    CompanyName = Result.CompanyName,
                    ContractNumber = Result.ContractNumber,
                    ContractTypeId = Result.ContractTypeId,
                    ContractTypeTitle = Result.ContractTypeTitle
                });

            return result;
        }

    }
}
