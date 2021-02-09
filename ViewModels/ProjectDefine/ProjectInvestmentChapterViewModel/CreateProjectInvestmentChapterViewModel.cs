using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;

namespace ViewModels.ProjectDefine.ProjectInvestmentChapterViewModel
{
    public class CreateProjectInvestmentChapterViewModel
	{
        public ProjectInvestmentChapter ProjectInvestmentChapter { get; set; }

        [DisplayName("فصل سرمایه گذاری")]
        public Int64? InvestmentChapterId { get; set; }

        public List<FinantialYear> FinantialYears { get; set; }
        public List<InvestmentChapter> InvestmentChapters { get; set; }
        public List<InvestmentChapterType> InvestmentChapterTypes { get; set; }
	}
}