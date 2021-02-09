using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionDeliveryTemproryViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionDeliveryTemproryService : GenericService<ProjectSectionDeliveryTemprory>, IProjectSectionDeliveryTemproryService
    {
        public ProjectSectionDeliveryTemproryService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionDeliveryTemproryViewModel> GetAll(ListProjectSectionDeliveryTemproryViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionDeliveryTemproryViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionDeliveryTemprory>()
                .Join(_uow.Set<Opinion>(), ProjectSectionDeliveryTemprory => ProjectSectionDeliveryTemprory.CommisionOpinionId, Opinion => Opinion.Id, (ProjectSectionDeliveryTemprory, Opinion) => new { ProjectSectionDeliveryTemprory, Opinion })
                .Join(_uow.Set<Defect>(), ProjectSectionDeliveryTemprory => ProjectSectionDeliveryTemprory.ProjectSectionDeliveryTemprory.DefectId, Defect => Defect.Id, (ProjectSectionDeliveryTemprory, Defect) => new { ProjectSectionDeliveryTemprory.ProjectSectionDeliveryTemprory, ProjectSectionDeliveryTemprory.Opinion, Defect });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionDeliveryTemproryViewModel()
            {
                ProjectSectionDeliveryTemprory = new ProjectSectionDeliveryTemprory() { Id = Result.ProjectSectionDeliveryTemprory.Id, Description = Result.ProjectSectionDeliveryTemprory.Description, ProjectSectionId = Result.ProjectSectionDeliveryTemprory.ProjectSectionId, CommisionOpinionId = Result.ProjectSectionDeliveryTemprory.CommisionOpinionId, ConveneDate = Result.ProjectSectionDeliveryTemprory.ConveneDate, ConveneNumber = Result.ProjectSectionDeliveryTemprory.ConveneNumber, DefectId = Result.ProjectSectionDeliveryTemprory.DefectId, DefectDetails = Result.ProjectSectionDeliveryTemprory.DefectDetails, MeetingDate = Result.ProjectSectionDeliveryTemprory.MeetingDate, PaidPrice = Result.ProjectSectionDeliveryTemprory.PaidPrice, RequestDate = Result.ProjectSectionDeliveryTemprory.RequestDate, RequestNumber = Result.ProjectSectionDeliveryTemprory.RequestNumber },
                Defect = new Defect() { Id = Result.Defect.Id, Title = Result.Defect.Title },
                Opinion = new Opinion() { Id = Result.Opinion.Id, Title = Result.Opinion.Title },
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionDeliveryTemproryViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionDeliveryTemproryService _ProjectSectionDeliveryTemproryService = new ProjectSectionDeliveryTemproryService(_uow);
            ProjectSectionDeliveryTemproryFileService _ProjectSectionDeliveryTemproryFileService = new ProjectSectionDeliveryTemproryFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionDeliveryTemprory.Id;

            if (isInsert == -1)
            {
                _ProjectSectionDeliveryTemproryService.Insert(cpsivm.ProjectSectionDeliveryTemprory);
            }
            else
            {
                _ProjectSectionDeliveryTemproryService.Update(cpsivm.ProjectSectionDeliveryTemprory);

                foreach (var item in _ProjectSectionDeliveryTemproryFileService.Get(i => i.ProjectSectionDeliveryTemproryId == cpsivm.ProjectSectionDeliveryTemprory.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionDeliveryTemproryFileService.Delete(item);
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
                            ProjectSectionDeliveryTemproryFile ProjectSectionDeliveryTemproryFile = new ProjectSectionDeliveryTemproryFile();
                            ProjectSectionDeliveryTemproryFile.ProjectSectionDeliveryTemproryId = cpsivm.ProjectSectionDeliveryTemprory.Id;
                            ProjectSectionDeliveryTemproryFile.FileId = fileId;

                            _ProjectSectionDeliveryTemproryFileService.Insert(ProjectSectionDeliveryTemproryFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionDeliveryTemprory.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }
        public IEnumerable<ReportProjectSectionDeliveryTemproryViewModel> GetAllReport(ReportParameterProjectSectionDeliveryTemproryViewModel parameters)
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

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPSDT.RequestNumber, PDPSDT.RequestDate, PDPSDT.ConveneNumber,
                       	   PDPSDT.ConveneDate, PDPSDT.MeetingDate, PDPSDT.DefectDetails, PDPSDT.PaidPrice, PDPSDT.CommisionOpinionId,
                       	   BIO.Title CommisionOpinionTitle, PDPSDT.[Description]
                       From ProjectDevision.ProjectSectionDeliveryTemprories PDPSDT
                       	   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPSDT.ProjectSectionId
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       	   inner join BaseInformation.Opinions BIO on BIO.Id = PDPSDT.CommisionOpinionId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionDeliveryTemproryViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionDeliveryTemproryViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionDeliveryTemproryViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    RequestNumber = Result.RequestNumber,
                    RequestDate = Result.RequestDate,
                    ConveneNumber = Result.ConveneNumber,
                    ConveneDate = Result.ConveneDate,
                    MeetingDate = Result.MeetingDate,
                    DefectDetails = Result.DefectDetails,
                    PaidPrice = Result.PaidPrice,
                    CommisionOpinionId = Result.CommisionOpinionId,
                    CommisionOpinionTitle = Result.CommisionOpinionTitle,
                    Description = Result.Description
                });

            return result;
        }
    }
}
