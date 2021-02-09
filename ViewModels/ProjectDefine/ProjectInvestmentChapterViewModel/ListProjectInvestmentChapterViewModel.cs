using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace ViewModels.ProjectDefine.ProjectInvestmentChapterViewModel
{
    public class ListProjectInvestmentChapterViewModel
	{
        public ProjectInvestmentChapter ProjectInvestmentChapter { get; set; }
        public InvestmentChapter InvestmentChapter { get; set; }
        public InvestmentChapterType InvestmentChapterType { get; set; }
        public FinantialYear FinantialYear { get; set; }
	}
}