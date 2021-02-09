using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;

namespace ViewModels.BaseInformation.ProgramViewModel
{
	public class CreateProgramViewModel
	{
		public Program Program { get; set; }

        public List<FinantialYear> FinantialYears { get; set; }
	}
}