using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using System.ComponentModel.DataAnnotations;

namespace ViewModels.ProjectDevision.ProjectSectionDraftViewModel
{
    public class CreateProjectSectionDraftViewModel
	{
        public ProjectSectionDraft ProjectSectionDraft { get; set; }
        public string Files { get; set; }
	}
}