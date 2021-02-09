using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Enums;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDefine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDefine.ProjectQuantityGoalViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDefine.Controllers
{
    public class ProjectQuantityGoalController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectQuantityGoalService _ProjectQuantityGoalService;
        IUnitService _unitService;
        IPermissionService _permissionService;
        IProjectService _projectService;

        public ProjectQuantityGoalController(IUnitOfWork uow, IProjectService projectService, IProjectQuantityGoalService ProjectQuantityGoalService, IUnitService unitService, IPermissionService permissionService)
        {
            _uow = uow;

            _ProjectQuantityGoalService = ProjectQuantityGoalService;
            _unitService = unitService;
            _permissionService = permissionService;
            _projectService = projectService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/ProjectQuantityGoal/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectQuantityGoalViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectQuantityGoalService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            CreateProjectQuantityGoalViewModel cpftvm = new CreateProjectQuantityGoalViewModel();
            cpftvm.Units = new List<Unit>() { new Unit() { Title = "انتخاب کنید..." } };
            cpftvm.Units.AddRange(_unitService.Get().ToList());

            ViewBag.Id = "-1";
            return PartialView(cpftvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectQuantityGoalViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectQuantityGoal.Id;

                if (isInsert == -1)
                {
                    _ProjectQuantityGoalService.Insert(catvm.ProjectQuantityGoal);
                }
                else
                {
                    _ProjectQuantityGoalService.Update(catvm.ProjectQuantityGoal);
                }


                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
                    var project = _projectService.Get(c => c.Id == catvm.ProjectQuantityGoal.ProjectId).FirstOrDefault();
                    project.ProjectStateId = Convert.ToInt64(GeneralEnums.ProjectState.WaitingAlocation);
                    _projectService.Update(project);
                    _uow.SaveChanges(Member.Id);


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
                CreateProjectQuantityGoalViewModel cpftvm = new CreateProjectQuantityGoalViewModel();
                cpftvm.ProjectQuantityGoal = _ProjectQuantityGoalService.Find(id);

                cpftvm.Units = new List<Unit>() { new Unit() { Title = "انتخاب کنید..." } };
                cpftvm.Units.AddRange(_unitService.Get().ToList());


                return PartialView("_Create", cpftvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectQuantityGoalViewModel());
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

                _ProjectQuantityGoalService.Delete(id);

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