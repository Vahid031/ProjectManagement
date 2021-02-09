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
using ViewModels.ProjectDevision.ProjectSectionFinanceEvaluationViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;
using DomainModels.Enums;
using Services.Interfaces.BaseInformation;
using DomainModels.Entities.BaseInformation;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionFinanceEvaluationController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IContractorService _contractorService;
        IProjectSectionService _projectSectionService;
        IProjectSectionFinanceEvaluationService _ProjectSectionFinanceEvaluationService;
        IPermissionService _permissionService;

        public ProjectSectionFinanceEvaluationController(IUnitOfWork uow, IContractorService contractorService, IProjectSectionService projectSectionService, IProjectSectionFinanceEvaluationService ProjectSectionFinanceEvaluationService, IPermissionService permissionService)
        {
            _uow = uow;

            _contractorService = contractorService;
            _projectSectionService = projectSectionService;
            _ProjectSectionFinanceEvaluationService = ProjectSectionFinanceEvaluationService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index(Int64 Id)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionFinanceEvaluation/_Index/" + Id, Member.RoleId);

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
        public JsonResult _List(ListProjectSectionFinanceEvaluationViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectSectionFinanceEvaluationService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            CreateProjectSectionFinanceEvaluationViewModel catvm = new CreateProjectSectionFinanceEvaluationViewModel();
            catvm.Contractors = new List<Contractor>() { new Contractor() { CompanyName = "انتخاب کنید..." } };
            catvm.Contractors.AddRange(_contractorService.Get().ToList());

            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionFinanceEvaluationViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectSectionFinanceEvaluation.Id;

                if (isInsert == -1)
                {
                    if (!_ProjectSectionFinanceEvaluationService.Get(i => i.ProjectSectionId == catvm.ProjectSectionFinanceEvaluation.ProjectSectionId.Value).Any())
                    {
                        var projectSection = _projectSectionService.Get(i => i.Id == catvm.ProjectSectionFinanceEvaluation.ProjectSectionId.Value).FirstOrDefault();
                        projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionFinalParticipant);
                    }

                    _ProjectSectionFinanceEvaluationService.Insert(catvm.ProjectSectionFinanceEvaluation);
                }
                else
                {
                    _ProjectSectionFinanceEvaluationService.Update(catvm.ProjectSectionFinanceEvaluation);
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
                CreateProjectSectionFinanceEvaluationViewModel catvm = new CreateProjectSectionFinanceEvaluationViewModel();
                catvm.ProjectSectionFinanceEvaluation = _ProjectSectionFinanceEvaluationService.Find(id);

                catvm.Contractors = new List<Contractor>() { new Contractor() { CompanyName = "انتخاب کنید..." } };
                catvm.Contractors.AddRange(_contractorService.Get().ToList());

                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionFinanceEvaluationViewModel());
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

                _ProjectSectionFinanceEvaluationService.Delete(id);

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