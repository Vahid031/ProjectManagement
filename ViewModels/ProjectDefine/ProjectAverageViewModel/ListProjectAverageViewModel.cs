using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDefine.ProjectAverageViewModel
{
    public class ListProjectAverageViewModel
	{
        public ProjectAverage ProjectAverage { get; set; }
        public FinantialYear FinantialYear { get; set; }
	}
}