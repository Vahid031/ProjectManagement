using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.Other;

namespace Services.Interfaces.ProjectDefine
{
    public interface IProjectAllocateFileService : IGenericService<ProjectAllocateFile>
    {
        IEnumerable<UploadFilesResultViewModel> GetByProjectAllocateId(Int64 MemberId, Int64 ProjectAllocateId);
    }
}