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
    [Table("ResourceTypes", Schema = "BaseInformation")]
    public class ResourceType
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("منبع سرمایه گذاری")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? ResourceId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نوع منبع اعتبار")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "ResourceType", "BaseInformation", AdditionalFields = "Id,ResourceId", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [ForeignKey("ResourceId")]
        public virtual Resource Resource { get; set; }

        public virtual ICollection<ProjectFunding> ProjectFundings { get; set; }


    }
}
