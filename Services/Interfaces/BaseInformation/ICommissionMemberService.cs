using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.CommissionMemberViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface ICommissionMemberService : IGenericService<CommissionMember>
    {
        IEnumerable<ListCommissionMemberViewModel> GetAll(ListCommissionMemberViewModel lcmvm, ref Paging pg);
    }
}