using DomainModels.Entities;
using DomainModels.Entities.General;
using System;

namespace Services.Interfaces.General
{
    public interface IFileService : IGenericService<File>
    {
        void DeleteFiles(Int64 MemberId);
    }
}