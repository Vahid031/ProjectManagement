using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionCorrespondenceViewModel
{
    public class CreateProjectSectionCorrespondenceViewModel
	{
        public ProjectSectionCorrespondence ProjectSectionCorrespondence { get; set; }
        public string Files { get; set; }
        public List<LetterType> LetterTypes { get; set; }
	}
}