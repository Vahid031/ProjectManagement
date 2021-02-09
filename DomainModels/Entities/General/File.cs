using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels.Entities.General;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;

namespace DomainModels.Entities.General
{
    [Table("Files", Schema = "General")]
    public class File
    {
        [Key]
        [DisplayName("شناسه")]
        public Int64? Id { get; set; }

        [ScaffoldColumn(false)]
        [StringLength(500, MinimumLength = 0, ErrorMessage = "حد اکثر 500 کاراکتر")]
        [DisplayName("عنوان")]
        [Required(ErrorMessage = "الزامی است")]
        public string Title { get; set; }

        [StringLength(500, MinimumLength = 0, ErrorMessage = "حد اکثر 500 کاراکتر")]
        [DisplayName("آدرس")]
        [Required(ErrorMessage = "الزامی است")]
        public string Address { get; set; }

        [DisplayName("زمان آپلود")]
        [Required(ErrorMessage = "الزامی است")]
        public string DateTime { get; set; }

        [DisplayName("نوع")]
        [Required(ErrorMessage = "الزامی است")]
        public string Type { get; set; }

        [DisplayName("نوع فایل")]
        [Required(ErrorMessage = "الزامی است")]
        public string Size { get; set; }

        [DisplayName("نوع فایل")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? FileTypeId { get; set; }

        [DisplayName("آپلود کننده")]
        [Required(ErrorMessage = "الزامی است")]
        public Int64? MemberId { get; set; }

        [DisplayName("وضعیت فایل")]
        [Required(ErrorMessage = "الزامی است")]
        public int? FileState { get; set; }

        [ForeignKey("FileTypeId")]
        public virtual FileType FileType { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }


        public virtual ICollection<ProjectFile> ProjectFiles { get; set; }
        public virtual ICollection<ProjectSectionInquiryFile> ProjectSectionInquiryFiles { get; set; }
        public virtual ICollection<ProjectSectionCeremonialFile> ProjectSectionCeremonialFiles { get; set; }
        public virtual ICollection<ProjectSectionComplementarityDateFile> ProjectSectionComplementarityDateFiles { get; set; }
        public virtual ICollection<ProjectSectionComplementarityPriceFile> ProjectSectionComplementarityPriceFiles { get; set; }
        public virtual ICollection<ProjectSectionContractFile> ProjectSectionContractFiles { get; set; }
        public virtual ICollection<ProjectSectionCorrespondenceFile> ProjectSectionCorrespondenceFiles { get; set; }
        public virtual ICollection<ProjectSectionDeliveryFixdFile> ProjectSectionDeliveryFixdFiles { get; set; }
        public virtual ICollection<ProjectSectionDeliveryTemproryFile> ProjectSectionDeliveryTemproryFiles { get; set; }
        public virtual ICollection<ProjectSectionWinnerDegreeFile> ProjectSectionWinnerDegreeFiles { get; set; }
        public virtual ICollection<ProjectSectionTenderFile> ProjectSectionTenderFiles { get; set; }
        public virtual ICollection<ProjectSectionFinalParticipantFile> ProjectSectionFinalParticipantFiles { get; set; }
        public virtual ICollection<ProjectSectionScheduleFile> ProjectSectionScheduleFiles { get; set; }
        public virtual ICollection<ProjectSectionAdvisorVisitFile> ProjectSectionAdvisorVisitFiles { get; set; }
        public virtual ICollection<ProjectSectionSupervisorVisitFile> ProjectSectionSupervisorVisitFiles { get; set; }
        public virtual ICollection<ProjectSectionDepositeLiberalizationFile> ProjectSectionDepositeLiberalizationFiles { get; set; }
        public virtual ICollection<ProjectSectionMadeh46File> ProjectSectionMadeh46Files { get; set; }
        public virtual ICollection<ProjectSectionMadeh48File> ProjectSectionMadeh48Files { get; set; }
        public virtual ICollection<ProjectSectionStatementFile> ProjectSectionStatementFiles { get; set; }
        public virtual ICollection<ProjectSectionDraftFile> ProjectSectionDraftFiles { get; set; }
        public virtual ICollection<ProjectSectionPaidPriceFile> ProjectSectionPaidPriceFiles { get; set; }
        public virtual ICollection<ProjectAllocateFile> ProjectAllocateFiles { get; set; }
        public virtual ICollection<ProjectSectionStatementConfirmFile> ProjectSectionStatementConfirmFiles { get; set; }
        public virtual ICollection<ProjectSectionProductFile> ProjectSectionProductFiles { get; set; }
        public virtual ICollection<ProjectFundingFile> ProjectFundingFiles { get; set; }
    }
}
