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
    [Table("Programs", Schema = "BaseInformation")]
    public class Program
    {
        [Key]
        [DisplayName("برنامه")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان برنامه")]
        [Required(ErrorMessage= "الزامی است")]
        [Remote("TitleValidation", "Program", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        [DisplayName("از سال")]
        [Range(1380, 1500, ErrorMessage = "سال معتبر نیست")]
        public int? FromYear { get; set; }

        [DisplayName("تا سال")]
        [Range(1380, 1500, ErrorMessage = "سال معتبر نیست")]
        public int? ToYear { get; set; }

        public virtual ICollection<ProgramPlan> ProgramPlans { get; set; }
        public virtual ICollection<ProjectPlan> ProjectPlans { get; set; }


    }
}
