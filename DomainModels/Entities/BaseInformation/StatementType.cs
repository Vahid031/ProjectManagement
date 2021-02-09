using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using System.Web.Mvc;

namespace DomainModels.Entities.BaseInformation
{
    [Table("StatementTypes", Schema = "BaseInformation")]
    public class StatementType
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نوع صورت وضعیت")]
        [Required(ErrorMessage = "الزامی است")]
        [Remote("TitleValidation", "StatementType", "BaseInformation", AdditionalFields = "Id", ErrorMessage = "تکراری است")]
        public string Title { get; set; }

        public virtual ICollection<ProjectSectionStatement> ProjectSectionStatements { get; set; }
    }
}
