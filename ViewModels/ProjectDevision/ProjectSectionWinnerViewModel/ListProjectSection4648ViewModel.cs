using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionWinnerViewModel
{
    public class ListProjectSection4648ViewModel
	{
        [DisplayName("شماره قرارداد")]
        public string ProjectNumber { get; set; }

        [DisplayName("نام پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("نام فاز پروژه")]
        public string ProjectSectionTitle { get; set; }

        [DisplayName("تاریخ شروع")]
        public string StartDate_ { get; set; }

        [DisplayName("تاریخ پایان")]
        public string EndDate_ { get; set; }

        [DisplayName("نوع")]
        public string Type4648 { get; set; }

        [DisplayName("تاریخ")]
        public string Date_ { get; set; }


        [DisplayName("مبلغ")]
        public long Price { get; set; }
	}
}