using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionDepositeLiberalizationViewModel
{
    public class CreateProjectSectionDepositeLiberalizationViewModel
	{
        public ProjectSectionDepositeLiberalization ProjectSectionDepositeLiberalization { get; set; }

        public string Files { get; set; }
	}
}