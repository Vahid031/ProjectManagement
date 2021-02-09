using DomainModels.Entities.General;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.General.MemberViewModel;

namespace Services.Interfaces.General
{
    public interface IMemberService : IGenericService<Member>
    {
        IEnumerable<ListMemberViewModel> GetAll(ListMemberViewModel memberViewModel, ref Paging pg, string roleCode);

        Member GetMemberById(Int64 Id);

        Member AuthenticateUser(string UserName, string Password);

        bool IsValid(string Url, Int64 UserId, Int64 RoleId);
    }
}