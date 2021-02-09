using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.ProjectDevision;
using ViewModels.ProjectDevision.ProjectSectionProductViewModel;
using DomainModels.Entities.BaseInformation;
using Services.EfServices.General;
using DomainModels.Enums;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionProductService : GenericService<ProjectSectionProduct>, IProjectSectionProductService
    {
        public ProjectSectionProductService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionProductViewModel> GetAll(ListProjectSectionProductViewModel lcvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProjectSectionProductViewModel>(lcvm, ref pg);

            var a =
            _uow.Set<ProjectSectionProduct>()
                .Join(_uow.Set<ServiceProduct>(), ProjectSectionProduct => ProjectSectionProduct.ServiceProductId, ServiceProduct => ServiceProduct.Id, (ProjectSectionProduct, ServiceProduct) => new { ProjectSectionProduct, ServiceProduct })
                ;

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProjectSectionProductViewModel()
            {
                ProjectSectionProduct = new ProjectSectionProduct() { Id = Result.ProjectSectionProduct.Id, EstimatesPrice = Result.ProjectSectionProduct.EstimatesPrice, ProjectSectionId = Result.ProjectSectionProduct.ProjectSectionId, ServiceProductContent = Result.ProjectSectionProduct.ServiceProductContent, ServiceProductId = Result.ProjectSectionProduct.ServiceProductId, UsedAmount = Result.ProjectSectionProduct.UsedAmount },
                ServiceProduct = new ServiceProduct() { Id = Result.ServiceProduct.Id, Title = Result.ServiceProduct.Title },
            });
        }


        public KeyValuePair<Int64, string> Save(CreateProjectSectionProductViewModel cpsivm, Int64 MemberId, List<string> paths)
        {
            ProjectSectionProductService _ProjectSectionProductService = new ProjectSectionProductService(_uow);
            ProjectSectionProductFileService _ProjectSectionProductFileService = new ProjectSectionProductFileService(_uow);
            
            FileService _fileService = new FileService(_uow);

            long? isInsert = cpsivm.ProjectSectionProduct.Id;

            if (isInsert == -1)
            {
                _ProjectSectionProductService.Insert(cpsivm.ProjectSectionProduct);
            }
            else
            {
                _ProjectSectionProductService.Update(cpsivm.ProjectSectionProduct);

                foreach (var item in _ProjectSectionProductFileService.Get(i => i.ProjectSectionProductId == cpsivm.ProjectSectionProduct.Id).ToList())
                {
                    var file = _fileService.Get(i => i.Id == item.FileId && i.FileState == 3).FirstOrDefault();
                    if (file != null)
                    {
                        paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                        _fileService.Delete(item.FileId);
                        _ProjectSectionProductFileService.Delete(item);
                    }
                }
            }

            if (cpsivm.Files != null)
            {
                foreach (string item in cpsivm.Files.Split(','))
                {
                    if (item != "")
                    {
                        Int64 fileId = Convert.ToInt64(item);
                        if (_fileService.Get(i => i.Id == fileId).First().FileState.Value == 1)
                        {
                            ProjectSectionProductFile ProjectSectionProductFile = new ProjectSectionProductFile();
                            ProjectSectionProductFile.ProjectSectionProductId = cpsivm.ProjectSectionProduct.Id;
                            ProjectSectionProductFile.FileId = fileId;

                            _ProjectSectionProductFileService.Insert(ProjectSectionProductFile);

                            var file = _fileService.Find(Convert.ToInt64(item));
                            file.FileState = Convert.ToInt32(GeneralEnums.FileStates.Confirmed);
                            _fileService.Update(file);
                        }
                    }
                }
            }

            if (_uow.SaveChanges(MemberId) > 0)
            {
                return new KeyValuePair<long, string>(cpsivm.ProjectSectionProduct.Id, "عملیات با موفقیت انجام شد");
            }
            else
            {
                return new KeyValuePair<long, string>(0, "خطا در ثبت اطلاعات");
            }
        }

        // این کد چک می کند که آیا مجموع مبلغ تخصیص به پروژه
        //بیشتر از مجموع مبلغ پیش بینی فاز های پروژه می باشد؟
        public bool CheckAllocatePriceAndProductPrice(long ProjectSectionId)
        {
            bool b = _uow.GetInstance().Database.SqlQuery<bool>("exec CheckAllocatePriceAndProductPrice " + ProjectSectionId).FirstOrDefault();

            return b;
        }
    }
}
