using DatabaseContext.Context;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectDefine;
using Infrastracture.AppCode;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ViewModels.Other;
using ViewModels.ProjectDefine.ProgramPlanViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDefine.Controllers
{
    public class ProgramPlanController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProgramPlanService _programPlanService;
        IProgramService _programService;
        IPlanTypeService _planTypeService;
        ISetService _setService;
        IFinantialYearService _finantialYearService;
        IPermissionService _permissionService;

        public ProgramPlanController(IUnitOfWork uow, IProgramPlanService programPlanService, IProgramService programService, IPlanTypeService planTypeService, ISetService setService, IFinantialYearService finantialYearService, IPermissionService permissionService)
        {
            _uow = uow;

            _programPlanService = programPlanService;
            _programService = programService;
            _planTypeService = planTypeService;
            _setService = setService;
            _finantialYearService = finantialYearService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/ProgramPlan/_Index", Member.RoleId);

            return PartialView();
        }

        #endregion


        #region Validation

        [AllowAnonymous]
        public JsonResult ExecutionSetTitleValidation([Bind(Prefix = "ExecutionSetTitle")]string value, [Bind(Prefix = "ProgramPlan.ExecutionSetId")]Int64? executionSetId)
        {

            if (value != null && value != "انتخاب کنید..." && (executionSetId == null || executionSetId == -1))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult BeneficiarySetTitleValidation([Bind(Prefix = "BeneficiarySetTitle")]string value, [Bind(Prefix = "ProgramPlan.BeneficiarySetId")]Int64? beneficiarySetId)
        {
            if (value != null && value != "انتخاب کنید..." && (beneficiarySetId == null || beneficiarySetId == -1))
                return Json(false, JsonRequestBehavior.AllowGet);
            else
                return Json(true, JsonRequestBehavior.AllowGet);
        }


        [AllowAnonymous]
        public JsonResult PlanCodeValidation([Bind(Prefix = "ProgramPlan.PlanCode")]string value, [Bind(Prefix = "ProgramPlan.Id")]long id)
        {
            var item = _programPlanService.Get(c => c.PlanCode == value && c.Id != id).ToList();
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
        public JsonResult _List(ListProgramPlanViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_programPlanService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
            }
            catch (Exception)
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(""), RowCount = 0, type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ErrorLoadData) });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetSets()
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_setService.Get().Select(i => new { i.Id, i.Title, i.Code }).ToList()) });
        }


        [HttpPost]
        [AllowAnonymous]
        public JsonResult GetProgramPlans(Int64 programId)
        {
            return Json(new { Values = new JavaScriptSerializer().Serialize(_programPlanService.Get(i => i.ProgramId == programId).Select(i => new { Id = i.Id, Title = i.PlanTitle, Code = i.PlanCode }).ToList()) });
        }

        #endregion


        #region Insert And Update

        [HttpGet]
        public PartialViewResult _Create()
        {
            CreateProgramPlanViewModel cpptvm = new CreateProgramPlanViewModel();
            cpptvm.Programs = new List<Program>() { new Program() { Title = "انتخاب کنید..." } };
            cpptvm.Programs.AddRange(_programService.Get().ToList());

            cpptvm.PlanTypes = new List<PlanType>() { new PlanType() { Title = "انتخاب کنید..." } };
            cpptvm.PlanTypes.AddRange(_planTypeService.Get().ToList());

            cpptvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            cpptvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());


            ViewBag.Id = "-1";
            return PartialView(cpptvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProgramPlanViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProgramPlan.Id;

                if (isInsert == -1)
                {
                    _programPlanService.Insert(catvm.ProgramPlan);
                }
                else
                {
                    _programPlanService.Update(catvm.ProgramPlan);
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
                CreateProgramPlanViewModel cpptvm = new CreateProgramPlanViewModel();
                cpptvm.ProgramPlan = _programPlanService.Find(id);

                cpptvm.Programs = new List<Program>() { new Program() { Title = "انتخاب کنید..." } };
                cpptvm.Programs.AddRange(_programService.Get().ToList());

                cpptvm.PlanTypes = new List<PlanType>() { new PlanType() { Title = "انتخاب کنید..." } };
                cpptvm.PlanTypes.AddRange(_planTypeService.Get().ToList());

                cpptvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
                cpptvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());
    
                var set = _setService.Get(i => i.Id == cpptvm.ProgramPlan.ExecutionSetId.Value).FirstOrDefault();
                if (set != null)
                    cpptvm.ExecutionSetTitle = set.Title + " (" + set.Code + ")";

                set = _setService.Get(i => i.Id == cpptvm.ProgramPlan.BeneficiarySetId.Value).FirstOrDefault();
                if (set != null)
                    cpptvm.BeneficiarySetTitle = set.Title + " (" + set.Code + ")";

                return PartialView("_Create", cpptvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProgramPlanViewModel());
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

                _programPlanService.Delete(id);

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