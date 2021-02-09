using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.Other;

namespace Services.Interfaces.ProjectDefine
{
    public interface IProjectFundingFileService : IGenericService<ProjectFundingFile>
    {
        IEnumerable<UploadFilesResultViewModel> GetByProjectFundingId(Int64 MemberId, Int64 ProjectFundingId);
    }
}