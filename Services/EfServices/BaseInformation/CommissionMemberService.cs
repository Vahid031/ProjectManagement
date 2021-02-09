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
using ViewModels.BaseInformation.CommissionMemberViewModel;

namespace Services.EfServices.BaseInformation
{
    public class CommissionMemberService : GenericService<CommissionMember>, ICommissionMemberService
    {
        public CommissionMemberService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListCommissionMemberViewModel> GetAll(ListCommissionMemberViewModel lcmvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("CommissionMember.", "");
            DynamicFiltering.GetFilter<ListCommissionMemberViewModel>(lcmvm, ref pg);
            pg._filter = pg._filter.Replace("CommissionMember.", "");

            var a = _uow.Set<CommissionMember>().AsQueryable<CommissionMember>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListCommissionMemberViewModel()
            {
                CommissionMember = new CommissionMember() { Id = Result.Id, FirstName = Result.FirstName, LastName = Result.LastName, Post = Result.Post, MobileNumber = Result.MobileNumber, IsActive = Result.IsActive }
            });
        }

    }
}
