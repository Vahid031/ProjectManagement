using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.BaseInformation
{
    [Table("Opinions", Schema = "BaseInformation")]
    public class Opinion
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("نظر واحد فنی")]
        [Required(ErrorMessage= "الزامی است")]
        public string Title { get; set; }

        public virtual ICollection<ProjectSectionDeliveryTemprory> ProjectSectionDeliveryTemprories { get; set; }

        public virtual ICollection<ProjectSectionDeliveryFixd> ProjectSectionDeliveryFixdes { get; set; }

        public virtual ICollection<ProjectSectionStatementConfirm> ProjectSectionStatementConfirms { get; set; }

        public virtual ICollection<ProjectSectionSchedule> ProjectSectionSchedules { get; set; }

        public virtual ICollection<ProjectSectionStatement> ProjectSectionStatements { get; set; }


    }
}
