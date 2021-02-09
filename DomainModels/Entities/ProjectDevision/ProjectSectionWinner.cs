using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSectionWinners", Schema = "ProjectDevision")]
    public class ProjectSectionWinner
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectSectionId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("رتبه")]
        public Int64? RankId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("پیمانکار")]
        public Int64? ContractorId { get; set; }

        [ForeignKey("ContractorId")]
        public virtual Contractor Contractor { get; set; }

        [ForeignKey("RankId")]
        public virtual Rank Rank { get; set; }

        [ForeignKey("ProjectSectionId")]
        public virtual ProjectSection ProjectSection { get; set; }


    }
}
