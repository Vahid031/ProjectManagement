using DomainModels.Entities;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.BaseInformation.DocumentTypeViewModel;

namespace Services.Interfaces.BaseInformation
{
    public interface IDocumentTypeService : IGenericService<DocumentType>
    {
        IEnumerable<ListDocumentTypeViewModel> GetAll(ListDocumentTypeViewModel ldtvm, ref Paging pg);
    }
}