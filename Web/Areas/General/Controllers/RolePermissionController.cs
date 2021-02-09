using DatabaseContext.Context;
using Newtonsoft.Json;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.General.RoleViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.General.Controllers
{
    public class RolePermissionController : Controller
    {
        #region Index And ctor

		private readonly IUnitOfWork _uow;
        private readonly IRoleService _roleService;
        private readonly IPermissionService _permissionService;
        private readonly IRolePermissionService _rolePermissionService;
		private readonly IMemberService _memberService;

        public RolePermissionController(
			IUnitOfWork uow,
			IRoleService roleService,
            IPermissionService permissionService,
            IRolePermissionService rolePermissionService,
			IMemberService memberService)
		{
			_uow = uow;
			_roleService = roleService;
            _permissionService = permissionService;
            _rolePermissionService = rolePermissionService;
			_memberService = memberService;
		}

        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/General/RolePermission/_Index", Member.RoleId);

            ViewBag.Level = 3;
            return PartialView();
        }

		#endregion


		#region _List

        [HttpGet]
        public PartialViewResult _List()
        {
            return PartialView();
        }


        [HttpPost]
        public JsonResult _List(string none)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            return Json(new { Values = _roleService.FillTree(Member.RoleCode), type = AlarmType.none.ToString() });
        }
        
        [HttpPost]
        [AllowAnonymous]
        public JsonResult _ListPermission(string none)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            return Json(new { Values = _permissionService.FillTreeChecked(Member.RoleId), type = AlarmType.none.ToString() });
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult _ListRolePermission(Int64 RoleId)
        {
            return Json(new { Values = JsonConvert.SerializeObject(_rolePermissionService.GetPermissionsByRoleId(RoleId).Select(i => i.PermissionId)), type = AlarmType.none.ToString() });
        }


		#endregion


		#region _Create

        public PartialViewResult _Create()
        {
            CreateRoleViewModel role = new CreateRoleViewModel();
			role.Members = _memberService.Get().ToList();

            ViewBag.Id = "-1";
            return PartialView(role);
        }

        [HttpPost]
        public JsonResult _Create(CreateRoleViewModel crvm)
        {
            try
            {
                ModelState.Remove("Role.Title");

                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = crvm.Role.Id;

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_roleService.SaveRolePermission(crvm, Member.Id) > 0)
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

		#endregion


		#region _Update

        [HttpPost]
        public PartialViewResult _Update(Int64 id)
        {
            try
            {
                CreateRoleViewModel crvm = new CreateRoleViewModel();
				crvm.Members = _memberService.Get().ToList();
                crvm.Role = _roleService.Find(id);

                return PartialView("_Create", crvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateRoleViewModel());
            }
        }

		#endregion


		#region _Delete

        [HttpPost]
        public JsonResult _Delete(Int64 id)
        {
            try
            {
                if (!ModelState.IsValidField("Id") || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;


                if (_roleService.DeleteRolePermission(id, Member.Id) > 0)
                    return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessDelete) });
                else
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorDelete) });
            }
            catch (Exception ex)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }

		#endregion
	}
}