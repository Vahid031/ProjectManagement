using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using ViewModels.Other;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionCeremonialFileService : GenericService<ProjectSectionCeremonialFile>, IProjectSectionCeremonialFileService
    {
        public ProjectSectionCeremonialFileService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<UploadFilesResultViewModel> GetByProjectSectionCeremonialId(Int64 MemberId, Int64 ProjectSectionCeremonialId)
        {
            string query = "";
            query += @"Select GF.Id imageId, GF.Title name, GF.[Address] url , 'POST' delete_type, '/ProjectDevision/ProjectSectionCeremonial/DeleteFile' delete_url,
                              Cast(GF.Size as int) size, GF.[Type] type, Case When GF.[Type] like '%image%' then GF.[Address] else '/Uploads/noimage.png' end thumbnail_url
                       From General.Files GF
                                left join ProjectDevision.ProjectSectionCeremonialFiles PSIF on GF.Id = PSIF.FileId
                       Where ((GF.MemberId = " + MemberId + " And FileState = 1) OR PSIF.ProjectSectionCeremonialId = " + ProjectSectionCeremonialId + ") And GF.FileTypeId = 3 And FileState <> 3";

            IEnumerable<UploadFilesResultViewModel> result = _uow.GetInstance().Database.SqlQuery<UploadFilesResultViewModel>(query)
                .Select(Result => new UploadFilesResultViewModel()
                {
                    imageId = Result.imageId,
                    name = Result.name,
                    url = Result.url,
                    thumbnail_url = Result.thumbnail_url,
                    delete_type = Result.delete_type,
                    delete_url = Result.delete_url,
                    size = Result.size,
                    type = Result.type
                });

            return result;
        }
    }
}
