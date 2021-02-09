using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionCorrespondenceViewModel
{
    public class ListProjectSectionCorrespondenceViewModel
	{
        public ProjectSectionCorrespondence ProjectSectionCorrespondence { get; set; }
        public LetterType LetterType { get; set; }
	}
}