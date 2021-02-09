using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionMadeh46ViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionMadeh46Service : GenericService<ProjectSectionMadeh46>, IProjectSectionMadeh46Service
    {
        public ProjectSectionMadeh46Service(IUnitOfWork uow)
            : base(uow)
        {

        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionMadeh46ViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionMadeh46Service _ProjectSectionMadeh46Service = new ProjectSectionMadeh46Service(_uow);
            ProjectSectionMadeh46FileService _ProjectSectionMadeh46FileService = new ProjectSectionMadeh46FileService(_uow);
            ProjectSectionService _projectSectionService = new ProjectSectionService(_uow);
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionMadeh46.Id;

            if (isInsert == -1)
            {
                _ProjectSectionMadeh46Service.Insert(cpsivm.ProjectSectionMadeh46);
            }
            else
            {
                _ProjectSectionMadeh46Service.Update(cpsivm.ProjectSectionMadeh46);

                foreach (var item in _ProjectSectionMadeh46FileService.Get(i => i.ProjectSectionMadeh46Id == cpsivm.ProjectSectionMadeh46.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionMadeh46FileService.Delete(item);
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
                            ProjectSectionMadeh46File ProjectSectionMadeh46File = new ProjectSectionMadeh46File();
                            ProjectSectionMadeh46File.ProjectSectionMadeh46Id = cpsivm.ProjectSectionMadeh46.Id;
                            ProjectSectionMadeh46File.FileId = fileId;

                            _ProjectSectionMadeh46FileService.Insert(ProjectSectionMadeh46File);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionMadeh46.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }


        public IEnumerable<ReportProjectSectionMadeh46ViewModel> GetAllReport(ReportParameterProjectSectionMadeh46ViewModel parameters)
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

            query += @"Select PDP.FileCode, PDP.ProjectTitle, PDPS.Title, PDPM46.LetterNumber, PDPM46.LicenseNumber, PDPM46.LetterDate,
                       	   PDPM46.ForfeitPrice, PDPM46.ConfirmPrice
                       From ProjectDevision.ProjectSectionMadeh46s PDPM46
                       	   inner join ProjectDevision.ProjectSections PDPS on PDPS.Id = PDPM46.ProjectSectionId
                       	   inner join ProjectDefine.Projects PDP on PDP.Id = PDPS.ProjectId
                       ) Result"
            + filter;

            IEnumerable<ReportProjectSectionMadeh46ViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportProjectSectionMadeh46ViewModel>(allField + query)
                .Select(Result => new ReportProjectSectionMadeh46ViewModel()
                {
                    FileCode = Result.FileCode,
                    ProjectTitle = Result.ProjectTitle,
                    Title = Result.Title,
                    LetterNumber = Result.LetterNumber,
                    LicenseNumber = Result.LicenseNumber,
                    LetterDate = Result.LetterDate,
                    ForfeitPrice = Result.ForfeitPrice,
                    ConfirmPrice = Result.ConfirmPrice
                });

            return result;
        }

    }
}
