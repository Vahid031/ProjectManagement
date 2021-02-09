using DatabaseContext.Context;
using DomainModels.Entities;
using DomainModels.Entities.General;
using Services.Interfaces;
using Services.Interfaces.General;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Services.EfServices.General
{
    
    public class RolePermissionService : GenericService<RolePermission>, IRolePermissionService
    {
        public RolePermissionService(IUnitOfWork uow)
            : base(uow)
        {
            
        }

        public IEnumerable<RolePermission> GetPermissionsByRoleId(Int64 RoleId)
        {
            return _uow.Set<RolePermission>().Where(i => i.RoleId == RoleId).ToList();
        }

    }
}