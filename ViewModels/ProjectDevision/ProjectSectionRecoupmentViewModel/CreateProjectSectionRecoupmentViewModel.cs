using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionRecoupmentViewModel
{
    public class CreateProjectSectionRecoupmentViewModel
	{
        public ProjectSectionRecoupment ProjectSectionRecoupment { get; set; }
        public ICollection<RecoupmentType> RecoupmentTypes { get; set; }
        public string Files { get; set; }
	}
}