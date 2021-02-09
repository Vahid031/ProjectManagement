using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.SectionViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface ISectionService : IGenericService<Section>
    {
        IEnumerable<ListSectionViewModel> GetAll(ListSectionViewModel lsvm, ref Paging pg);
    }
}