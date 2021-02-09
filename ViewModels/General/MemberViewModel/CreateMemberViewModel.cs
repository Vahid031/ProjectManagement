using System.Collections.Generic;
using DomainModels.Entities.General;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ViewModels.Other;

namespace ViewModels.General.MemberViewModel
{
    public class CreateMemberViewModel
	{
		public Member Member { get; set; }

        [DisplayName("کلمه عبور قبلی")]
        public string OldPassword { get; set; }

		public ICollection<BaseDropdownViewModel> Roles { get; set; }
		public ICollection<MemberGroup> MemberGroups { get; set; }
	}
}