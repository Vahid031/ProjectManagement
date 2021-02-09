using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.BaseInformation;
using ViewModels.ProjectDefine.ProjectAllocateViewModel;
using DomainModels.Entities.ProjectDefine;
using Services.Interfaces.ProjectDefine;
using Services.EfServices.General;
using DomainModels.Enums;
using Services.EfServices.BaseInformation;

namespace Services.EfServices.ProjectDefine
{
    public class ProjectAllocateService : GenericService<ProjectAllocate>, IProjectAllocateService
    {
        public ProjectAllocateService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectAllocateViewModel> GetAll(ListProjectAllocateViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectAllocate.", "");
            DynamicFiltering.GetFilter<ListProjectAllocateViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectAllocate.", "");

            var a = _uow.Set<ProjectAllocate>().AsQueryable<ProjectAllocate>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectAllocateViewModel()
            {
                ProjectAllocate = new ProjectAllocate() { Id = Result.Id, AllocateAmount = Result.AllocateAmount, AllocateDate = Result.AllocateDate, CreditAmount = Result.CreditAmount, CreditPercent = Result.CreditPercent, ProjectId = Result.ProjectId }
            });
        }

        public KeyValuePair<Int64, string> Save(CreateProjectAllocateViewModel cpavm, Int64 memberId, List<string> paths)
        {
            ProjectAllocateService _projectAllocateService = new ProjectAllocateService(_uow);
            ProjectAllocateFileService _ProjectAllocateFileService = new ProjectAllocateFileService(_uow);
            
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpavm.ProjectAllocate.Id;

            if (isInsert == -1)
            {
                _projectAllocateService.Insert(cpavm.ProjectAllocate);
            }
            else
            {
                _projectAllocateService.Update(cpavm.ProjectAllocate);

                foreach (var item in _ProjectAllocateFileService.Get(i => i.ProjectAllocateId == cpavm.ProjectAllocate.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectAllocateFileService.Delete(item);
                    }
                }
            }


            if (cpavm.Files != null)
            {
                foreach (string item in cpavm.Files.Split(','))
                {
                    if (item != "")
                    {
                        Int64 fileId = Convert.ToInt64(item);
                        if (_fileService.Get(i => i.Id == fileId).First().FileState.Value == 1)
                        {
                            ProjectAllocateFile ProjectAllocateFile = new ProjectAllocateFile();
                            ProjectAllocateFile.ProjectAllocateId = cpavm.ProjectAllocate.Id;
                            ProjectAllocateFile.FileId = fileId;

                            _ProjectAllocateFileService.Insert(ProjectAllocateFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(memberId) > 0)
            {
                ProjectService _projectService = new ProjectService(_uow);

                var project = _projectService.Get(c => c.Id == cpavm.ProjectAllocate.ProjectId).FirstOrDefault();
                project.ProjectStateId = Convert.ToInt64(GeneralEnums.ProjectState.WaitingDivisionProject);
                _projectService.Update(project);
                _uow.SaveChanges(memberId);

                return new KeyValuePair<long, string>(cpavm.ProjectAllocate.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

        public IEnumerable<ReportProjectAllocateViewModel> GetAllReport(ReportParameterProjectAllocateViewModel parameters)
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

            if (parameters.CityId != null)
            {
                tempFilter += and;
                tempFilter += "Result.CityId = " + parameters.CityId;
                and = " AND ";
            }

            if (parameters.ProjectTypeId != null)
            {
                tempFilter += and;
                tempFilter += "Result.ProjectTypeId = " + parameters.ProjectTypeId;
                and = " AND ";
            }


            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDP.ProjectStateId, PDPS.Title, PDPA.AllocateAmount, PDPA.AllocateDate, 
                       	   PDP.StartFinantialYearId, BIFY.Title StartFinantialYearTitle,PDP.CityId,PDP.ProjectTypeId
                       From ProjectDefine.ProjectAllocates PDPA
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPA.ProjectId
                       	   left join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPA.ProjectSectionId 
                       	   left join BaseInformation.FinantialYears BIFY on BIFY.Id = PDP.StartFinantialYearId 
                       ) Result"
            + filter;

            IEnumerable<ReportProjectAllocateViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectAllocateViewModel>(allField + query)
                .Select(Result => new ReportProjectAllocateViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    ProjectStateId = Result.ProjectStateId,
                    Title = Result.Title,
                    AllocateAmount = Result.AllocateAmount,
                    AllocateDate = Result.AllocateDate,
                    StartFinantialYearId = Result.StartFinantialYearId,
                    StartFinantialYearTitle = Result.StartFinantialYearTitle
                });
            var test = result.ToList();
            return result;
        }
    }
}
