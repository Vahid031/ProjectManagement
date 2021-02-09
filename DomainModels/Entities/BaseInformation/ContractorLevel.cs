using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.ReqularExpression;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("ContractorLevels", Schema = "BaseInformation")]
    public class ContractorLevel
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نماینده")]
        public Int64? ContractorId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("رتبه پیمانکار")]
        public Int64? LevelTypeId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("رشته پیمانکار")]
        public Int64? MajorTypeId { get; set; }

        [StringLength(10, MinimumLength = 0, ErrorMessage = "حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ اتمام اعتبار")]
        [RegularExpression(CustomRegex.PERSIAN_DATE, ErrorMessage = "تاریخ را درست وارد کنید")]
        public string FinishCreditDate { get; set; }


        [ForeignKey("ContractorId")]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey("LevelTypeId")]
        public virtual LevelType LevelType { get; set; }

        [ForeignKey("MajorTypeId")]
        public virtual MajorType MajorType { get; set; }
    }
}