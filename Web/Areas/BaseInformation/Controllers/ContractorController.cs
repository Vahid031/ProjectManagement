using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.BaseInformation.ContractorViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.BaseInformation.Controllers
{
    public class ContractorController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IContractorService _contractorService;
        ILevelTypeService _levelTypeService;
        IMajorTypeService _majorTypeService;
        IContractorLevelService _contractorLevelService;
        IPermissionService _permissionService;

        public ContractorController(
            IUnitOfWork uow, 
            IContractorService contractorService, 
            IPermissionService permissionService,
            IMajorTypeService majorTypeService,
            IContractorLevelService contractorLevelService,
            ILevelTypeService levelTypeService)
        {
            _uow = uow;

            _contractorService = contractorService;
            _levelTypeService = levelTypeService;
            _majorTypeService = majorTypeService;
            _contractorLevelService = contractorLevelService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/BaseInformation/Contractor/_Index" , Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult CompanyNameValidation([Bind(Prefix = "Contractor.CompanyName")]string value, [Bind(Prefix = "Contractor.Id")]long id)
        {
            var item = _contractorService.Get(c => c.CompanyName == value && c.Id != id).ToList();

            if (item.Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [AllowAnonymous]
        public JsonResult ManagerNationalCodeValidation([Bind(Prefix = "Contractor.ManagerNationalCode")]string value)
        {
            if (value == null)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            string strValue = value.ToString().Trim();
            if (strValue == string.Empty)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            string meli_code = "";
            meli_code = value.ToString();
            if (meli_code.Length == 10)
            {
                if (meli_code == "1111111111" || meli_code == "0000000000" || meli_code == "2222222222" || meli_code == "3333333333" || meli_code == "4444444444" || meli_code == "5555555555" || meli_code == "6666666666" || meli_code == "7777777777" || meli_code == "8888888888" || meli_code == "9999999999")
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                char[] meli_code_chars = meli_code.ToCharArray();
                int c = Convert.ToInt32(meli_code_chars[9].ToString());
                int n = Convert.ToInt32(meli_code_chars[0].ToString()) * 10 + Convert.ToInt32(meli_code_chars[1].ToString()) * 9 + Convert.ToInt32(meli_code_chars[2].ToString()) * 8 + Convert.ToInt32(meli_code_chars[3].ToString()) * 7 + Convert.ToInt32(meli_code_chars[4].ToString()) * 6 + Convert.ToInt32(meli_code_chars[5].ToString()) * 5 + Convert.ToInt32(meli_code_chars[6].ToString()) * 4 + Convert.ToInt32(meli_code_chars[7].ToString()) * 3 + Convert.ToInt32(meli_code_chars[8].ToString()) * 2;
                int r = n - (n / 11) * 11;
                if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r))
                {
                    ;
                }
                else
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            else if (meli_code.Length > 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);
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
        public JsonResult _List(ListContractorViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_contractorService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateContractorViewModel ccvm = new CreateContractorViewModel();
            
            ViewBag.Id = "-1";
            return PartialView(ccvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateContractorViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                catvm.Contractor.RegistrationDate = ConvertDate.GetMiladi(catvm.Contractor.RegistrationDate_);


                long? isInsert = catvm.Contractor.Id;

                if (isInsert == -1)
                {
                    _contractorService.Insert(catvm.Contractor);
                }
                else
                {
                    _contractorService.Update(catvm.Contractor);

                    foreach (var item in _contractorLevelService.Get(i => i.ContractorId == catvm.Contractor.Id))
                    {
                        _contractorLevelService.Delete(item);
                    }
                }

                if (!String.IsNullOrEmpty(catvm.ContractorLevels))
                {
                    foreach (var item in catvm.ContractorLevels.Split('|'))
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            ContractorLevel cl = new ContractorLevel();
                            cl.ContractorId = catvm.Contractor.Id;
                            cl.MajorTypeId = Convert.ToInt64(item.Split(',')[0].Split(':')[1]);
                            cl.LevelTypeId = Convert.ToInt64(item.Split(',')[2].Split(':')[1]);
                            cl.FinishCreditDate = item.Split(',')[4].Split(':')[1];

                            _contractorLevelService.Insert(cl);
                        }
                    }
                }

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
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
                CreateContractorViewModel ccvm = new CreateContractorViewModel();
                ccvm.Contractor = _contractorService.Find(id);
                ccvm.Contractor.RegistrationDate_ = ConvertDate.GetShamsi(ccvm.Contractor.RegistrationDate);

                return PartialView("_Create", ccvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateContractorViewModel());
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

                foreach (var item in _contractorLevelService.Get(i => i.ContractorId == id))
                {
                    _contractorLevelService.Delete(item);
                }

                _contractorService.Delete(id);

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                    return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessDelete) });
                else
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorDelete) });
            }
            catch (Exception)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }

        #endregion


        #region ContractorLevels

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _Level()
        {
            CreateContractorViewModel ccvm = new CreateContractorViewModel();

            ccvm.LevelTypes = new List<LevelType>() { new LevelType() { Title = "انتخاب کنید..." } };
            ccvm.LevelTypes.AddRange(_levelTypeService.Get().ToList());

            ccvm.MajorTypes = new List<MajorType>() { new MajorType() { Title = "انتخاب کنید..." } };
            ccvm.MajorTypes.AddRange(_majorTypeService.Get().ToList());


            return PartialView(ccvm);
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult GetContractorLevelsByContractorId(Int64 contractorId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_contractorLevelService.Get(i => i.ContractorId == contractorId).Select(i => new { i.Id, i.ContractorId, i.LevelTypeId, LevelTypeTitle = i.LevelType.Title, i.MajorTypeId, MajorTypeTitle = i.MajorType.Title, i.FinishCreditDate }).ToList()) });
        }

        #endregion
    }
}