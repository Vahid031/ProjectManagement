using System.Collections.Generic;
using DomainModels.Entities.General;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.General.MemberViewModel
{
    public class ListMemberViewModel
	{
		public Member Member { get; set; }
		public Role Role { get; set; }
		public MemberGroup MemberGroup { get; set; }
	}
}