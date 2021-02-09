using System.Collections.Generic;
using DomainModels.Entities.General;

namespace ViewModels.General.RoleViewModel
{
    public class CreateRoleViewModel
	{
		public Role Role { get; set; }

        public string PermissionIds { get; set; }

		public ICollection<Member> Members { get; set; }
	}
}