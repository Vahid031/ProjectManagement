using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionDeliveryFixdViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionDeliveryFixdService : GenericService<ProjectSectionDeliveryFixd>, IProjectSectionDeliveryFixdService
    {
        public ProjectSectionDeliveryFixdService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionDeliveryFixdViewModel> GetAll(ListProjectSectionDeliveryFixdViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionDeliveryFixdViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionDeliveryFixd>()
                .Join(_uow.Set<Opinion>(), ProjectSectionDeliveryFixd => ProjectSectionDeliveryFixd.CommisionOpinionId, Opinion => Opinion.Id, (ProjectSectionDeliveryFixd, Opinion) => new { ProjectSectionDeliveryFixd, Opinion })
                .Join(_uow.Set<Defect>(), ProjectSectionDeliveryFixd => ProjectSectionDeliveryFixd.ProjectSectionDeliveryFixd.DefectId, Defect => Defect.Id, (ProjectSectionDeliveryFixd, Defect) => new { ProjectSectionDeliveryFixd.ProjectSectionDeliveryFixd, ProjectSectionDeliveryFixd.Opinion, Defect });

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionDeliveryFixdViewModel()
            {
                ProjectSectionDeliveryFixd = new ProjectSectionDeliveryFixd() { Id = Result.ProjectSectionDeliveryFixd.Id, Description = Result.ProjectSectionDeliveryFixd.Description, ProjectSectionId = Result.ProjectSectionDeliveryFixd.ProjectSectionId, CommisionOpinionId = Result.ProjectSectionDeliveryFixd.CommisionOpinionId, ConveneDate = Result.ProjectSectionDeliveryFixd.ConveneDate, ConveneNumber = Result.ProjectSectionDeliveryFixd.ConveneNumber, DefectId = Result.ProjectSectionDeliveryFixd.DefectId, DefectDetails = Result.ProjectSectionDeliveryFixd.DefectDetails, MeetingDate = Result.ProjectSectionDeliveryFixd.MeetingDate, RequestDate = Result.ProjectSectionDeliveryFixd.RequestDate, RequestNumber = Result.ProjectSectionDeliveryFixd.RequestNumber },
                Defect = new Defect() { Id = Result.Defect.Id, Title = Result.Defect.Title },
                Opinion = new Opinion() { Id = Result.Opinion.Id, Title = Result.Opinion.Title },
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionDeliveryFixdViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionDeliveryFixdService _ProjectSectionDeliveryFixdService = new ProjectSectionDeliveryFixdService(_uow);
            ProjectSectionDeliveryFixdFileService _ProjectSectionDeliveryFixdFileService = new ProjectSectionDeliveryFixdFileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionDeliveryFixd.Id;

            if (isInsert == -1)
            {
                _ProjectSectionDeliveryFixdService.Insert(cpsivm.ProjectSectionDeliveryFixd);
            }
            else
            {
                _ProjectSectionDeliveryFixdService.Update(cpsivm.ProjectSectionDeliveryFixd);

                foreach (var item in _ProjectSectionDeliveryFixdFileService.Get(i => i.ProjectSectionDeliveryFixdId == cpsivm.ProjectSectionDeliveryFixd.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionDeliveryFixdFileService.Delete(item);
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
                            ProjectSectionDeliveryFixdFile ProjectSectionDeliveryFixdFile = new ProjectSectionDeliveryFixdFile();
                            ProjectSectionDeliveryFixdFile.ProjectSectionDeliveryFixdId = cpsivm.ProjectSectionDeliveryFixd.Id;
                            ProjectSectionDeliveryFixdFile.FileId = fileId;

                            _ProjectSectionDeliveryFixdFileService.Insert(ProjectSectionDeliveryFixdFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionDeliveryFixd.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

        public IEnumerable<ReportProjectSectionDeliveryFixdViewModel> GetAllReport(ReportParameterProjectSectionDeliveryFixdViewModel parameters)
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

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPSDF.RequestNumber, PDPSDF.RequestDate, PDPSDF.ConveneNumber,
                       	   PDPSDF.ConveneDate, PDPSDF.MeetingDate, PDPSDF.DefectDetails, PDPSDF.CommisionOpinionId,
                       	   BIO.Title CommisionOpinionTitle, PDPSDF.[Description]
                       From ProjectDevision.ProjectSectionDeliveryFixdes PDPSDF
                       	   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPSDF.ProjectSectionId
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       	   inner join BaseInformation.Opinions BIO on BIO.Id = PDPSDF.CommisionOpinionId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionDeliveryFixdViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionDeliveryFixdViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionDeliveryFixdViewModel()
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
                    CommisionOpinionId = Result.CommisionOpinionId,
                    CommisionOpinionTitle = Result.CommisionOpinionTitle,
                    Description = Result.Description
                });

            return result;
        }
    }
}
