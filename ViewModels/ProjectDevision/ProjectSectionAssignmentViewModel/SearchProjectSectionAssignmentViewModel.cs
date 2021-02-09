﻿using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDevision.ProjectSectionAssignmentViewModel
{
    public class SearchProjectSectionAssignmentViewModel
	{
        [DisplayName("کد پروژه")]
        public string FileCode { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("عنوان فاز")]
        public string ProjectSectionTitle { get; set; }

        [DisplayName("استان")]
        public Int64? StateId { get; set; }

        [DisplayName("شهرستان")]
        public Int64? CityId { get; set; }

        [DisplayName("وضعیت فاز")]
        public Int64? ProjectSectionAssignmentStateId { get; set; }

        public List<ProjectSectionAssignmentState> ProjectSectionAssignmentStates { get; set; }
        public List<State> States { get; set; }
        public List<City> Cities { get; set; }
	}
}