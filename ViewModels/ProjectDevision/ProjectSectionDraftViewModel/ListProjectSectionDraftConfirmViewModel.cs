using System.Collections.Generic;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.BaseInformation;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;
using DomainModels.Entities.ProjectDevision;

namespace ViewModels.ProjectDevision.ProjectSectionDraftViewModel
{
    // فرم حواله لیست تمام صورت وضعیت های تایید شده
    public class ListProjectSectionDraftListViewModel 
	{
        public long Id { get; set; } // شناسه فرم حواله
        public long ProjectSectionId { get; set; }
        public long ProjectSectionStatementId { get; set; }

        [DisplayName("شماره صورت وضعیت")]
        public string StatementNumberTitle { get; set; } // شماره صورت وضعیت

        [DisplayName("مبلغ صورت وضعیت")]
        public long ConfirmPrice { get; set; } // مبلغ تایید صورت وضعیت

        [DisplayName("مبلغ تایید شده صورت وضعیت")]
        public long PriceStatement { get; set; } // مبلغ صورت وضعیت تایید شده

        [DisplayName("مانده حساب")]
        public long AccountBalance { get; set; } // مانده حساب
        public DateTime? ConfirmDate { get; set; } // تاریخ تایید صورت حساب

        [DisplayName("تاریخ تایید صورت وضعیت")]
        public string ConfirmDate_ { get; set; } // تاریخ تایید صورت وضعیت
        
	}
}