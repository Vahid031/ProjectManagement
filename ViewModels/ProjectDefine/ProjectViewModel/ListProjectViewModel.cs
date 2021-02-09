using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel;
using System;

namespace ViewModels.ProjectDefine.ProjectViewModel
{
    public class ListProjectViewModel
	{
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [DisplayName("عنوان پروژه")]
        public string ProjectTitle { get; set; }

        [DisplayName("استان")]
        public Int64? StateId { get; set; }

        [DisplayName("استان")]
        public string StateTitle { get; set; }

        [DisplayName("شهر")]
        public Int64? CityId { get; set; }

        [DisplayName("شهر")]
        public string CityTitle { get; set; }

        [DisplayName("سال شروع")]
        public Int64? StartFinantialYearId { get; set; }

        [DisplayName("سال شروع")]
        public string StartFinantialYearTitle { get; set; }

        [DisplayName("پیش بینی سال خاتمه")]
        public Int64? ForecastEndFinantialYearId { get; set; }

        [DisplayName("پیش بینی سال خاتمه")]
        public string ForecastEndFinantialYearTitle { get; set; }

        [DisplayName("مساحت")]
        public string Area { get; set; }

	}
}