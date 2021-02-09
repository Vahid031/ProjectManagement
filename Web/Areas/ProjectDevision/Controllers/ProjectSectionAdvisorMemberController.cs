using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDevision.ProjectSectionMemberViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionAdvisorMemberController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IMonitoringTypeService _monitoringTypeService;
        IMemberService _memberService;
        IProjectSectionService _projectSectionService;
        IProjectSectionMemberService _projectSectionMemberService;
        IPermissionService _permissionService;

        public ProjectSectionAdvisorMemberController(IUnitOfWork uow, IMonitoringTypeService monitoringTypeService, IMemberService memberService, IProjectSectionService projectSectionService, IProjectSectionMemberService ProjectSectionMemberService, IPermissionService permissionService)
        {
            _uow = uow;

            _monitoringTypeService = monitoringTypeService;
            _memberService = memberService;
            _projectSectionService = projectSectionService;
            _projectSectionMemberService = ProjectSectionMemberService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionAdvisorMember/_Index", Member.RoleId);

            return PartialView();
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
        public JsonResult _List(ListProjectSectionMemberViewModel lpptvm, Paging _Pg)
        {
            try
            {
                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                if (Member.RoleId == 3 || Member.RoleId == 4)
                    lpptvm.ProjectSectionMember.MemberId = Member.Id;
                
                lpptvm.Member.MemberGroupId = 3;
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionMemberService.GetAllAdvisor(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            CreateProjectSectionMemberViewModel cpsivm = new CreateProjectSectionMemberViewModel();

            cpsivm.MonitoringTypes = new List<MonitoringType>() { new MonitoringType() { Title = "انتخاب کنید..." } };
            cpsivm.MonitoringTypes.AddRange(_monitoringTypeService.Get().ToList());

            cpsivm.Members = new List<BaseDropdownViewModel>() { new BaseDropdownViewModel() { Title = "انتخاب کنید..." } };

            Int64 AdvisorId = Convert.ToInt64(GeneralEnums.MemberGroup.Advisor);

            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            if (Member.RoleId == 3 || Member.RoleId == 4)
            {
                cpsivm.Members.AddRange(_memberService.Get(i => i.Id == Member.Id && i.MemberGroupId == AdvisorId).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.FirstName + " " + i.LastName }).ToList());
            }
            else
            {
                cpsivm.Members.AddRange(_memberService.Get(i => i.MemberGroupId == AdvisorId).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.FirstName + " " + i.LastName }).ToList());
            }
            ViewBag.Id = "-1";
            return PartialView(cpsivm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionMemberViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectSectionAdvisorMember.Id;

                if (isInsert == -1)
                {
                    _projectSectionMemberService.Insert(catvm.ProjectSectionAdvisorMember);
                }
                else
                {
                    _projectSectionMemberService.Update(catvm.ProjectSectionAdvisorMember);
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
                CreateProjectSectionMemberViewModel cpsivm = new CreateProjectSectionMemberViewModel();
                cpsivm.ProjectSectionAdvisorMember = _projectSectionMemberService.Find(id);

                cpsivm.MonitoringTypes = new List<MonitoringType>() { new MonitoringType() { Title = "انتخاب کنید..." } };
                cpsivm.MonitoringTypes.AddRange(_monitoringTypeService.Get().ToList());

                cpsivm.Members = new List<BaseDropdownViewModel>() { new BaseDropdownViewModel() { Title = "انتخاب کنید..." } };
                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
                if (Member.RoleId == 3 || Member.RoleId == 4 || Member.RoleId == 10)
                {
                    cpsivm.Members.AddRange(_memberService.Get(i => i.Id == Member.Id && i.MemberGroupId == 3).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.FirstName + " " + i.LastName }).ToList());
                }
                else
                {
                    cpsivm.Members.AddRange(_memberService.Get(i => i.MemberGroupId == 3).Select(i => new BaseDropdownViewModel() { Id = i.Id, Title = i.FirstName + " " + i.LastName }).ToList());
                }
                return PartialView("_Create", cpsivm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionMemberViewModel());
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

                _projectSectionMemberService.Delete(id);

                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
                    return Json(new { type = AlarmType.success.ToString(), message = Messages.GetMsg(MsgKey.SuccessDelete) });
                }
                else
                {
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorDelete) });
                }
            }
            catch (Exception)
            {
                return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorSystem) });
            }
        }

        #endregion
    }
}