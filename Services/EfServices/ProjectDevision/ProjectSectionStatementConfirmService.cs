using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.ProjectDevision.ProjectSectionStatementConfirmViewModel;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using Services.EfServices.General;
using DomainModels.Enums;
using DomainModels.Entities.BaseInformation;
using ViewModels.ProjectDevision.ProjectSectionDraftViewModel;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionStatementConfirmService : GenericService<ProjectSectionStatementConfirm>, IProjectSectionStatementConfirmService
    {
        public ProjectSectionStatementConfirmService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionStatementConfirmViewModel> GetAll(ListProjectSectionStatementConfirmViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionStatementConfirmViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionStatementConfirm>()
                .Join(_uow.Set<Opinion>(), ProjectSectionStatementConfirm => ProjectSectionStatementConfirm.FinalOpinionId, Opinion => Opinion.Id, (ProjectSectionStatementConfirm, Opinion) => new { ProjectSectionStatementConfirm, Opinion });
            var test = a.ToList();
            
            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionStatementConfirmViewModel()
            {
                ProjectSectionStatementConfirm = new ProjectSectionStatementConfirm() { Id = Result.ProjectSectionStatementConfirm.Id, Price = Result.ProjectSectionStatementConfirm.Price, Description = Result.ProjectSectionStatementConfirm.Description },
                Opinion = new Opinion() { Id = Result.Opinion.Id, Title = Result.Opinion.Title }
            });
        }



        public IEnumerable<ListProjectSectionDraftListViewModel> GetConfirmDraft(ListProjectSectionDraftListViewModel lpscvm, ref Paging pg)
        {
            if (lpscvm.ConfirmDate_ != null)
            {
                lpscvm.ConfirmDate = ConvertDate.GetMiladi(lpscvm.ConfirmDate_);
                lpscvm.ConfirmDate_ = null;
            }

            string filter = GetSqlFilterDynamic(lpscvm, "ProjectSectionDraftList", " WHERE ");

            string[] orderby = pg._orderColumn.Split(' ');
            orderby[1] = orderby[1] == "descending" ? "DESC" : "ASC";

            string query =
@"
SELECT * FROM
(
SELECT 
	ProjectSectionDraftList.Id,
	ProjectSectionDraftList.ProjectSectionId,
    ProjectSectionDraftList.ProjectSectionStatementId,
	BaseInformation.StatementNumbers.Title AS 'StatementNumberTitle',
	ProjectSectionDraftList.Price AS 'ConfirmPrice',
    ProjectDevision.ProjectSectionStatements.ConfirmPrice AS 'PriceStatement',
	ProjectDevision.ProjectSectionStatements.ConfirmPrice - ProjectSectionDraftList.Price AS 'AccountBalance'
FROM ProjectDevision.ProjectSectionStatementConfirms As ProjectSectionDraftList
LEFT OUTER JOIN ProjectDevision.ProjectSectionStatements
	ON ProjectSectionDraftList.ProjectSectionStatementId = ProjectDevision.ProjectSectionStatements.Id
LEFT OUTER JOIN BaseInformation.StatementNumbers
	ON  ProjectDevision.ProjectSectionStatements.StatementNumberId = BaseInformation.StatementNumbers.Id
) AS ProjectSectionDraftList
"
+ filter;
            string orderQuery = @"
ORDER BY " + orderby[0] + " " + orderby[1] + @"
OFFSET " + (pg._pageNumber - 1) * pg._pageSize + @" ROWS
FETCH NEXT " + pg._pageSize + " ROWS ONLY;";

            string s = query + orderQuery;
            IEnumerable<ListProjectSectionDraftListViewModel> GetList = _uow.GetInstance().Database.SqlQuery<ListProjectSectionDraftListViewModel>(query + orderQuery)
            .Select(Result => new ListProjectSectionDraftListViewModel()
            {
                Id = Result.Id,
                ProjectSectionStatementId = Result.ProjectSectionStatementId,
                StatementNumberTitle = Result.StatementNumberTitle,
                ConfirmPrice = Result.ConfirmPrice,
                PriceStatement = Result.PriceStatement,
                AccountBalance = ConfirmedPrice(Result.Id, Result.ConfirmPrice),
                ConfirmDate = Result.ConfirmDate
            });

            string queryCount = @"
SELECT CAST(COUNT(*) AS BIGINT) AS [Count]
FROM ProjectDevision.ProjectSectionStatementConfirms As ProjectSectionDraftList
LEFT OUTER JOIN ProjectDevision.ProjectSectionStatements
	ON ProjectSectionDraftList.ProjectSectionStatementId = ProjectDevision.ProjectSectionStatements.Id
LEFT OUTER JOIN BaseInformation.StatementNumbers
	ON  ProjectDevision.ProjectSectionStatements.StatementNumberId = BaseInformation.StatementNumbers.Id
 "
+ filter;

            var count = _uow.GetInstance().Database.SqlQuery<long>(queryCount).FirstOrDefault();
            pg._rowCount = Convert.ToInt64(count);

            return GetList;
        }

        public KeyValuePair<Int64, string> Save(CreateProjectSectionStatementConfirmViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionStatementConfirmFileService _ProjectSectionStatementConfirmFileService = new ProjectSectionStatementConfirmFileService(_uow);

            FileService _fileService = new FileService(_uow);

            foreach (var item in _ProjectSectionStatementConfirmFileService.Get(i => i.ProjectSectionStatementConfirmId == cpsivm.ProjectSectionStatementConfirm.Id).ToList())
            {
                var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                if (file != null)
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _ProjectSectionStatementConfirmFileService.Delete(item);
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
                            ProjectSectionStatementConfirmFile ProjectSectionStatementConfirmFile = new ProjectSectionStatementConfirmFile();
                            ProjectSectionStatementConfirmFile.ProjectSectionStatementConfirmId = cpsivm.ProjectSectionStatementConfirm.Id;
                            ProjectSectionStatementConfirmFile.FileId = fileId;

                            _ProjectSectionStatementConfirmFileService.Insert(ProjectSectionStatementConfirmFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionStatementConfirm.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }


        //-------------------------------------
        // GetSqlFilterDynamic
        public string GetSqlFilterDynamic<TEntity>(TEntity entity, string className, string firstFilter)
        {
            string filter = firstFilter;
            string and = "";

            foreach (var item in entity.GetType().GetProperties())
            {
                object itemValue = item.GetValue(entity, null);

                if (itemValue == null || item.Name == "Id")
                    continue;

                if (item.PropertyType == typeof(Nullable<DateTime>))
                    continue;

                if (itemValue != null && item.Name == "Id" && Convert.ToInt64(itemValue) == 0)
                    continue;

                if (item.Name == "Title" || item.Name.EndsWith("Title"))
                {
                    filter += className + "." + item.Name + " LIKE N'%" + itemValue + "%'";
                    and = " AND ";
                    continue;
                }

                if (Convert.ToInt64(itemValue) != 0)
                {
                    filter += and;
                    filter += className + "." + item.Name + " = '" + itemValue + "'";
                    and = " AND ";
                }

            }

            if (filter.Length < 10)
                return "";
            else
                return filter;
        }

        private Int64 ConfirmedPrice(long id, Int64 confirmPrice)
        {
            // به دست آوردن جمع کل حواله های تایید شده
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            var otherDraft = db.ProjectSectionDrafts.Where(m => m.ProjectSectionStatementConfirm.Id == id).ToList();


            // مبلغ آخرین صورت وضعیت تایید شده
            Int64 value = confirmPrice;
            foreach (var item in otherDraft)
                value -= (Int64)item.TempDraftPrice;

            return value;
        }


    }
}
