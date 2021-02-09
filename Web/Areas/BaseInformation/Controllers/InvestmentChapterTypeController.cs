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
using ViewModels.BaseInformation.InvestmentChapterTypeViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.BaseInformation.Controllers
{
    public class InvestmentChapterTypeController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IInvestmentChapterTypeService _investmentChapterTypeService;
        IPermissionService _permissionService;

        public InvestmentChapterTypeController(IUnitOfWork uow, IInvestmentChapterTypeService investmentChapterTypeService, IPermissionService permissionService)
        {
            _uow = uow;

            _investmentChapterTypeService = investmentChapterTypeService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/BaseInformation/InvestmentChapterType/_Index" , Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult TitleValidation([Bind(Prefix = "InvestmentChapterType.Title")]string value, [Bind(Prefix = "InvestmentChapterType.InvestmentChapterId")]long investmentChapterId, [Bind(Prefix = "InvestmentChapterType.Id")]long id)
        {
            List<InvestmentChapterType> item;

            if (id == -1)
                item = _investmentChapterTypeService.Get(c => c.Title == value && c.InvestmentChapterId == investmentChapterId).ToList();
            else
                item = _investmentChapterTypeService.Get(c => c.Title == value && c.Id != id && c.InvestmentChapterId == investmentChapterId).ToList();


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
        public JsonResult _List(ListInvestmentChapterTypeViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_investmentChapterTypeService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetInvestmentChapterTypesByInvestmentChapterId(Int64 investmentChapterId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_investmentChapterTypeService.Get(i => i.InvestmentChapterId == investmentChapterId).Select(i => new { i.Id, i.Title }).ToList()) });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            CreateInvestmentChapterTypeViewModel catvm = new CreateInvestmentChapterTypeViewModel();
            
            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateInvestmentChapterTypeViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.InvestmentChapterType.Id;

                if (isInsert == -1)
                {
                    _investmentChapterTypeService.Insert(catvm.InvestmentChapterType);
                }
                else
                {
                    _investmentChapterTypeService.Update(catvm.InvestmentChapterType);
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

                CreateInvestmentChapterTypeViewModel catvm = new CreateInvestmentChapterTypeViewModel();
                catvm.InvestmentChapterType = _investmentChapterTypeService.Find(id);

                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateInvestmentChapterTypeViewModel());
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

                _investmentChapterTypeService.Delete(id);

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