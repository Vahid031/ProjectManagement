using System.Collections.Generic;
using DomainModels.Entities.General;

namespace ViewModels.General.LogViewModel
{
	public class CreateLogViewModel
	{
		public Log Log { get; set; }
		public ICollection<Member> Members { get; set; }
	}
}