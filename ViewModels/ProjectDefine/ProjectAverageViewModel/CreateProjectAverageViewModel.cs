using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;

namespace ViewModels.ProjectDefine.ProjectAverageViewModel
{
    public class CreateProjectAverageViewModel
	{
        public ProjectAverage ProjectAverage { get; set; }

        public List<FinantialYear> FinantialYears { get; set; }
	}
}