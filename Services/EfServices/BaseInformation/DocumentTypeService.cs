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
using ViewModels.BaseInformation.DocumentTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class DocumentTypeService : GenericService<DocumentType>, IDocumentTypeService
    {
        public DocumentTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }


        public IEnumerable<ListDocumentTypeViewModel> GetAll(ListDocumentTypeViewModel ldtvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("DocumentType.", "");
            DynamicFiltering.GetFilter<ListDocumentTypeViewModel>(ldtvm, ref pg);
            pg._filter = pg._filter.Replace("DocumentType.", "");

            var a = _uow.Set<DocumentType>().AsQueryable<DocumentType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListDocumentTypeViewModel()
            {
                DocumentType = new DocumentType() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
