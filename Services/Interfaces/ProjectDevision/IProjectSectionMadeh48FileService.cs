using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.Other;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionMadeh48FileService : IGenericService<ProjectSectionMadeh48File>
    {
        IEnumerable<UploadFilesResultViewModel> GetByProjectSectionMadeh48Id(Int64 MemberId, Int64 ProjectSectionMadeh48Id);
    }
}