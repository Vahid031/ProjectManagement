using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionFinalParticipantViewModel
{
    public class ListProjectSectionFinalParticipantViewModel
	{
        public ProjectSectionFinalParticipant ProjectSectionFinalParticipant { get; set; }
        public Contractor Contractor { get; set; }
	}
}