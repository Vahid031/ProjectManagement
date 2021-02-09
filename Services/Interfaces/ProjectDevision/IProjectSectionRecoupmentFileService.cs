using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.Other;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionRecoupmentFileService : IGenericService<ProjectSectionRecoupmentFile>
    {
        IEnumerable<UploadFilesResultViewModel> GetByProjectSectionRecoupmentId(Int64 MemberId, Int64 ProjectSectionRecoupmentId);
    }
}