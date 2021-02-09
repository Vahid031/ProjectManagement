using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.General.MemberViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;

namespace Web.Areas.General.Controllers
{
    public class MemberController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IMemberService _memberService;
        IRoleService _roleService;
        IPermissionService _permissionService;

        public MemberController(IUnitOfWork uow, IMemberService memberService, IRoleService roleService, IPermissionService permissionService)
        {
            _uow = uow;

            _memberService = memberService;
            _roleService = roleService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/General/Member/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult FirstNameValidation([Bind(Prefix = "Member.FirstName")]string value, [Bind(Prefix = "Member.Id")]long id, [Bind(Prefix = "Member.LastName")]string lastName, [Bind(Prefix = "Member.FatherName")]string fatherName)
        {
            List<Member> item;

            if (id == -1)
                item = _memberService.Get(c => c.FirstName == value && c.LastName == lastName && c.FatherName == fatherName).ToList();
            else
                item = _memberService.Get(c => c.FirstName == value && c.Id != id && c.LastName == lastName && c.FatherName == fatherName).ToList();

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
        public JsonResult UserNameValidation([Bind(Prefix = "Member.UserName")]string value, [Bind(Prefix = "Member.Id")]long id)
        {
            var item = _memberService.Get(c => c.UserName == value && c.Id != id).ToList();

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
        public JsonResult NationalCodeValidation([Bind(Prefix = "Member.NationalCode")]string value)
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
        public JsonResult _List(ListMemberViewModel lmvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_memberService.GetAll(lmvm, ref _Pg, member.RoleCode)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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

            CreateMemberViewModel cmvm = new CreateMemberViewModel();
            cmvm.Roles = DropdownTree.GetDdlList(_roleService.Get(i => i.Code.StartsWith(Member.RoleCode)).Select(i => new BaseTreeviewViewModel() { Id = i.Id, Title = i.Title, ParentId = i.ParentId, HaveChild = (i.Level == 3) ? false : true }).ToList(), true, 1);
            
            ViewBag.Id = "-1";
            return PartialView(cmvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateMemberViewModel cmvm)
        {
            try
            {

                if (cmvm.Member.RoleId == 3)
                    cmvm.Member.MemberGroupId = Convert.ToInt64(GeneralEnums.MemberGroup.Supervisor); // ناظر
                else if (cmvm.Member.RoleId == 4)
                    cmvm.Member.MemberGroupId = Convert.ToInt64(GeneralEnums.MemberGroup.Advisor); // مشاور
                else
                    cmvm.Member.MemberGroupId = Convert.ToInt64(GeneralEnums.MemberGroup.Personel); // پرسنل

                cmvm.Member.Password = Hashing.MultiEncrypt(cmvm.Member.Password);
                cmvm.Member.PasswordCampare = Hashing.MultiEncrypt(cmvm.Member.PasswordCampare);

                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = cmvm.Member.Id;

                if (isInsert == -1)
                {
                    _memberService.Insert(cmvm.Member);
                }
                else
                {
                    _memberService.Update(cmvm.Member);
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

                CreateMemberViewModel cmvm = new CreateMemberViewModel();
                cmvm.Roles = DropdownTree.GetDdlList(_roleService.Get(i => i.Code.StartsWith(Member.RoleCode)).Select(i => new BaseTreeviewViewModel() { Id = i.Id, Title = i.Title, ParentId = i.ParentId, HaveChild = (i.Level == 3) ? false : true }).ToList(), true, 1);
                cmvm.Member = _memberService.Find(id);
                cmvm.Member.Password = "";
                cmvm.Member.PasswordCampare = "";

                return PartialView("_Create", cmvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateMemberViewModel());
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

                _memberService.Delete(id);

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