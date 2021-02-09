using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("Resources", Schema = "BaseInformation")]
    public class Resource
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("منبع اعتبار")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "Resource", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        public virtual ICollection<ResourceType> ResourceTypes { get; set; }
    }
}
