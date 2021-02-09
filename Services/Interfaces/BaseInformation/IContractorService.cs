using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.ContractorViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IContractorService : IGenericService<Contractor>
    {
        IEnumerable<ListContractorViewModel> GetAll(ListContractorViewModel lcvm, ref Paging pg);
        List<Contractor> SelectAll();
        IEnumerable<ReportContractorViewModel> GetAllReport(ReportParameterContractorViewModel parameters);
    }
}