using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using ViewModels.Other;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDefine.ProjectAllocateViewModel
{
    public class CreateProjectAllocateViewModel
	{
        public ProjectAllocate ProjectAllocate { get; set; }
        public string Files { get; set; }

        public List<BaseDropdownViewModel> ProjectPlans { get; set; }

        public List<ProjectSection> ProjectSections { get; set; }
	}
}