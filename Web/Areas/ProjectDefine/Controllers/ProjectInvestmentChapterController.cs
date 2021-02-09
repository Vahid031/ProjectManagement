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
using ViewModels.ProjectDefine.ProjectInvestmentChapterViewModel;
using Web.Utilities.AppCode;

namespace Web.Areas.ProjectDefine.Controllers
{
    public class ProjectInvestmentChapterController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IProjectInvestmentChapterService _ProjectInvestmentChapterService;
        IFinantialYearService _finantialYearService;
        IInvestmentChapterService _investmentChapterService;
        IInvestmentChapterTypeService _investmentChapterTypeService;
        IPermissionService _permissionService;
        IProjectService _projectService;

        public ProjectInvestmentChapterController(IUnitOfWork uow, 
            IProjectInvestmentChapterService ProjectInvestmentChapterService, 
            IFinantialYearService finantialYearService, 
            IInvestmentChapterService investmentChapterService, 
            IInvestmentChapterTypeService investmentChapterTypeService, 
            IPermissionService permissionService,
            IProjectService projectService
            )
        {
            _uow = uow;

            _ProjectInvestmentChapterService = ProjectInvestmentChapterService;
            _finantialYearService = finantialYearService;
            _investmentChapterService = investmentChapterService;
            _investmentChapterTypeService = investmentChapterTypeService;
            _permissionService = permissionService;
            _projectService = projectService;
        }


        public PartialViewResult _Index()
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDefine/ProjectInvestmentChapter/_Index", Member.RoleId);

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
        public JsonResult _List(ListProjectInvestmentChapterViewModel lpptvm, Paging _Pg)
        {
            try
            {
                return Json(new { Values = new JavaScriptSerializer().Serialize(_ProjectInvestmentChapterService.GetAll(lpptvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            CreateProjectInvestmentChapterViewModel cpftvm = new CreateProjectInvestmentChapterViewModel();
            cpftvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
            cpftvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());

            cpftvm.InvestmentChapters = new List<InvestmentChapter>() { new InvestmentChapter() { Title = "انتخاب کنید..." } };
            cpftvm.InvestmentChapters.AddRange(_investmentChapterService.Get().ToList());

            cpftvm.InvestmentChapterTypes = new List<InvestmentChapterType>();

            ViewBag.Id = "-1";
            return PartialView(cpftvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectInvestmentChapterViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectInvestmentChapter.Id;

                if (isInsert == -1)
                {
                    _ProjectInvestmentChapterService.Insert(catvm.ProjectInvestmentChapter);
                }
                else
                {
                    _ProjectInvestmentChapterService.Update(catvm.ProjectInvestmentChapter);
                }


                AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;

                if (_uow.SaveChanges(Member.Id) > 0)
                {
                    var project = _projectService.Get(c => c.Id == catvm.ProjectInvestmentChapter.ProjectId).FirstOrDefault();
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
                CreateProjectInvestmentChapterViewModel cpftvm = new CreateProjectInvestmentChapterViewModel();
                cpftvm.ProjectInvestmentChapter = _ProjectInvestmentChapterService.Find(id);

                cpftvm.FinantialYears = new List<FinantialYear>() { new FinantialYear() { Title = "انتخاب کنید..." } };
                cpftvm.FinantialYears.AddRange(_finantialYearService.Get().ToList());

                cpftvm.InvestmentChapterId = _investmentChapterTypeService.Get(i => i.Id == cpftvm.ProjectInvestmentChapter.InvestmentChapterTypeId).First().InvestmentChapterId.Value;

                cpftvm.InvestmentChapters = new List<InvestmentChapter>() { new InvestmentChapter() { Title = "انتخاب کنید..." } };
                cpftvm.InvestmentChapters.AddRange(_investmentChapterService.Get().ToList());

                cpftvm.InvestmentChapterTypes = new List<InvestmentChapterType>() { new InvestmentChapterType() { Title = "انتخاب کنید..." } };
                cpftvm.InvestmentChapterTypes.AddRange(_investmentChapterTypeService.Get(i => i.InvestmentChapterId == cpftvm.InvestmentChapterId));

                return PartialView("_Create", cpftvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectInvestmentChapterViewModel());
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

                _ProjectInvestmentChapterService.Delete(id);

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