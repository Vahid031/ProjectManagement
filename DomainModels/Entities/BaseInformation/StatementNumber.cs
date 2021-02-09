using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.BaseInformation
{
    [Table("StatementNumbers", Schema = "BaseInformation")]
    public class StatementNumber
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? TempFixedId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("شماره صورت وضعیت")]
        public string Title { get; set; }

        public virtual ICollection<ProjectSectionStatement> ProjectSectionStatements { get; set; }

        [ForeignKey("TempFixedId")]
        public virtual TempFixed TempFixed { get; set; }


    }
}
