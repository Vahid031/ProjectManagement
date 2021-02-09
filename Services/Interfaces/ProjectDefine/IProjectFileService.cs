using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.Other;

namespace Services.Interfaces.BaseInformation
{
    public interface IProjectFileService : IGenericService<ProjectFile>
    {
        IEnumerable<UploadFilesResultViewModel> GetByProjectId(Int64 MemberId, Int64 ProjectId);
    }
}