using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.MajorTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IMajorTypeService : IGenericService<MajorType>
    {
        IEnumerable<ListMajorTypeViewModel> GetAll(ListMajorTypeViewModel lctvm, ref Paging pg);
    }
}