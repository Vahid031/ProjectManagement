using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.RecoupmentTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IRecoupmentTypeService : IGenericService<RecoupmentType>
    {
        IEnumerable<ListRecoupmentTypeViewModel> GetAll(ListRecoupmentTypeViewModel lctvm, ref Paging pg);
    }
}