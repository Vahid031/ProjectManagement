using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDevision;
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
using ViewModels.ProjectDevision.ProjectSectionViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectSectionService _projectSectionService;
        IAssignmentTypeService _assignmentTypeService;
        IPermissionService _permissionService;

        public ProjectSectionController(IUnitOfWork uow, IProjectSectionService projectSectionService, IAssignmentTypeService assignmentTypeService, IPermissionService permissionService)
        {
            _uow = uow;

            _projectSectionService = projectSectionService;
            _assignmentTypeService = assignmentTypeService;
            _permissionService = permissionService;
        }

        [AllowAnonymous]
        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/ProjectProgramming/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectSection2ViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionService.GetAll2(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        #endregion


        #region Insert And Update

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetProjectSectionByProjectId(Int64 projcetId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionService.Get(i => i.ProjectId == projcetId).Select(i => new { i.Id, i.Title }).ToList()) });
        }

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateProjectSectionViewModel cpstvm = new CreateProjectSectionViewModel();

            cpstvm.AssignmentTypes = new List<AssignmentType>() { new AssignmentType() { Title = "انتخاب کنید..." } };
            cpstvm.AssignmentTypes.AddRange(_assignmentTypeService.Get().ToList());

            ViewBag.Id = "-1";
            return PartialView(cpstvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionViewModel catvm)
        {
            try
            {
                if (catvm.ProjectSection.Id == -1)
                {
                    catvm.ProjectSection.ProjectSectionOperationStateId = Convert.ToInt64(GeneralEnums.ProjectSectionOperationStates.ProjectSectionAdvisorSupervisorSelect);
                    catvm.ProjectSection.ProjectSectionAssignmentStateId = catvm.ProjectSection.AssignmentTypeId; 
                }

                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectSection.Id;

                if (isInsert == -1)
                {
                    _projectSectionService.Insert(catvm.ProjectSection);
                }
                else
                {
                    _projectSectionService.Update(catvm.ProjectSection);
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
                CreateProjectSectionViewModel cpstvm = new CreateProjectSectionViewModel();
                cpstvm.ProjectSection = _projectSectionService.Find(id);

                cpstvm.AssignmentTypes = new List<AssignmentType>() { new AssignmentType() { Title = "انتخاب کنید..." } };
                cpstvm.AssignmentTypes.AddRange(_assignmentTypeService.Get().ToList());


                return PartialView("_Create", cpstvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionViewModel());
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

                _projectSectionService.Delete(id);

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