using DomainModels.Entities;
using DomainModels.Entities.General;
using System;
using System.Collections.Generic;
using ViewModels.General.PermissionViewModel;

namespace Services.Interfaces.General
{
    public interface IPermissionService : IGenericService<Permission>
    {
        string FillTreeChecked(Int64 RoleId);

        string FillTree(Int64 ParentId, Int64 RoleId);

        string GetPagePermissions(string Url, Int64 RoleId);
    }
}