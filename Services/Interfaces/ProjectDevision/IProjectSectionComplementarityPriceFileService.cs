using DomainModels.Entities;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using System;
using System.Collections.Generic;
using ViewModels.Other;

namespace Services.Interfaces.ProjectDevision
{
    public interface IProjectSectionComplementarityPriceFileService : IGenericService<ProjectSectionComplementarityPriceFile>
    {
        IEnumerable<UploadFilesResultViewModel> GetByProjectSectionComplementarityPriceId(Int64 MemberId, Int64 ProjectSectionComplementarityPriceId);
    }
}