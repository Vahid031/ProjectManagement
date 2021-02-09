using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionWinnerDegreeFiles", Schema = "ProjectDevision")]
    public class ProjectSectionWinnerDegreeFile
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionWinnerDegreeId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectSectionWinnerDegreeId")]
        public virtual ProjectSectionWinnerDegree ProjectSectionWinnerDegree { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }
    }
}
