using DomainModels.Entities.General;
using System;
using System.Collections.Generic;
using ViewModels.General.RoleViewModel;

namespace Services.Interfaces.General
{
    public interface IRoleService : IGenericService<Role>
    {
        string FillTree(string roleCode);

        string FillTreeRadio();

        Int64 SaveRolePermission(CreateRoleViewModel createRole, Int64 MemberId);

        Int64 DeleteRolePermission(Int64 RoleId, Int64 MemberId);
    }
}