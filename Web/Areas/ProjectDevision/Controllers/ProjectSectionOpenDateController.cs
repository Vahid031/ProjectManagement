using DatabaseContext.Context;
using Infrastracture.AppCode;
using Services.Interfaces.ProjectDevision;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.ProjectDevision.ProjectSectionOpenDateViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;
using DomainModels.Enums;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionOpenDateController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;
        IProjectSectionOpenDateService _projectSectionOpenDateService;
        IPermissionService _permissionService;

        public ProjectSectionOpenDateController(IUnitOfWork uow, IProjectSectionService projectSectionService, IProjectSectionOpenDateService projectSectionOpenDateService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
            _projectSectionOpenDateService = projectSectionOpenDateService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index(Int64 Id)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionOpenDate/_Index/" + Id, Member.RoleId);

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
        public JsonResult _List(ListProjectSectionOpenDateViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionOpenDateService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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

            CreateProjectSectionOpenDateViewModel catvm = new CreateProjectSectionOpenDateViewModel();
            
            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionOpenDateViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectSectionOpenDate.Id;

                if (isInsert == -1)
                {
                    if (!_projectSectionOpenDateService.Get(i => i.ProjectSectionId == catvm.ProjectSectionOpenDate.ProjectSectionId.Value).Any())
                    {
                        var projectSection = _projectSectionService.Get(i => i.Id == catvm.ProjectSectionOpenDate.ProjectSectionId.Value).FirstOrDefault();
                        projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionWinner);
                    }

                    _projectSectionOpenDateService.Insert(catvm.ProjectSectionOpenDate);
                }
                else
                {
                    _projectSectionOpenDateService.Update(catvm.ProjectSectionOpenDate);
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

                CreateProjectSectionOpenDateViewModel catvm = new CreateProjectSectionOpenDateViewModel();
                catvm.ProjectSectionOpenDate = _projectSectionOpenDateService.Find(id);

                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionOpenDateViewModel());
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

                _projectSectionOpenDateService.Delete(id);

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