﻿using DatabaseContext.Context;
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
using ViewModels.BaseInformation.AccountTypeViewModel;

namespace Services.EfServices.BaseInformation
{
    public class AccountTypeService : GenericService<AccountType>, IAccountTypeService
    {
        public AccountTypeService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListAccountTypeViewModel> GetAll(ListAccountTypeViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("AccountType.", "");
            DynamicFiltering.GetFilter<ListAccountTypeViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("AccountType.", "");

            var a = _uow.Set<AccountType>().AsQueryable<AccountType>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListAccountTypeViewModel()
            {
                AccountType = new AccountType() { Id = Result.Id, Title = Result.Title }
            });
        }

    }
}
