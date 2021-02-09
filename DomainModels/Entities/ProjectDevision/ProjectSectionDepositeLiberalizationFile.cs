using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionDepositeLiberalizationFiles", Schema = "ProjectDevision")]
    public class ProjectSectionDepositeLiberalizationFile
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionDepositeLiberalizationId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectSectionDepositeLiberalizationId")]
        public virtual ProjectSectionDepositeLiberalization ProjectSectionDepositeLiberalization { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }
    }
}
