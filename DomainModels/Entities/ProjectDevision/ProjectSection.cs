using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.ProjectDevision
{
    [Table("ProjectSections", Schema = "ProjectDevision")]
    public class ProjectSection
    {
        [Key]
        [DisplayName("شناسه ")]
        [Required(ErrorMessage= "الزامی است")]
        public Int64 Id { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("وضعیت کار")]
        public Int64? ProjectSectionAssignmentStateId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("وضعیت کار")]
        public Int64? ProjectSectionOperationStateId { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("شناسه ")]
        public Int64? ProjectId { get; set; }

        [StringLength(100, MinimumLength=0, ErrorMessage="حد اکثر 100 کاراکتر")]
        [DisplayName("عنوان فاز پروژه")]
        public string Title { get; set; }

        [ScaffoldColumn(false)]
        [DisplayName("نحوه واگذاری")]
        public Int64? AssignmentTypeId { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("تاریخ شروع اولین پروژه")]
        public string ForecastStartDate { get; set; }

        [StringLength(10, MinimumLength=0, ErrorMessage="حد اکثر 10 کاراکتر")]
        [DisplayName("پیش بینی تاریخ خاتمه پروژه")]
        public string ForecastEndDate { get; set; }

        [ForeignKey("AssignmentTypeId")]
        public virtual AssignmentType AssignmentType { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("ProjectSectionOperationStateId")]
        public virtual ProjectSectionOperationState ProjectSectionOperationState { get; set; }

        [ForeignKey("ProjectSectionAssignmentStateId")]
        public virtual ProjectSectionAssignmentState ProjectSectionAssignmentState { get; set; }

        public virtual ICollection<ProjectSectionCorrespondence> ProjectSectionCorrespondences { get; set; }

        public virtual ICollection<ProjectSectionMadeh48> ProjectSectionMadeh48s { get; set; }

        public virtual ICollection<ProjectSectionMadeh46> ProjectSectionMadeh46s { get; set; }

        public virtual ICollection<ProjectSectionDepositeLiberalization> ProjectSectionDepositeLiberalizations { get; set; }

        public virtual ICollection<ProjectSectionStatementConfirm> ProjectSectionStatementConfirms { get; set; }

        public virtual ICollection<ProjectSectionDeliveryFixd> ProjectSectionDeliveryFixdes { get; set; }

        public virtual ICollection<ProjectSectionDeliveryTemprory> ProjectSectionDeliveryTemprories { get; set; }

        public virtual ICollection<ProjectSectionTender> ProjectSectionTenders { get; set; }

        public virtual ICollection<ProjectSectionProduct> ProjectSectionProducts { get; set; }

        public virtual ICollection<ProjectSectionParticipant> ProjectSectionParticipants { get; set; }

        public virtual ICollection<ProjectSectionOpenDate> ProjectSectionOpenDates { get; set; }

        public virtual ICollection<ProjectSectionWinner> ProjectSectionWinners { get; set; }

        public virtual ICollection<ProjectSectionWinnerDegree> ProjectSectionWinnerDegrees { get; set; }

        public virtual ICollection<ProjectSectionContract> ProjectSectionContracts { get; set; }

        public virtual ICollection<ProjectSectionComplementarityDate> ProjectSectionComplementarityDates { get; set; }

        public virtual ICollection<ProjectSectionSchedule> ProjectSectionSchedules { get; set; }

        public virtual ICollection<ProjectSectionSupervisorVisit> ProjectSectionSupervisorVisits { get; set; }

        public virtual ICollection<ProjectSectionAdvisorVisit> ProjectSectionAdvisorVisits { get; set; }

        public virtual ICollection<ProjectSectionDraft> ProjectSectionDrafts { get; set; }

        public virtual ICollection<ProjectSectionPaidPrice> ProjectSectionPaidPrices { get; set; }

        public virtual ICollection<ProjectSectionCeremonial> ProjectSectionCeremonials { get; set; }

        public virtual ICollection<ProjectSectionInquiry> ProjectSectionInquiries { get; set; }
        
        public virtual ICollection<ProjectSectionMember> ProjectSectionMembers { get; set; }

        public virtual ICollection<ProjectSectionComplementarityPrice> ProjectSectionComplementarityPrices { get; set; }

        public virtual ICollection<ProjectSectionFinalParticipant> ProjectSectionFinalParticipants { get; set; }

        public virtual ICollection<ProjectSectionFinanceEvaluation> ProjectSectionFinanceEvaluations { get; set; }

        public virtual ICollection<ProjectSectionStatement> ProjectSectionStatements { get; set; }

        public virtual ICollection<ProjectSectionRecoupment> ProjectSectionRecoupments { get; set; }

        public virtual ICollection<ProjectSectionOperationTitle> ProjectSectionOperationTitles { get; set; }

        public virtual ICollection<ProjectAllocate> ProjectAllocates { get; set; }
    }
}
