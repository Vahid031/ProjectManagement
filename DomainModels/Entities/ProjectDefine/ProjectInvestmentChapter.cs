using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;

namespace DomainModels.Entities.ProjectDefine
{
    [Table("ProjectInvestmentChapters", Schema = "ProjectDefine")]
    public class ProjectInvestmentChapter
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectId { get; set; }

        [DisplayName("سال عملکرد")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? FinantialYearId { get; set; }


        [ScaffoldColumn(false)]
        [DisplayName("نوع فصول سرمایه گذاری")]
        public Int64? InvestmentChapterTypeId { get; set; }

        [DisplayName("مبلغ")]
        public Int64? Price { get; set; }

        [ForeignKey("InvestmentChapterTypeId")]
        public virtual InvestmentChapterType InvestmentChapterType { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("FinantialYearId")]
        public virtual FinantialYear FinantialYear { get; set; }

    }
}
