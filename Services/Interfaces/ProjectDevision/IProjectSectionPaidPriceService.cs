using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDefine.ProjectSectionStatementPayReportViewModel;
using ViewModels.ProjectDevision.ProjectSectionPaidPriceViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionPaidPriceService : IGenericService<ProjectSectionPaidPrice>
    {
        KeyValuePair<Int64, string> Save(CreateProjectSectionPaidPriceViewModel cpscvm, Int64 MemberId, List<string> paths);
        IEnumerable<ListProjectSectionPaidPriceViewModel> GetAll(ListProjectSectionPaidPriceViewModel ldtvm, ref Paging pg);
        IEnumerable<ReportProjectSectionStatementPayViewModel> GetAllReport(ReportParameterProjectSectionStatementPayViewModel parameters);
        IEnumerable<CalculateProjectSectionPaidPriceViewModel> CalculatePadePriceList(long ProjectSectionId);


        // این کد چک می کند که مبلغ پرداختی کمتر از مبلغ قرارداد باشد
        bool CalculateCanInsert(long? projectSectionId, long price);
    }
}