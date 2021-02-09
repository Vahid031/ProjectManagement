using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.PlanTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IPlanTypeService : IGenericService<PlanType>
    {
        IEnumerable<ListPlanTypeViewModel> GetAll(ListPlanTypeViewModel lctvm, ref Paging pg);
    }
}