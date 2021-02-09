using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.ProgramViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IProgramService : IGenericService<Program>
    {
        IEnumerable<ListProgramViewModel> GetAll(ListProgramViewModel lpvm, ref Paging pg);
    }
}