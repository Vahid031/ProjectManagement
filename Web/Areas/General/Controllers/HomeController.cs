using DatabaseContext.Context;
using DomainModels.Entities.General;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.General.MemberViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.General.Controllers
{
    public class HomeController : Controller
    {
        IUnitOfWork _uow;
        IMemberService _memberService;
        IPermissionService _PermissionService;

        public HomeController(IUnitOfWork uow, IMemberService memberService, IPermissionService permissionService)
        {
            _uow = uow;

            _memberService = memberService;
            _PermissionService = permissionService;
        }

        public ActionResult Index()
        {
            //if (HttpContext.Session["Member"] == null)
            //    return RedirectToAction("Index", "Site.Home");

            return View();
        }

        [AllowAnonymous]
        public PartialViewResult _ChangePassword()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            CreateMemberViewModel cmvm = new CreateMemberViewModel();
            cmvm.Member = _memberService.Find(Member.Id);
            cmvm.Member.Password = "";
            cmvm.Member.PasswordCampare = "";

            return PartialView(cmvm);
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult _SaveChanges(CreateMemberViewModel tempCmvm)
        {
            try
            {
                CreateMemberViewModel cmvm = new CreateMemberViewModel();
                cmvm.Member = _memberService.Find(tempCmvm.Member.Id);

                if (cmvm.Member.Password == Hashing.MultiEncrypt(tempCmvm.OldPassword))
                {
                    cmvm.Member.Password = Hashing.MultiEncrypt(tempCmvm.Member.Password);
                    cmvm.Member.PasswordCampare = Hashing.MultiEncrypt(tempCmvm.Member.PasswordCampare);

                    _memberService.Update(cmvm.Member);

                    if (_uow.SaveChanges(cmvm.Member.Id) > 0)
                    {
                        return Json(new { type = AlarmType.info.ToString(), message = Messages.GetMsg(MsgKey.SuccessUpdate) });
                    }
                    else
                    {
                        return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorUpdate) });
                    }
                }
                else
                {
                    return Json(new { type = AlarmType.danger.ToString(), message = "کلمه عبور قبلی اشتباه است" });
                }

            }
            catch (Exception)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }



        [AllowAnonymous]
        public PartialViewResult _RightNavigation()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

            Member m = _memberService.Find(Member.Id);

            ViewBag.UserName = m.Role.Title;
            ViewBag.value = _PermissionService.FillTree(1, Member.RoleId);

            return PartialView();
        }

        public PartialViewResult _Default()
        {
            return PartialView();
        }

        public ActionResult Exit()
        {
            HttpContext.Session["Member"] = null;
            HttpContext.Session.Remove("Member");
            HttpContext.Session.Abandon();

            return RedirectToAction("Index");
        }


    }
}