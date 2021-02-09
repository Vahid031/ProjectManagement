using DomainModels.Entities;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.ProjectDevision.ProjectSectionInquiryViewModel;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionInquiryService : IGenericService<ProjectSectionInquiry>
    {
        IEnumerable<ListProjectSectionInquiryViewModel> GetAll(ListProjectSectionInquiryViewModel latvm, ref Paging pg);

        KeyValuePair<Int64, string> Save(CreateProjectSectionInquiryViewModel cpsivm, Int64 MemberId, List<string> paths);
    }
}