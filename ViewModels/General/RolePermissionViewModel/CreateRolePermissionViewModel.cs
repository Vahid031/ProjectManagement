using System.Collections.Generic;
using DomainModels.Entities.General;

namespace ViewModels.General.RolePermissionViewModel
{
    public class CreateRolePermissionViewModel
	{
        public RolePermission RolePermission { get; set; }
        public Role Role { get; set; }
        public Permission Permission { get; set; }
	}
}