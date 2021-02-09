using DatabaseContext.Context;
using DomainModels.Entities;
using DomainModels.Entities.General;
using Services.Interfaces;
using Services.Interfaces.General;
using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Services.EfServices.BaseInformation;

namespace Services.EfServices.General
{
    
    public class FileService : GenericService<File>, IFileService
    {
        public FileService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public void DeleteFiles(Int64 MemberId)
        {
            ProjectFileService _projectFileService = new ProjectFileService(_uow);
            List<string> paths = new List<string>();

            foreach (var item in this.Get(i => (i.FileState == 1 || i.FileState == 3) && i.MemberId == MemberId).ToList())
            {
                if ((item.FileState == 1) || (item.FileState == 3 && _projectFileService.Get(i => i.FileId == item.Id).FirstOrDefault() == null))
                {
                    paths.Add(this.Get(i => i.Id == item.Id).First().Address);

                    this.Delete(item);
                }
                else if (item.FileState == 3 && _projectFileService.Get(i => i.FileId == item.Id).FirstOrDefault() != null)
                {
                    item.FileState = 2;

                    this.Update(item);
                }
                
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                foreach (string item in paths)
                {
                    if (System.IO.File.Exists(System.AppDomain.CurrentDomain.BaseDirectory + item))
                    {
                        System.IO.File.Delete(System.AppDomain.CurrentDomain.BaseDirectory + item);
                    }
                }
            }
        }

    }
}