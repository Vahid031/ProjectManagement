using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels.Entities.BaseInformation
{
    [Table("DeductionPercents", Schema = "BaseInformation")]
    public class DeductionPercent
    {
        [Key]
        [DisplayName("درصد کسورات")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان ")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        [DisplayName("درصد کسری")]
        [Required(ErrorMessage= "الزامی است")]
        [Range(0, 100, ErrorMessage="مقدار معتبر نیست")]
        public float? PercentAmount { get; set; }


    }
}
