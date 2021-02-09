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
using ViewModels.BaseInformation.FinantialYearViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.BaseInformation.Controllers
{
    public class FinantialYearController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IFinantialYearService _finantialYearService;
        IPermissionService _permissionService;

        public FinantialYearController(IUnitOfWork uow, IFinantialYearService finantialYearService, IPermissionService permissionService)
        {
            _uow = uow;

            _finantialYearService = finantialYearService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/BaseInformation/FinantialYear/_Index" , Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult TitleValidation([Bind(Prefix = "FinantialYear.Title")]string value, [Bind(Prefix = "FinantialYear.Id")]long id)
        {
            var item = _finantialYearService.Get(c => c.Title == value && c.Id != id).ToList();

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
        public JsonResult _List(ListFinantialYearViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_finantialYearService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            CreateFinantialYearViewModel catvm = new CreateFinantialYearViewModel();
            
            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateFinantialYearViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                long? isInsert = catvm.FinantialYear.Id;


                if (catvm.FinantialYear.IsActive == true)
                {
                    var finantialYears = _finantialYearService.Get().ToList();

                    foreach (var item in finantialYears)
                    {
                        item.IsActive = false;
                        _finantialYearService.Update(item);
                        _uow.SaveChanges(Member.Id);
                        _uow.Entry<FinantialYear>(item).State = System.Data.Entity.EntityState.Detached;
                    }

                }

                if (isInsert == -1)
                {
                    _finantialYearService.Insert(catvm.FinantialYear);
                }
                else
                {
                    _finantialYearService.Update(catvm.FinantialYear);
                }


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
                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                CreateFinantialYearViewModel catvm = new CreateFinantialYearViewModel();
                catvm.FinantialYear = _finantialYearService.Find(id);

                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateFinantialYearViewModel());
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

                _finantialYearService.Delete(id);

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