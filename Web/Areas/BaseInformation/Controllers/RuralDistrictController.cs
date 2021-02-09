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
using ViewModels.BaseInformation.RuralDistrictViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.BaseInformation.Controllers
{
    public class RuralDistrictController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IRuralDistrictService _ruralDistrictService;
        IVillageService _villageService;
        ISectionService _sectionService;
        ICityService _cityService;
        IStateService _stateService;
        IPermissionService _permissionService;

        public RuralDistrictController(IUnitOfWork uow, IRuralDistrictService ruralDistrictService, IVillageService villageService, ISectionService sectionService, ICityService cityService, IStateService stateService, IPermissionService permissionService)
        {
            _uow = uow;

            _ruralDistrictService = ruralDistrictService;
            _villageService = villageService;
            _sectionService = sectionService;
            _cityService = cityService;
            _stateService = stateService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/BaseInformation/RuralDistrict/_Index" , Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult CodeValidation([Bind(Prefix = "RuralDistrict.Code")]string value, [Bind(Prefix = "RuralDistrict.Id")]long id, [Bind(Prefix = "RuralDistrict.VillageId")]long villageId)
        {
            List<RuralDistrict> item;

            if (id == -1)
                item = _ruralDistrictService.Get(c => c.Code == value && c.VillageId == villageId).ToList();
            else
                item = _ruralDistrictService.Get(c => c.Code == value && c.Id != id && c.VillageId == villageId).ToList();


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
        public JsonResult TitleValidation([Bind(Prefix = "RuralDistrict.Title")]string value, [Bind(Prefix = "RuralDistrict.Id")]long id, [Bind(Prefix = "RuralDistrict.VillageId")]long villageId)
        {
            List<RuralDistrict> item;

            if (id == -1)
                item = _ruralDistrictService.Get(c => c.Title == value && c.VillageId == villageId).ToList();
            else
                item = _ruralDistrictService.Get(c => c.Title == value && c.Id != id && c.VillageId == villageId).ToList();


            if (item.Count() != 0)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult _List(ListRuralDistrictViewModel latvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ruralDistrictService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetRuralDistrictByVillageId(Int64 villageId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_ruralDistrictService.Get(i => i.VillageId == villageId).Select(i => new { i.Id, i.Title }).ToList()) });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateRuralDistrictViewModel catvm = new CreateRuralDistrictViewModel();
            catvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            catvm.States.AddRange(_stateService.Get().ToList());

            catvm.Cities = new List<City>();
            catvm.Sections = new List<Section>();
            catvm.Villages = new List<Village>();

            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateRuralDistrictViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.RuralDistrict.Id;

                if (isInsert == -1)
                {
                    _ruralDistrictService.Insert(catvm.RuralDistrict);
                }
                else
                {
                    _ruralDistrictService.Update(catvm.RuralDistrict);
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
                CreateRuralDistrictViewModel catvm = new CreateRuralDistrictViewModel();
                catvm.RuralDistrict = _ruralDistrictService.Find(id);
                catvm.SectionId = _villageService.Get(i => i.Id == catvm.RuralDistrict.VillageId).FirstOrDefault().SectionId.Value;
                catvm.CityId = _sectionService.Get(i => i.Id == catvm.SectionId).FirstOrDefault().CityId.Value;
                catvm.StateId = _cityService.Get(i => i.Id == catvm.CityId).FirstOrDefault().StateId.Value;

                catvm.Villages = new List<Village>() { new Village() { Title = "انتخاب کنید..." } };
                catvm.Villages.AddRange(_villageService.Get(i => i.SectionId == catvm.SectionId).ToList());

                catvm.Sections = new List<Section>() { new Section() { Title = "انتخاب کنید..." } };
                catvm.Sections.AddRange(_sectionService.Get(i => i.CityId == catvm.CityId).ToList());

                catvm.Cities = new List<City>() { new City() { Title = "انتخاب کنید..." } };
                catvm.Cities.AddRange(_cityService.Get(i => i.StateId == catvm.StateId).ToList());

                catvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
                catvm.States.AddRange(_stateService.Get().ToList());

                
                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateRuralDistrictViewModel());
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

                _ruralDistrictService.Delete(id);

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
	}
}