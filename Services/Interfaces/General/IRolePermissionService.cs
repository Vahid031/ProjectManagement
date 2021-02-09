using DomainModels.Entities;
using DomainModels.Entities.General;
using System;
using System.Collections.Generic;

namespace Services.Interfaces.General
{
    public interface IRolePermissionService : IGenericService<RolePermission>
    {
        IEnumerable<RolePermission> GetPermissionsByRoleId(Int64 RoleId);
    }
}