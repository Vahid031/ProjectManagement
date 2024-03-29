using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.BaseInformation
{
    [Table("ProjectSectionStatementStates", Schema = "BaseInformation")]
    public class ProjectSectionStatementState
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength = 0, ErrorMessage = "حد اکثر 100 کاراکتر")]
        [DisplayName("وضعیت صورت وضعیت")]
        public string Title { get; set; }

        [StringLength(20, MinimumLength=0, ErrorMessage="حد اکثر 20 کاراکتر")]
        [DisplayName("رنگ")]
        public string Color { get; set; }

        public virtual ICollection<ProjectSectionStatement> ProjectSectionStatements { get; set; }
    }
}