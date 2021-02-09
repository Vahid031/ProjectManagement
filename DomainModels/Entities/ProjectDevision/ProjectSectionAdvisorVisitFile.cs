using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionAdvisorVisitFiles", Schema = "ProjectDevision")]
    public class ProjectSectionAdvisorVisitFile
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionAdvisorVisitId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectSectionAdvisorVisitId")]
        public virtual ProjectSectionAdvisorVisit ProjectSectionAdvisorVisit { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }

    }
}
