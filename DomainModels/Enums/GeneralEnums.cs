using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModels.Enums
{
    public static class GeneralEnums
    {
        public enum FileTypes
        {
            ProjectFiles = 1,
            ProjectSectionInquiryFiles = 2,
            ProjectSectionCeremonialFiles = 3,
            ProjectSectionTenderFiles = 4,
            ProjectSectionFinalParticipantFiles = 5,
            ProjectSectionWinnerDegreeFiles = 6,
            ProjectSectionComplementarityPriceFiles = 7,
            ProjectSectionComplementarityDateFiles = 8,
            ProjectSectionContractFiles = 9,
            ProjectSectionScheduleFiles = 10,
            ProjectSectionSupervisorVisitFiles = 11,
            ProjectSectionAdvisorVisitFiles = 12,
            ProjectSectionDeliveryTemproryFiles = 13,
            ProjectSectionDeliveryFixdFiles = 14,
            ProjectSectionDepositeLiberalizationFiles = 15,
            ProjectSectionMadeh46Files = 16,
            ProjectSectionMadeh48Files = 17,
            ProjectSectionCorrespondenceFiles = 18,
            ProjectSectionStatementFiles = 19,
            ProjectSectionDraftFiles = 20,
            ProjectSectionPaidPriceFiles = 21,
            ProjectSectionRecoupmentFiles = 22,
            ProjectAllocateFiles = 23,
            ProjectSectionStatementConfirmFiles = 24,
            ProjectSectionProductFiles = 25,
            ProjectFundingFiles = 26
        }

        public enum MemberGroup
        {
            Personel = 1,
            Supervisor = 2,
            Advisor = 3,
        }

        public enum FileStates
        {
            Temp = 1,
            Confirmed = 2,
            Deleted = 3
        }

        public enum ProjectState
        {
            WaitingAgreement = 1, // منتظر ثبت موافقت نامه
            WaitingAlocation = 2, // منتظر تخصیص
            WaitingDivisionProject = 3, // منتظر تقسیم بندی پروژه
            DivisionPerformed = 4 // تقسیم بندی انجام شد
        }

        public enum ProjectSectionAssignmentStates
        {
            ProjectSectionInquiry = 1,
            ProjectSectionTender = 2,
            ProjectSectionCeremonial = 3,
            ProjectSectionParticipant = 4,
            ProjectSectionFinanceEvaluation = 5,
            ProjectSectionFinalParticipant = 6,
            ProjectSectionOpenDate = 7,
            ProjectSectionWinner = 8,
            ProjectSectionWinnerDegree = 9,
            ProjectSectionContract = 10,
            ProjectSectionFinalContract = 11,
        }

        public enum ProjectSectionOperationStates
        {
            ProjectSectionAdvisorSupervisorSelect = 1,
            ProjectSectionSchedule = 2,
            ProjectSectionAdvisorSupervisorVisit = 3,
            End = 4,
        }

        public enum ProjectSectionStatementStates
        {
            Statement = 1,
            StatementConfirm = 2,
            PaidPrice = 3,
            DonePaidPrice = 4,
            End = 5,
        }
    }
}
