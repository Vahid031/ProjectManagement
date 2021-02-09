using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDefine;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("ExecutionTypes", Schema = "BaseInformation")]
    public class ExecutionType
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "الزامی است")]
        [Remote("TitleValidation", "ExecutionType", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}
