using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;

namespace DomainModels.Entities.BaseInformation
{
    [Table("MajorTypes", Schema = "BaseInformation")]
    public class MajorType
    {
        [Key]
        [DisplayName("رشته")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان رشته")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        public virtual ICollection<ContractorLevel> ContractorLevels { get; set; }


    }
}
