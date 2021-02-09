using System.Collections.Generic;
using DomainModels.Entities.General;

namespace ViewModels.General.LogDetailViewModel
{
	public class CreateLogDetailViewModel
	{
		public LogDetail LogDetail { get; set; }
		public ICollection<Log> Logs { get; set; }
	}
}