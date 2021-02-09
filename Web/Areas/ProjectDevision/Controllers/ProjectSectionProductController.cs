using DatabaseContext.Context;
using DomainModels.Entities.ProjectDevision;
using Infrastracture.AppCode;
using Services.Interfaces.ProjectDevision;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.ProjectDevision.ProjectSectionProductViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;
using Services.Interfaces.BaseInformation;
using DomainModels.Entities.BaseInformation;
using DomainModels.Enums;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionProductController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionProductService _projectSectionProductService;
        IServiceProductService _serviceProductService;
        IProjectSectionProductFileService _projectSectionProductFileService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionProductController(IUnitOfWork uow, IProjectSectionProductService ProjectSectionProductService, IServiceProductService serviceProductService, IProjectSectionProductFileService projectSectionProductFileService, IFileService fileService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionProductService = ProjectSectionProductService;
            _serviceProductService = serviceProductService;
            _permissionService = permissionService;
            _projectSectionProductFileService = projectSectionProductFileService;
            _fileService = fileService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            return PartialView();
        }

        #endregion


        #region List

        [HttpGet]
        public PartialViewResult _List()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult _List(ListProjectSectionProductViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionProductService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create(int Id)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            CreateProjectSectionProductViewModel cpsptvm = new CreateProjectSectionProductViewModel();

            cpsptvm.ServiceProducts = new List<ServiceProduct>() { new ServiceProduct() { Title = "انتخاب کنید..." } };
            cpsptvm.ServiceProducts.AddRange(_serviceProductService.Get().ToList());

            ViewBag.Id = "-1";

            ReloadFiles();

            // محاسبه برآورد مبالغ
            //---------------------------------------------------
            var product = _projectSectionProductService.Get()
                   .Where(c => c.ProjectSectionId == Id && c.ServiceProductId == 1)
                   .Sum(c => c.EstimatesPrice);

            ViewBag.Product = product = (product == null) ? 0 : product;

            //-----------
            var services = _projectSectionProductService.Get()
                .Where(c => c.ProjectSectionId == Id && c.ServiceProductId == 2)
                .Sum(c => c.EstimatesPrice);

            ViewBag.Services = services = (services == null) ? 0 : services;

            //-----------
            //var total = product + services;
            var total = _projectSectionProductService.Get()
                .Where(c => c.ProjectSectionId == Id)
                .Sum(c => c.EstimatesPrice);

            ViewBag.Total = total;
            //--------------------------------------------------


            return PartialView(cpsptvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionProductViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectSectionProduct.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _projectSectionProductService.Save(catvm, Member.Id, paths);

                if (result.Key > 0)
                {
                    // محاسبه برآورد مبالغ
                    //------------------------------------------------------
                    var product = _projectSectionProductService.Get()
                       .Where(c => c.ProjectSectionId == catvm.ProjectSectionProduct.ProjectSectionId && c.ServiceProductId == 1)
                       .Sum(c => c.EstimatesPrice);

                    product = (product == null) ? 0 : product;

                    //-----------
                    var services = _projectSectionProductService.Get()
                        .Where(c => c.ProjectSectionId == catvm.ProjectSectionProduct.ProjectSectionId && c.ServiceProductId == 2)
                        .Sum(c => c.EstimatesPrice);

                    services = (services == null) ? 0 : services;

                    //-----------
                    //var total = product + services;
                    var total = _projectSectionProductService.Get()
                        .Where(c => c.ProjectSectionId == catvm.ProjectSectionProduct.ProjectSectionId)
                        .Sum(c => c.EstimatesPrice);

                    //------------------------------------------------------


                    bool IsAllocatePriceBiger = _projectSectionProductService.CheckAllocatePriceAndProductPrice(catvm.ProjectSectionProduct.ProjectSectionId.Value);


                    foreach (string item in paths)
                    {
                        CheckUploadFile.DeleteFile(Server.MapPath(item));
                    }

                    if (isInsert == -1)
                        return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessInsert), product = product, services = services, total = total, isAllocatePriceBiger = IsAllocatePriceBiger });
                    else
                        return Json(new { type = AlarmType.info.ToString(), message = Messages.GetMsg(MsgKey.SuccessUpdate), product = product, services = services, total = total, isAllocatePriceBiger = IsAllocatePriceBiger });
                }
                else
                {
                    if (isInsert == -1)
                        return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorInsert) });
                    else
                        return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorUpdate) });
                }
                
            }
            catch (Exception)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }

        [HttpPost]
        public PartialViewResult _Update(Int64 id)
        {
            try
            {
                CreateProjectSectionProductViewModel cpsptvm = new CreateProjectSectionProductViewModel();
                cpsptvm.ProjectSectionProduct = _projectSectionProductService.Find(id);

                cpsptvm.ServiceProducts = new List<ServiceProduct>() { new ServiceProduct() { Title = "انتخاب کنید..." } };
                cpsptvm.ServiceProducts.AddRange(_serviceProductService.Get().ToList());

                ReloadFiles();

                foreach (var item in _projectSectionProductFileService.Get(i => i.ProjectSectionProductId == id).ToList())
                {
                    cpsptvm.Files += item.FileId + ",";
                }
                return PartialView("_Create", cpsptvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionProductViewModel());
            }

        }



        #endregion


        #region Delete

        [HttpPost]
        public JsonResult _Delete(Int64 id)
        {
            try
            {
                if (!ModelState.IsValidField("Id") || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });

                ProjectSectionProduct psp = _projectSectionProductService.Find(id);
                _uow.Entry<ProjectSectionProduct>(psp).State = System.Data.Entity.EntityState.Detached;


                List<string> paths = new List<string>();

                foreach (var item in _projectSectionProductFileService.Get(i => i.ProjectSectionProductId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _projectSectionProductFileService.Delete(item);
                }
                

                _projectSectionProductService.Delete(id);

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
                    foreach (string item in paths)
                    {
                        Utilities.AppCode.CheckUploadFile.DeleteFile(Server.MapPath(item));
                    }

                    // محاسبه برآورد مبالغ
                    //------------------------------------------------------
                    var product = _projectSectionProductService.Get()
                       .Where(c => c.ProjectSectionId == psp.ProjectSectionId && c.ServiceProductId == 1)
                       .Sum(c => c.EstimatesPrice);

                    product = (product == null) ? 0 : product;

                    //-----------
                    var services = _projectSectionProductService.Get()
                        .Where(c => c.ProjectSectionId == psp.ProjectSectionId && c.ServiceProductId == 2)
                        .Sum(c => c.EstimatesPrice);

                    services = (services == null) ? 0 : services;

                    //-----------
                    //var total = product + services;
                    var total = _projectSectionProductService.Get()
                        .Where(c => c.ProjectSectionId == psp.ProjectSectionId)
                        .Sum(c => c.EstimatesPrice);

                    //------------------------------------------------------


                    bool IsAllocatePriceBiger = _projectSectionProductService.CheckAllocatePriceAndProductPrice(psp.ProjectSectionId.Value);

                    return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessDelete), product = product, services = services, total = total, isAllocatePriceBiger = IsAllocatePriceBiger });
                }
                else
                {
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorDelete) });
                }
            }
            catch (Exception)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }

        #endregion


        #region FileUpload

        [AllowAnonymous]
        public void ReloadFiles()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            _fileService.DeleteFiles(Member.Id);
        }

        [AllowAnonymous]
        public PartialViewResult _FileUpload()
        {
            return PartialView();
        }

        [AllowAnonymous]
        [HttpPost]
        public virtual ActionResult UploadFile()
        {
            List<UploadFilesResultViewModel> statuses = new List<UploadFilesResultViewModel>();
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionProduct"), "ProjectSectionProduct", "/ProjectDevision/ProjectSectionProduct/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionProductFiles));

            if (result.Key == 3)
            {
                JsonResult r = Json(new { files = statuses });
                r.ContentType = "text/plain";

                return r;
            }

            return Json(new { files = "{'name': 'تصویر'}, {'size': 0}, {'error': '" + result.Value + "'}" });
        }

        [AllowAnonymous]
        public JsonResult DeleteFile(Int64 Id)
        {
            var file = _fileService.Get(i => i.Id == Id).FirstOrDefault();
            if (file.FileState == 1)
            {
                Utilities.AppCode.CheckUploadFile.DeleteFile(Server.MapPath(file.Address));
                _fileService.Delete(file);
            }
            else
            {
                file.FileState = 3;
                _fileService.Update(file);
            }
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            _uow.SaveChanges(Member.Id);

            return Json(new { files = new JavaScriptSerializer().Serialize("{'" + file.Title + "': true}") });
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetProjectSectionProductFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionProductFileService.GetByProjectSectionProductId(Member.Id, projectSectionId)) });
        }


        #endregion
	}
}