using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.Other;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionMadeh46FileService : IGenericService<ProjectSectionMadeh46File>
    {
        IEnumerable<UploadFilesResultViewModel> GetByProjectSectionMadeh46Id(Int64 MemberId, Int64 ProjectSectionMadeh46Id);
    }
}