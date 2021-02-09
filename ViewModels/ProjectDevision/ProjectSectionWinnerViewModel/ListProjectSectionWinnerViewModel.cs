using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionWinnerViewModel
{
    public class ListProjectSectionWinnerViewModel
	{
        public ProjectSectionWinner ProjectSectionWinner { get; set; }
        public Rank Rank { get; set; }
        public ProjectSectionFinalParticipant ProjectSectionFinalParticipant { get; set; }
        public Contractor Contractor { get; set; }
	}
}