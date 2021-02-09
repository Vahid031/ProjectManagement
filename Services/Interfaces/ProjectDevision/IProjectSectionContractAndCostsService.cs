using DomainModels.Entities.ProjectDevision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProjectDefine.ProjectSectionContractAndCostsReportViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionContractAndCostsService : IGenericService<ProjectSectionContract>
    {
        IEnumerable<ReportProjectSectionContractAndCostsViewModel> GetAllReport(ReportParameterProjectSectionContractAndCostsViewModel parameters);

    }
}
