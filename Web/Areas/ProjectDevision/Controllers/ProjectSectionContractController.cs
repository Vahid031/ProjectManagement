using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDevision.ProjectSectionContractViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionContractController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IContractTypeService _contractTypeService;
        IUnitService _unitService;
        IProjectSectionService _projectSectionService;
        IProjectSectionContractService _projectSectionContractService;
        IProjectSectionContractFileService _projectSectionContractFileService;
        IProjectSectionContractDetailService _projectSectionContractDetailService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionContractController(IUnitOfWork uow, IContractTypeService contractTypeService, IUnitService unitService, IProjectSectionService projectSectionService, IProjectSectionContractService projectSectionContractService, IProjectSectionContractFileService projectSectionContractFileService, IProjectSectionContractDetailService projectSectionContractDetailService, IFileService fileService, IPermissionService permissionService)
        {
            _uow = uow;

            _contractTypeService = contractTypeService;
            _unitService = unitService;
            _projectSectionService = projectSectionService;
            _projectSectionContractService = projectSectionContractService;
            _projectSectionContractFileService = projectSectionContractFileService;
            _projectSectionContractDetailService = projectSectionContractDetailService;
            _fileService = fileService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index(Int64 Id)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionContract/_Index/" + Id, Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create(Int64 Id)
        {
            CreateProjectSectionContractViewModel cpsivm = new CreateProjectSectionContractViewModel();

            cpsivm.ContractTypes = new List<ContractType>() { new ContractType() { Title = "انتخاب کنید..." } };
            cpsivm.ContractTypes.AddRange(_contractTypeService.Get().ToList());

            cpsivm.Units = new List<Unit>() { new Unit() { Title = "انتخاب کنید..." } };
            cpsivm.Units.AddRange(_unitService.Get().ToList());

            var result = _projectSectionContractService.Get(i => i.ProjectSectionId == Id).FirstOrDefault();

            if (result != null)
            {
                cpsivm.ProjectSectionContract = result;

                foreach (var item in _projectSectionContractFileService.Get(i => i.ProjectSectionContractId == result.Id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                foreach (var item in _projectSectionContractDetailService.Get(i => i.ProjectSectionContractId == result.Id).ToList())
                {
                    cpsivm.ContractDetails += item.ProjectDetails + "|";
                    cpsivm.ContractDetails += item.TotalPrice + "|";
                    cpsivm.ContractDetails += item.TotalAmonts + "|";
                    cpsivm.ContractDetails += (item.UnitId == null ? -1 : item.UnitId) + "|";
                    cpsivm.ContractDetails += (item.UnitId == null ? "" : item.Unit.Title) + "|";
                    cpsivm.ContractDetails += item.UnitPrice + "|";
                    cpsivm.ContractDetails += "#";
                }

                ViewBag.Id = result.Id;
            }
            else
            {
                ViewBag.Id = "-1";
            }

            ReloadFiles();
            
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionContractViewModel cpsivm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = cpsivm.ProjectSectionContract.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _projectSectionContractService.Save(cpsivm, Member.Id, paths);

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
                CreateProjectSectionContractViewModel cpsivm = new CreateProjectSectionContractViewModel();
                cpsivm.ProjectSectionContract = _projectSectionContractService.Find(id);

                ReloadFiles();

                foreach (var item in _projectSectionContractFileService.Get(i => i.ProjectSectionContractId == id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionContractViewModel());
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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionContract"), "ProjectSectionContract", "/ProjectDevision/ProjectSectionContract/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionContractFiles));

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
        public JsonResult GetProjectSectionContractFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionContractFileService.GetByProjectSectionContractId(Member.Id, projectSectionId)) });
        }


        #endregion


        #region Get Date

        [AllowAnonymous]
        public JsonResult GetDate(string beginDate, string duration, string endDate, int which) 
        {
            //which: 1= Get beginDate, 2= Get duration, 3= Get endDate
            if (which == 1)
            {
                if (!string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(duration))
                {
                    DateTime endDt = Infrastracture.AppCode.ConvertDate.GetMiladi(endDate).Value;
                    beginDate = Infrastracture.AppCode.ConvertDate.GetShamsi(endDt.AddMonths(Convert.ToInt32(duration)));
                    duration = Math.Abs(Convert.ToInt64(duration)).ToString();
                }
                else
                {
                    return Json(new { beginDate = beginDate, duration = duration, endDate = endDate });        
                }
            }

            if (which == 2)
            {
                if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(endDate))
                {
                    DateTime beginDt = Infrastracture.AppCode.ConvertDate.GetMiladi(beginDate).Value;
                    DateTime endDt = Infrastracture.AppCode.ConvertDate.GetMiladi(endDate).Value;
                    duration = (endDt.Subtract(beginDt).Days / 30).ToString();
                }
                else
                {
                    return Json(new { beginDate = beginDate, duration = duration, endDate = endDate });
                }
            }

            if (which == 3)
            {
                if (!string.IsNullOrEmpty(beginDate) && !string.IsNullOrEmpty(duration))
                {
                    DateTime beginDt = Infrastracture.AppCode.ConvertDate.GetMiladi(beginDate).Value;
                    endDate = Infrastracture.AppCode.ConvertDate.GetShamsi(beginDt.AddMonths(Convert.ToInt32(duration)));
                }
                else
                {
                    return Json(new { beginDate = beginDate, duration = duration, endDate = endDate });
                }
            }

            DateTime dt1 = Infrastracture.AppCode.ConvertDate.GetMiladi(beginDate).Value;
            DateTime dt2 = Infrastracture.AppCode.ConvertDate.GetMiladi(endDate).Value;

            if (dt1.CompareTo(dt2) > 0)
            {
                endDate = beginDate;
                duration = "0";
            }

            return Json(new { beginDate = beginDate, duration = duration, endDate = endDate });
        }

        #endregion
    }
}