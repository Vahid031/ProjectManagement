using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionMadeh48ViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionMadeh48Service : GenericService<ProjectSectionMadeh48>, IProjectSectionMadeh48Service
    {
        public ProjectSectionMadeh48Service(IUnitOfWork uow)
            : base(uow)
        {

        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionMadeh48ViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionMadeh48Service _ProjectSectionMadeh48Service = new ProjectSectionMadeh48Service(_uow);
            ProjectSectionMadeh48FileService _ProjectSectionMadeh48FileService = new ProjectSectionMadeh48FileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionMadeh48.Id;

            if (isInsert == -1)
            {
                _ProjectSectionMadeh48Service.Insert(cpsivm.ProjectSectionMadeh48);
            }
            else
            {
                _ProjectSectionMadeh48Service.Update(cpsivm.ProjectSectionMadeh48);

                foreach (var item in _ProjectSectionMadeh48FileService.Get(i => i.ProjectSectionMadeh48Id == cpsivm.ProjectSectionMadeh48.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionMadeh48FileService.Delete(item);
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
                            ProjectSectionMadeh48File ProjectSectionMadeh48File = new ProjectSectionMadeh48File();
                            ProjectSectionMadeh48File.ProjectSectionMadeh48Id = cpsivm.ProjectSectionMadeh48.Id;
                            ProjectSectionMadeh48File.FileId = fileId;

                            _ProjectSectionMadeh48FileService.Insert(ProjectSectionMadeh48File);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionMadeh48.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }
        public IEnumerable<ReportProjectSectionMadeh48ViewModel> GetAllReport(ReportParameterProjectSectionMadeh48ViewModel parameters)
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

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPM48.LetterNumber, PDPM48.LetterDate, PDPM48.ConfirmPrice
                       From ProjectDevision.ProjectSectionMadeh48s PDPM48
                       	   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPM48.ProjectSectionId
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionMadeh48ViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionMadeh48ViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionMadeh48ViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    LetterNumber = Result.LetterNumber,
                    LetterDate = Result.LetterDate,
                    ConfirmPrice = Result.ConfirmPrice
                });

            return result;
        }
    }
}
