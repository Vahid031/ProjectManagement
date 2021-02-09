using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;
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
using ViewModels.ProjectDevision.ProjectSectionPaidPriceViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionPaidPriceController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IOpinionService _opinionService;
        ITempFixedService _tempFixedService;
        IStatementTypeService _statementTypeService;
        IStatementNumberService _statementNumberService;
        IProjectSectionService _projectSectionService;
        IProjectSectionPaidPriceService _ProjectSectionPaidPriceService;
        IProjectSectionDraftService _projectSectionDraftService;
        IDeductionPercentService _deductionPercentService;
        IProjectSectionPaidPriceFileService _ProjectSectionPaidPriceFileService;
        IFileService _fileService;
        IPermissionService _permissionService;

        public ProjectSectionPaidPriceController(IUnitOfWork uow, 
            IStatementNumberService statementNumberService, 
            IStatementTypeService statementTypeService, 
            ITempFixedService tempFixedService, 
            IOpinionService opinionService, 
            IDefectService defectService, 
            IProjectSectionService projectSectionService, 
            IProjectSectionPaidPriceService ProjectSectionPaidPriceService, 
            IProjectSectionPaidPriceFileService ProjectSectionPaidPriceFileService,
            IFileService fileService, 
            IPermissionService permissionService,
            IProjectSectionDraftService projectSectionDraftService,
            IDeductionPercentService deductionPercentService)
        {
            _uow = uow;

            _statementTypeService = statementTypeService;
            _statementNumberService = statementNumberService;
            _opinionService = opinionService;
            _tempFixedService = tempFixedService;
            _projectSectionService = projectSectionService;
            _ProjectSectionPaidPriceService = ProjectSectionPaidPriceService;
            _ProjectSectionPaidPriceFileService = ProjectSectionPaidPriceFileService;
            _fileService = fileService;
            _permissionService = permissionService;
            _projectSectionDraftService = projectSectionDraftService;
            _deductionPercentService = deductionPercentService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionPaidPrice/_Index", Member.RoleId);
            
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
        public JsonResult _List(ListProjectSectionPaidPriceViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionPaidPriceService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetStatementNumbersByTempFixedId(Int64 tempFixedId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_statementNumberService.Get(i => i.TempFixedId == tempFixedId).Select(i => new { i.Id, i.Title }).ToList()) });
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetInsurances(int insuranceType, Int64 price)
        {
            List<DeductionPercent> _DeductionPercent = _deductionPercentService.Get().ToList();

            Int64 contractInsurance = 0;
            Int64 masterInsurance = 0;

            if (insuranceType == 1) // بیمه اعتبارات تملک و داراییها
            {
                contractInsurance = (Int64)((price * _DeductionPercent[3].PercentAmount.Value) / 100);
                masterInsurance = (Int64)((price * _DeductionPercent[4].PercentAmount.Value) / 100);
            }

            else if (insuranceType == 2) // بیمه دستمزدی با مصالح
            {
                contractInsurance = (Int64)((price * _DeductionPercent[5].PercentAmount.Value) / 100);
                masterInsurance = (Int64)((price * _DeductionPercent[6].PercentAmount.Value) / 100);
            }
            else if (insuranceType == 3) // بیمه دستمزدی بدون مصالح
            {
                contractInsurance = (Int64)((price * _DeductionPercent[7].PercentAmount.Value) / 100);
                masterInsurance = (Int64)((price * _DeductionPercent[8].PercentAmount.Value) / 100);
            }

            return Json(new { contractInsurance = contractInsurance, masterInsurance = masterInsurance });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create(long Id)
        {
            CreateProjectSectionPaidPriceViewModel cpsivm = new CreateProjectSectionPaidPriceViewModel();
            ProjectSectionDraft _ProjectSectionDraft = _projectSectionDraftService.Get(c => c.Id == Id).FirstOrDefault();
            long price = _ProjectSectionDraft.DraftPrice.Value;
            List<DeductionPercent> _DeductionPercent = _deductionPercentService.Get().ToList();






            Int64 tax = (Int64)((price * _DeductionPercent[0].PercentAmount.Value) / 100);
            Int64 valueTax = (Int64)((price * _DeductionPercent[1].PercentAmount.Value) / 100);
            Int64 deposit = (Int64)((price * _DeductionPercent[2].PercentAmount.Value) / 100);
            Int64 contractInsurance = (Int64)((price * _DeductionPercent[3].PercentAmount.Value) / 100);
            Int64 masterInsurance = (Int64)((price * _DeductionPercent[4].PercentAmount.Value) / 100);

            cpsivm.ProjectSectionPaidPrice = new ProjectSectionPaidPrice
            {
                DraftPrice = _ProjectSectionDraft.DraftPrice,
                DraftDate_ = ConvertDate.GetShamsi(_ProjectSectionDraft.DraftDate),
                PaidDate_ = ConvertDate.GetShamsi(DateTime.Now),
                DraftNumber = _ProjectSectionDraft.DraftNumber,
                MasterInsurance = masterInsurance, // بیمه سهم کارفرما
                ContractorInsurance = contractInsurance, // بیمه سهم پیمانکار
                Tax = tax, // مالیات
                ValueTax = valueTax, // مالیات بر ارزش افزوده
                Deposit = deposit // سپرده
            };
            

            
           

            ReloadFiles();

            ViewBag.Id = "-1";
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionPaidPriceViewModel cpsivm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                // این کد چک می کند که مبلغ پرداختی کمتر از مبلغ قرارداد باشد
               if(!_ProjectSectionPaidPriceService.CalculateCanInsert(cpsivm.ProjectSectionPaidPrice.ProjectSectionId.Value, cpsivm.ProjectSectionPaidPrice.PaidPrice.Value))
                   return Json(new { type = AlarmType.danger.ToString(), message = "مبلغ پرداختی بیشتر از مبلغ قرارداد است" });

                long? isInsert = cpsivm.ProjectSectionPaidPrice.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                List<string> paths = new List<string>();

                KeyValuePair<Int64, string> result = _ProjectSectionPaidPriceService.Save(cpsivm, Member.Id, paths);

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
                CreateProjectSectionPaidPriceViewModel cpsivm = new CreateProjectSectionPaidPriceViewModel();
                cpsivm.ProjectSectionPaidPrice = _ProjectSectionPaidPriceService.Find(id);

                ReloadFiles();

                foreach (var item in _ProjectSectionPaidPriceFileService.Get(i => i.ProjectSectionPaidPriceId == id).ToList())
                {
                    cpsivm.Files += item.FileId + ",";
                }

                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionPaidPriceViewModel());
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

                foreach (var item in _ProjectSectionPaidPriceFileService.Get(i => i.ProjectSectionPaidPriceId == id).ToList())
                {
                    paths.Add(_fileService.Get(i => i.Id == item.FileId).First().Address);

                    _fileService.Delete(item.FileId);
                    _ProjectSectionPaidPriceFileService.Delete(item);
                }
                
                _ProjectSectionPaidPriceService.Delete(id);

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

            KeyValuePair<Int64, string> result = Utilities.AppCode.CheckUploadFile.Upload(Request.Files[0], statuses, Member.Id, Server.MapPath("/Uploads/ProjectSectionPaidPrice"), "ProjectSectionPaidPrice", "/ProjectDevision/ProjectSectionPaidPrice/DeleteFile", Convert.ToInt64(GeneralEnums.FileTypes.ProjectSectionPaidPriceFiles));

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
        public JsonResult GetProjectSectionPaidPriceFiles(Int64 projectSectionId)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionPaidPriceFileService.GetByProjectSectionPaidPriceId(Member.Id, projectSectionId)) });
        }


        #endregion



        
    }
}