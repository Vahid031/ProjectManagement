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
using ViewModels.BaseInformation.ContractTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class ContractTypeService : GenericService<ContractType>, IContractTypeService
    {
        public ContractTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListContractTypeViewModel> GetAll(ListContractTypeViewModel lctvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ContractType.", "");
            DynamicFiltering.GetFilter<ListContractTypeViewModel>(lctvm, ref pg);
            pg._filter = pg._filter.Replace("ContractType.", "");

            var a = _uow.Set<ContractType>().AsQueryable<ContractType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListContractTypeViewModel()
            {
                ContractType = new ContractType() { Id = Result.Id, Title = Result.Title }
            });
        }
    }
}
