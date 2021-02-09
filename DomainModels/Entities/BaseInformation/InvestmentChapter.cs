using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("InvestmentChapters", Schema = "BaseInformation")]
    public class InvestmentChapter
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("فصل سرمایه گذاری")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "InvestmentChapter", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        public virtual ICollection<InvestmentChapterType> InvestmentChapterTypes { get; set; }
    }
}
