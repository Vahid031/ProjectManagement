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
using ViewModels.BaseInformation.VillageViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.BaseInformation.Controllers
{
    public class VillageController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IVillageService _villageService;
        ISectionService _sectionService;
        ICityService _cityService;
        IStateService _stateService;
        IPermissionService _permissionService;

        public VillageController(IUnitOfWork uow, IVillageService villageService, ISectionService sectionService, ICityService cityService, IStateService stateService, IPermissionService permissionService)
        {
            _uow = uow;

            _villageService = villageService;
            _sectionService = sectionService;
            _cityService = cityService;
            _stateService = stateService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/BaseInformation/Village/_Index" , Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult CodeValidation([Bind(Prefix = "Village.Code")]string value, [Bind(Prefix = "Village.Id")]long id, [Bind(Prefix = "Village.SectionId")]long sectionId)
        {
            List<Village> item;

            if (id == -1)
                item = _villageService.Get(c => c.Code == value && c.SectionId == sectionId).ToList();
            else
                item = _villageService.Get(c => c.Code == value && c.Id != id && c.SectionId == sectionId).ToList();


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
        public JsonResult TitleValidation([Bind(Prefix = "Village.Title")]string value, [Bind(Prefix = "Village.Id")]long id, [Bind(Prefix = "Village.SectionId")]long sectionId)
        {
            List<Village> item;

            if (id == -1)
                item = _villageService.Get(c => c.Title == value && c.SectionId == sectionId).ToList();
            else
                item = _villageService.Get(c => c.Title == value && c.Id != id && c.SectionId == sectionId).ToList();


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
        public JsonResult _List(ListVillageViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_villageService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetVillagesBySectionId(Int64 sectionId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_villageService.Get(i => i.SectionId == sectionId).Select(i => new { i.Id, i.Title }).ToList()) });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateVillageViewModel catvm = new CreateVillageViewModel();
            catvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            catvm.States.AddRange(_stateService.Get().ToList());

            catvm.Cities = new List<City>();

            catvm.Sections = new List<Section>();

            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateVillageViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.Village.Id;

                if (isInsert == -1)
                {
                    _villageService.Insert(catvm.Village);
                }
                else
                {
                    _villageService.Update(catvm.Village);
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
                CreateVillageViewModel catvm = new CreateVillageViewModel();
                catvm.Village = _villageService.Find(id);
                catvm.CityId = _sectionService.Get(i => i.Id == catvm.Village.SectionId).FirstOrDefault().CityId.Value;
                catvm.StateId = _cityService.Get(i => i.Id == catvm.CityId).FirstOrDefault().StateId.Value;

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
                return PartialView("_Create", new CreateVillageViewModel());
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

                _villageService.Delete(id);

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