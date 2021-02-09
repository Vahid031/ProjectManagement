using System.Collections.Generic;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionScheduleViewModel
{
    public class CreateProjectSectionScheduleViewModel
	{
        public ProjectSectionSchedule ProjectSectionSchedule { get; set; }

        public string Files { get; set; }

        public List<Opinion> Opinions { get; set; }
	}
}