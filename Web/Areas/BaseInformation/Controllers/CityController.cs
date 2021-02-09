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
using ViewModels.BaseInformation.CityViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.BaseInformation.Controllers
{
    public class CityController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        ICityService _cityService;
        IStateService _stateService;
        IPermissionService _permissionService;
        public CityController(IUnitOfWork uow, ICityService cityService, IStateService stateService, IPermissionService permissionService)
        {
            _uow = uow;
            _cityService = cityService;
            _stateService = stateService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/BaseInformation/City/_Index" , Member.RoleId);
            var test = ViewBag.Permission;
            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult CodeValidation([Bind(Prefix = "City.Code")]string value, [Bind(Prefix = "City.Id")]long id, [Bind(Prefix = "City.StateId")]long stateId)
        {
            List<City> item;

            if (id == -1)
                item = _cityService.Get(c => c.Code == value && c.StateId == stateId).ToList();
            else
                item = _cityService.Get(c => c.Code == value && c.Id != id && c.StateId == stateId).ToList();


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
        public JsonResult TitleValidation([Bind(Prefix = "City.Title")]string value, [Bind(Prefix = "City.Id")]long id, [Bind(Prefix = "City.StateId")]long stateId)
        {
            List<City> item;

            if (id == -1)
                item = _cityService.Get(c => c.Title == value && c.StateId == stateId).ToList();
            else
                item = _cityService.Get(c => c.Title == value && c.Id != id && c.StateId == stateId).ToList();


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
        public JsonResult _List(ListCityViewModel latvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_cityService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetCitiesByStateId(Int64 stateId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_cityService.Get(i => i.StateId == stateId).Select(i => new { i.Id, i.Title }).ToList()) });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateCityViewModel catvm = new CreateCityViewModel();
            catvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            catvm.States.AddRange(_stateService.Get().ToList());           
            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateCityViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.City.Id;

                if (isInsert == -1)
                {
                    _cityService.Insert(catvm.City);
                }
                else
                {
                    _cityService.Update(catvm.City);
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
                CreateCityViewModel catvm = new CreateCityViewModel();
                catvm.City = _cityService.Find(id);

                catvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
                catvm.States.AddRange(_stateService.Get().ToList());

                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateCityViewModel());
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

                _cityService.Delete(id);

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