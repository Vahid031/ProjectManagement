using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;
using ViewModels.Other;

namespace ViewModels.ProjectDevision.ProjectSectionWinnerViewModel
{
    public class CreateProjectSectionWinnerViewModel
	{
        public ProjectSectionWinner ProjectSectionWinner { get; set; }
        public List<Rank> Ranks { get; set; }
        public List<Contractor> Contractors { get; set; }
	}
}