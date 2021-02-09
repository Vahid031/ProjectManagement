using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;
using DomainModels.Entities.General;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionScheduleFiles", Schema = "ProjectDevision")]
    public class ProjectSectionScheduleFile
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionScheduleId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("مسیر")]
        public Int64 FileId { get; set; }

        [ForeignKey("ProjectSectionScheduleId")]
        public virtual ProjectSectionSchedule ProjectSectionSchedule { get; set; }

        [ForeignKey("FileId")]
        public virtual File File { get; set; }

    }
}
