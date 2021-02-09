using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionFinalParticipantViewModel
{
    public class CreateProjectSectionFinalParticipantViewModel
	{
        public ProjectSectionFinalParticipant ProjectSectionFinalParticipant { get; set; }
        public List<Contractor> Contractors { get; set; }
        public string Files { get; set; }
	}
}