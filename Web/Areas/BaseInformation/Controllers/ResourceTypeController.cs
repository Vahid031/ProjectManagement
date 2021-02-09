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
using ViewModels.BaseInformation.ResourceTypeViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.BaseInformation.Controllers
{
    public class ResourceTypeController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IResourceTypeService _resourceTypeService;
        IPermissionService _permissionService;

        public ResourceTypeController(IUnitOfWork uow, IResourceTypeService resourceTypeService, IPermissionService permissionService)
        {
            _uow = uow;

            _resourceTypeService = resourceTypeService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/BaseInformation/ResourceType/_Index" , Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult TitleValidation([Bind(Prefix = "ResourceType.Title")]string value, [Bind(Prefix = "ResourceType.ResourceId")]long resourceId, [Bind(Prefix = "ResourceType.Id")]long id)
        {
            List<ResourceType> item;

            if (id == -1)
                item = _resourceTypeService.Get(c => c.Title == value && c.ResourceId == resourceId).ToList();
            else
                item = _resourceTypeService.Get(c => c.Title == value && c.Id != id && c.ResourceId == resourceId).ToList();


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
        public JsonResult _List(ListResourceTypeViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_resourceTypeService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetResourceTypesByResourceId(Int64 resourceId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_resourceTypeService.Get(i => i.ResourceId == resourceId).Select(i => new { i.Id, i.Title }).ToList()) });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            CreateResourceTypeViewModel catvm = new CreateResourceTypeViewModel();
            
            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateResourceTypeViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ResourceType.Id;

                if (isInsert == -1)
                {
                    _resourceTypeService.Insert(catvm.ResourceType);
                }
                else
                {
                    _resourceTypeService.Update(catvm.ResourceType);
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
                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                CreateResourceTypeViewModel catvm = new CreateResourceTypeViewModel();
                catvm.ResourceType = _resourceTypeService.Find(id);

                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateResourceTypeViewModel());
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

                _resourceTypeService.Delete(id);

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