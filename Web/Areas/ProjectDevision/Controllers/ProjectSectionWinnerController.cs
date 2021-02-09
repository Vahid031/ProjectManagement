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
using ViewModels.ProjectDevision.ProjectSectionWinnerViewModel;
using ViewModels.Other;
using Web.Utilities.AppCode;
using DomainModels.Enums;
using Services.Interfaces.BaseInformation;
using DomainModels.Entities.BaseInformation;

namespace Web.Areas.ProjectDevision.Controllers
{
    public class ProjectSectionWinnerController : Controller
    {
        #region Index And ctor

        IUnitOfWork _uow;
        IRankService _rankService;
        IContractorService _contractorService;
        IProjectSectionService _projectSectionService;
        IProjectSectionWinnerService _projectSectionWinnerService;
        IPermissionService _permissionService;

        public ProjectSectionWinnerController(IUnitOfWork uow, IRankService rankService, IContractorService contractorService, IProjectSectionService projectSectionService, IProjectSectionWinnerService projectSectionWinnerService, IPermissionService permissionService)
        {
            _uow = uow;

            _rankService = rankService;
            _contractorService = contractorService;
            _projectSectionService = projectSectionService;
            _projectSectionWinnerService = projectSectionWinnerService;
            _permissionService = permissionService;
        }


        public PartialViewResult _Index(Int64 Id)
        {
            AdminViewModel Member = HttpContext.Session["Member"] as AdminViewModel;
            ViewBag.Permission = _permissionService.GetPagePermissions("/ProjectDevision/ProjectSectionWinner/_Index/" + Id, Member.RoleId);

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
        public JsonResult _List(ListProjectSectionWinnerViewModel latvm, Paging _Pg)
        {
            try
            {
                AdminViewModel member = HttpContext.Session["Member"] as AdminViewModel;

                return Json(new { Values = new JavaScriptSerializer().Serialize(_projectSectionWinnerService.GetAll(latvm, ref _Pg)), RowCount = _Pg._rowCount, type = AlarmType.none.ToString() });
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
            CreateProjectSectionWinnerViewModel catvm = new CreateProjectSectionWinnerViewModel();
            catvm.Ranks = new List<Rank>() { new Rank() { Title = "انتخاب کنید..." } };
            catvm.Ranks.AddRange(_rankService.Get().ToList());

            catvm.Contractors = new List<Contractor>() { new Contractor() { CompanyName = "انتخاب کنید..." } };
            catvm.Contractors.AddRange(_contractorService.Get().ToList());

            ViewBag.Id = "-1";
            return PartialView(catvm);
        }

        [HttpPost]
        public JsonResult _Create(CreateProjectSectionWinnerViewModel catvm)
        {
            try
            {
                if (!ModelState.IsValid || !Request.IsAjaxRequest())
                    return Json(new { type = AlarmType.danger.ToString(), message = Messages.GetMsg(MsgKey.ALLFieldsRequired) });

                long? isInsert = catvm.ProjectSectionWinner.Id;
                // برای معاملات جزء است به معنای رنک ندارد
                if (catvm.ProjectSectionWinner.RankId == null)
                    catvm.ProjectSectionWinner.RankId = 4;

                if (isInsert == -1)
                {
                    if (_projectSectionWinnerService.Get(c => c.ProjectSectionId == catvm.ProjectSectionWinner.ProjectSectionId && c.ContractorId == catvm.ProjectSectionWinner.ContractorId).Any())
                    {
                        return Json(new { type = AlarmType.danger.ToString(), message = "این پیمانکار قبلا ثبت شده است" });
                    }
                    
                    if (catvm.ProjectSectionWinner.RankId == 1)
                    {
                        if (_projectSectionWinnerService.Get(c => c.ProjectSectionId == catvm.ProjectSectionWinner.ProjectSectionId && c.RankId == 1).Any())
                        {
                            return Json(new { type = AlarmType.danger.ToString(), message = "رتبه تکراری انتخابی تکراری است" });
                        }
                    }


                    if (!_projectSectionWinnerService.Get(i => i.ProjectSectionId == catvm.ProjectSectionWinner.ProjectSectionId.Value).Any())
                    {
                        var projectSection = _projectSectionService.Get(i => i.Id == catvm.ProjectSectionWinner.ProjectSectionId.Value).FirstOrDefault();
                        projectSection.ProjectSectionAssignmentStateId = Convert.ToInt64(GeneralEnums.ProjectSectionAssignmentStates.ProjectSectionWinnerDegree);
                    }

                    _projectSectionWinnerService.Insert(catvm.ProjectSectionWinner);
                }
                else
                {
                    _projectSectionWinnerService.Update(catvm.ProjectSectionWinner);
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


                //---------------------
                


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
                CreateProjectSectionWinnerViewModel catvm = new CreateProjectSectionWinnerViewModel();
                catvm.ProjectSectionWinner = _projectSectionWinnerService.Find(id);

                catvm.Ranks = new List<Rank>() { new Rank() { Title = "انتخاب کنید..." } };
                catvm.Ranks.AddRange(_rankService.Get().ToList());

                catvm.Contractors = new List<Contractor>() { new Contractor() { CompanyName = "انتخاب کنید..." } };
                catvm.Contractors.AddRange(_contractorService.Get().ToList());

                return PartialView("_Create", catvm);
            }
            catch (Exception)
            {
                ViewBag.error = Messages.GetMsg(MsgKey.ErrorSystem);
                return PartialView("_Create", new CreateProjectSectionWinnerViewModel());
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

                _projectSectionWinnerService.Delete(id);

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