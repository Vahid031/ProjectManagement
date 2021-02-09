using DatabaseContext.Context;
using DomainModels.Entities.General;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using System;
using System.Web.Mvc;
using ViewModels;
using ViewModels.Other;
using Web.Utilities.AppCode;
using System.Linq;

namespace Web.Areas.General.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IMemberService _memberService;
        private readonly IRoleService _roleService;

        public AccountController(IUnitOfWork uow, IMemberService memberService, IRoleService roleService)
        {
            _uow = uow;
            _memberService = memberService;
            _roleService = roleService;
        }

        //
        // GET: /Account/
        [AllowAnonymous]
        public ViewResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public PartialViewResult _Login()
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            ViewBag.OrganizationTitle = db.Permissions.Where(m => m.ParentId == null).FirstOrDefault().Title; //"اداره کل ورزش و جوانان استان البرز";
            //var test = db.ProjectSectionGovermentWorthyDocumentsPaidPrice.ToList();



            return PartialView();
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult _Login(LoginViewModel model)
        {
            if (Request.IsAjaxRequest())
            {
                if (ModelState.IsValid)
                {

                    Member member = _memberService.AuthenticateUser(model.UserName, Hashing.MultiEncrypt(model.Password));

                    if (member != null)
                    {
                        Role _role = _roleService.Get(c => c.Id == member.RoleId).FirstOrDefault();
                        Session["Member"] = new AdminViewModel(member.Id, Convert.ToInt64(member.RoleId), member.UserName, _role.Code);
                        return Json(new { state = true, redirectToUrl = Url.Action("Index", "Home", routeValues: new { Area = "General" }) });
                    }
                    else
                    {
                        return Json(new { state = false, message = Message.GetMessage(Message.MessageType.ErrorUsernameMistake) });
                    }

                }

                return Json(new { state = false, message = Message.GetMessage(Message.MessageType.ALLFieldsRequired) });
            }

            return Json(new { state = false, message = Message.GetMessage(Message.MessageType.ALLFieldsRequired) });
        }

        [AllowAnonymous]
        public PartialViewResult _Recovery()
        {
            return PartialView();
        }
    }
}