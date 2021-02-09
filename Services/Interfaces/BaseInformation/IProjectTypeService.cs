using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.ProjectTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IProjectTypeService : IGenericService<ProjectType>
    {
        IEnumerable<ListProjectTypeViewModel> GetAll(ListProjectTypeViewModel lptvm, ref Paging pg);
    }
}