using DatabaseContext.Context;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDevision.ProjectSectionComplementarityPriceViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionComplementarityPriceController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;
        IProjectSectionComplementarityPriceService _ProjectSectionComplementarityPriceService;
        IProjectSectionComplementarityPriceFileService _ProjectSectionComplementarityPriceFileService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionComplementarityPriceController(IUnitOfWork uow, IProjectSectionService projectSectionService, IProjectSectionComplementarityPriceService ProjectSectionComplementarityPriceService, IProjectSectionComplementarityPriceFileService ProjectSectionComplementarityPriceFileService, IFileService fileService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
            _ProjectSectionComplementarityPriceService = ProjectSectionComplementarityPriceService;
            _ProjectSectionComplementarityPriceFileService = ProjectSectionComplementarityPriceFileService;
            _fileService = fileService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionComplementarityPrice/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectSectionComplementarityPriceViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionComplementarityPriceService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create(long Id)
        {
            CreateProjectSectionComplementarityPriceViewModel cpsivm = new CreateProjectSectionComplementarityPriceViewModel();
            cpsivm = _ProjectSectionComplementarityPriceService.GetContractPrice(Id);

            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionComplementarityPriceViewModel cpsivm)
        {
            try
            {
                //if (!ModelState.IsValid || !Request.IsAjaxRequest())
                //    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                if(cpsivm.ContractPrice == 0)
                    return Json(new { type = AlarmType.danger.ToString(), message = "مبلغ قرارداد 0 میباشد و امکان افزایش و کاهش مبلغ مقدور نیست" });

                CreateProjectSectionComplementarityPriceViewModel cpsivm2 = new CreateProjectSectionComplementarityPriceViewModel();
                cpsivm2 = _ProjectSectionComplementarityPriceService.GetContractPrice(cpsivm.ProjectSectionComplementarityPrice.ProjectSectionId.Value);
                if ((cpsivm2.TotalDPercent + cpsivm.ProjectSectionComplementarityPrice.DecreasePercent) > 25 || (cpsivm2.TotalIPercent + cpsivm.ProjectSectionComplementarityPrice.IncreasePercent) > 25)
                    return Json(new { type = AlarmType.danger.ToString(), message = "مجموع درصد کاهش و افزایش نباید بیشتر از 25 % باشد" });



                long? isInsert = cpsivm.ProjectSectionComplementarityPrice.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _ProjectSectionComplementarityPriceService.Save(cpsivm, Member.Id, paths);

                if (result.Key > 0)
                {
                    foreach (string item in paths)
                    {
                        CheckUploadFile.DeleteFile(Server.MapPath(item));
                    }

                    if (isInsert == -1)
                        return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessInsert) });
                    else
                        return Json(new { type = AlarmType.info.ToString(), message = Messages.GetMsg(MsgKey.SuccessUpdate) });
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
                CreateProjectSectionComplementarityPriceViewModel cpsivm = new CreateProjectSectionComplementarityPriceViewModel();
                cpsivm.ProjectSectionComplementarityPrice = _ProjectSectionComplementarityPriceService.Find(id);

                CreateProjectSectionComplementarityPriceViewModel cpsivm2 = new CreateProjectSectionComplementarityPriceViewModel();
                cpsivm2 = _ProjectSectionComplementarityPriceService.GetContractPrice(cpsivm.ProjectSectionComplementarityPrice.ProjectSectionId.Value);

                cpsivm.ContractPrice = cpsivm2.ContractPrice;
                cpsivm.TotalDPercent = cpsivm2.TotalDPercent;
                cpsivm.TotalIPercent = cpsivm2.TotalIPercent;

                ReloadFiles();

                foreach (var item in _ProjectSectionComplementarityPriceFileService.Get(i => i.ProjectSectionComplementarityPriceId == id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionComplementarityPriceViewModel());
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

                List<string> paths = new List<string>();

                foreach (var item in _ProjectSectionComplementarityPriceFileService.Get(i => i.ProjectSectionComplementarityPriceId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _ProjectSectionComplementarityPriceFileService.Delete(item);
                }
                
                _ProjectSectionComplementarityPriceService.Delete(id);

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
                    foreach (string item in paths)
                    {
                        Utilities.AppCode.CheckUploadFile.DeleteFile(Server.MapPath(item));
                    }

                    return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessDelete) });
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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionComplementarityPrice"), "ProjectSectionComplementarityPrice", "/ProjectDevision/ProjectSectionComplementarityPrice/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionComplementarityPriceFiles));

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
        public JsonResult GetProjectSectionComplementarityPriceFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionComplementarityPriceFileService.GetByProjectSectionComplementarityPriceId(Member.Id, projectSectionId)) });
        }


        #endregion
    }
}