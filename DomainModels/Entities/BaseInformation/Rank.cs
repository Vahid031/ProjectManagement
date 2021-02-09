using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.BaseInformation
{
    [Table("Ranks", Schema = "BaseInformation")]
    public class Rank
    {
        [Key]
        [DisplayName("شناسه ")]
        public Int64? Id { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("رتبه")]
        public string Title { get; set; }

        public virtual ICollection<ProjectSectionWinner> ProjectSectionWinners { get; set; }


    }
}
