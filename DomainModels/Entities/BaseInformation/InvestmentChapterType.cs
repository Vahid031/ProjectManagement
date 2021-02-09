using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("InvestmentChapterTypes", Schema = "BaseInformation")]
    public class InvestmentChapterType
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("فصل سرمایه گذاری")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? InvestmentChapterId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نوع فصول سرمایه گذاری")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "InvestmentChapterType", "BaseInformation", AdditionalFields = "Id,InvestmentChapterId", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [ForeignKey("InvestmentChapterId")]
        public virtual InvestmentChapter InvestmentChapter { get; set; }

        public virtual ICollection<ProjectInvestmentChapter> ProjectInvestmentChapters { get; set; }


    }
}
