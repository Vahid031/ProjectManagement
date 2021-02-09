using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionRecoupmentService : GenericService<ProjectSectionRecoupment>, IProjectSectionRecoupmentService
    {
        public ProjectSectionRecoupmentService(IUnitOfWork uow)
            : base(uow)
        {}

        public IEnumerable<ListProjectSectionRecoupmentViewModel> GetAll(ListProjectSectionRecoupmentViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionRecoupmentViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionRecoupment>()
                .Join(_uow.Set<RecoupmentType>(), ProjectSectionRecoupment => ProjectSectionRecoupment.RecoupmentTypeId, RecoupmentType => RecoupmentType.Id, (ProjectSectionRecoupment, RecoupmentType) => new { ProjectSectionRecoupment, RecoupmentType });
                

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionRecoupmentViewModel()
            {
                ProjectSectionRecoupment = new ProjectSectionRecoupment() { 
                    Id = Result.ProjectSectionRecoupment.Id,
                    BranchExporter = Result.ProjectSectionRecoupment.BranchExporter, 
                    Date_ = ConvertDate.GetShamsi(Result.ProjectSectionRecoupment.Date) ,  
                    Number = Result.ProjectSectionRecoupment.Number,
                    },
                RecoupmentType = new RecoupmentType() { Id = Result.RecoupmentType.Id, Title = Result.RecoupmentType.Title }
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionRecoupmentViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            if (cpsivm.ProjectSectionRecoupment.Date_ != null)
            {
                cpsivm.ProjectSectionRecoupment.Date = ConvertDate.GetMiladi(cpsivm.ProjectSectionRecoupment.Date_).Value;
                cpsivm.ProjectSectionRecoupment.Date_ = null;
            }

            ProjectSectionRecoupmentService _ProjectSectionRecoupmentService = new ProjectSectionRecoupmentService(_uow);
            ProjectSectionRecoupmentFileService _ProjectSectionRecoupmentFileService = new ProjectSectionRecoupmentFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionRecoupment.Id;

            if (isInsert == -1)
            {
                _ProjectSectionRecoupmentService.Insert(cpsivm.ProjectSectionRecoupment);
            }
            else
            {
                _ProjectSectionRecoupmentService.Update(cpsivm.ProjectSectionRecoupment);

                foreach (var item in _ProjectSectionRecoupmentFileService.Get(i => i.ProjectSectionRecoupmentId == cpsivm.ProjectSectionRecoupment.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionRecoupmentFileService.Delete(item);
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
                            ProjectSectionRecoupmentFile ProjectSectionRecoupmentFile = new ProjectSectionRecoupmentFile();
                            ProjectSectionRecoupmentFile.ProjectSectionRecoupmentId = cpsivm.ProjectSectionRecoupment.Id;
                            ProjectSectionRecoupmentFile.FileId = fileId;

                            _ProjectSectionRecoupmentFileService.Insert(ProjectSectionRecoupmentFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionRecoupment.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

        public IEnumerable<ReportProjectSectionRecoupmentViewModel> GetAllReport(ReportParameterProjectSectionRecoupmentViewModel parameters)
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

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDR.RecoupmentTypeId, BIRT.Title RecoupmentTypeTitle,
                       	   PDR.Number, PDR.[Date], PDR.BranchExporter
                       From ProjectDevision.ProjectSectionRecoupments PDR
                       	   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDR.ProjectSectionId
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       	   inner join BaseInformation.RecoupmentTypes BIRT on BIRT.Id = PDR.RecoupmentTypeId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionRecoupmentViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionRecoupmentViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionRecoupmentViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    RecoupmentTypeId = Result.RecoupmentTypeId,
                    RecoupmentTypeTitle = Result.RecoupmentTypeTitle,
                    Number = Result.Number,
                    Date = Result.Date,
                    BranchExporter = Result.BranchExporter
                });

            return result;
        }
    }
}
