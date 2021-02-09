using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionViewModel
{
    public class CreateProjectSectionViewModel
	{
        public ProjectSection ProjectSection { get; set; }

        public List<AssignmentType> AssignmentTypes { get; set; }
	}
}