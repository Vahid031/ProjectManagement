using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.LevelTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface ILevelTypeService : IGenericService<LevelType>
    {
        IEnumerable<ListLevelTypeViewModel> GetAll(ListLevelTypeViewModel lctvm, ref Paging pg);
    }
}