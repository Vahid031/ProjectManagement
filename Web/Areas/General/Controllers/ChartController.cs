using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.ProjectReporting;
using DomainModels.Entities.Report;
using Services.Interfaces.BaseInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViewModels.ProjectDefine.ProjectViewModel;

namespace Web.Areas.General.Controllers
{
    public class ChartController : Controller
    {
        IStateService _StateService;
        IProjectTypeService _ProjectTypeService;
        IAssignmentTypeService _AssignmentTypeService;
        ICreditProvidePlaceService _CreditProvidePlaceService;
        IContractTypeService _ContractTypeService;
        
        public ChartController(IStateService StateService,  IContractTypeService ContractTypeService, ICreditProvidePlaceService CreditProvidePlaceService, IProjectTypeService ProjectTypeService, IAssignmentTypeService AssignmentTypeService)
        {
            _StateService = StateService;
            _ProjectTypeService = ProjectTypeService;
            _AssignmentTypeService = AssignmentTypeService;
            _CreditProvidePlaceService = CreditProvidePlaceService;
            _ContractTypeService = ContractTypeService;
        }
        //
        // GET: /General/Chart/
        [AllowAnonymous]
        public PartialViewResult _C1()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();

            C1 selected = db.Database.SqlQuery<C1>(@"select top(1)
            
            (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId
where os.Id=1  ) as fir,
           (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId
where os.Id=2 ) as sec,
          (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId
where os.Id=3 ) as sum").FirstOrDefault();
            ViewBag.FirstNumber = selected.fir;
            ViewBag.SecoundNumber = selected.sec;
            ViewBag.ThirdNumber = selected.sum;
            return PartialView(cpvm);
        }
        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C1Update(Int32? state)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and pr.StateId=" + state;


            C1 selected = db.Database.SqlQuery<C1>(@"select top(1)
            
            (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId where os.Id=1 " + query + " ) as fir, (select COUNT(pr.Id) from [ProjectDefine].[Projects] pr inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId where os.Id=2 " + query + " ) as sec, (select COUNT(pr.Id) from [ProjectDefine].[Projects] pr inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId where os.Id=3" + query + "  ) as sum").FirstOrDefault();
            ViewBag.FirstNumber = selected.fir;
            ViewBag.SecoundNumber = selected.sec;
            ViewBag.ThirdNumber = selected.sum;

            return PartialView("_C1Chart");
        }
        [AllowAnonymous]
        public PartialViewResult _C2()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();

            C1 selected = db.Database.SqlQuery<C1>(@"select top(1)
            
            (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
where pr.ProjectTypeId=1  ) as fir,
           (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
where pr.ProjectTypeId=2 ) as sec,
          (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId
where os.Id=3 ) as sum").FirstOrDefault();
            ViewBag.FirstNumber = selected.fir;
            ViewBag.SecoundNumber = selected.sec;

            return PartialView(cpvm);
        }
        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C2Update(Int32? state)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and pr.StateId=" + state;


            C1 selected = db.Database.SqlQuery<C1>(@"select top(1)
            (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
where pr.ProjectTypeId=1" + query + " ) as fir, (select COUNT(pr.Id) from [ProjectDefine].[Projects] pr where pr.ProjectTypeId=2" + query + " ) as sec,  (select COUNT(pr.Id) from [ProjectDefine].[Projects] pr inner join [BaseInformation].[OwnershipTypes] os on os.Id=pr.OwnershipTypeId where os.Id=3 ) as sum").FirstOrDefault();
            ViewBag.FirstNumber = selected.fir;
            ViewBag.SecoundNumber = selected.sec;


            return PartialView("_C2Chart");
        }

        [AllowAnonymous]
        public PartialViewResult _C3()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();

            C1 selected = db.Database.SqlQuery<C1>(@"select top(1)
            
            (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
where pr.ExecutionTypeId=1  ) as fir,
           (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
where pr.ExecutionTypeId=2 ) as sec,
          (select COUNT(pr.Id)
from [ProjectDefine].[Projects] pr
where pr.ExecutionTypeId=3) as sum").FirstOrDefault();
            ViewBag.FirstNumber = selected.fir;
            ViewBag.SecoundNumber = selected.sec;
            ViewBag.ThirdNumber = selected.sum;

            return PartialView(cpvm);
        }
        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C3Update(Int32? state)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and pr.StateId=" + state;


            C1 selected = db.Database.SqlQuery<C1>(@"select top(1)
            
            (select COUNT(pr.Id) from [ProjectDefine].[Projects] pr where pr.ExecutionTypeId=1" + query + "  ) as fir,  (select COUNT(pr.Id) from [ProjectDefine].[Projects] pr where pr.ExecutionTypeId=2" + query + " ) as sec, (select COUNT(pr.Id) from [ProjectDefine].[Projects] pr where pr.ExecutionTypeId=3" + query + ") as sum").FirstOrDefault();
            ViewBag.FirstNumber = selected.fir;
            ViewBag.SecoundNumber = selected.sec;
            ViewBag.ThirdNumber = selected.sum;
            return PartialView("_C3Chart");
        }


        [AllowAnonymous]
        public PartialViewResult _C4()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            cpvm.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            cpvm.ProjectTypes.AddRange(_ProjectTypeService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();

            C9 selected = db.Database.SqlQuery<C9>(@" select top(1)
            
            (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSections] p where p.AssignmentTypeId=1 ) as one,
           (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSections] p where p.AssignmentTypeId=2 ) as two,
          (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSections] p where p.AssignmentTypeId=3) as there,
  (select COUNT(p.Id)
FROM [ProjectDevision].[ProjectSections] p where p.AssignmentTypeId=4) as four,
  (select COUNT(p.Id)
FROM [ProjectDevision].[ProjectSections] p where p.AssignmentTypeId=5) as five").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;
            ViewBag.fourthNumber = selected.four;
            ViewBag.fifthNumber = selected.five;
            ViewBag.sixthNumber = selected.six;

            return PartialView(cpvm);
        }
        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C4Update(Int32? state, Int32? type)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and p.StateId=" + state;
            if (type != null)
                query += " and p.ProjectTypeId=" + type;


            C9 selected = db.Database.SqlQuery<C9>(@" select top(1)
            
            (select COUNT(ps.Id)
 FROM [ProjectDevision].[ProjectSections] ps
 inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId where ps.AssignmentTypeId=1" + query + " ) as one,(select COUNT(ps.Id) FROM [ProjectDevision].[ProjectSections] ps inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId where ps.AssignmentTypeId=2" + query + " ) as two, (select COUNT(ps.Id) FROM [ProjectDevision].[ProjectSections] ps inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId where ps.AssignmentTypeId=3" + query + ") as there, (select COUNT(ps.Id) FROM [ProjectDevision].[ProjectSections] ps inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId where ps.AssignmentTypeId=4" + query + ") as four,  (select COUNT(ps.Id) FROM [ProjectDevision].[ProjectSections] ps inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId where ps.AssignmentTypeId=5" + query + ") as five").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;
            ViewBag.fourthNumber = selected.four;
            ViewBag.fifthNumber = selected.five;
            ViewBag.sixthNumber = selected.six;
            return PartialView("_C4Chart");
        }

        [AllowAnonymous]
        public PartialViewResult _C5()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            cpvm.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            cpvm.ProjectTypes.AddRange(_ProjectTypeService.Get().ToList());
            cpvm.AssignmentType = new List<AssignmentType>() { new AssignmentType() { Title = "انتخاب کنید..." } };
            cpvm.AssignmentType.AddRange(_AssignmentTypeService.Get().ToList());
            cpvm.CreditProvidePlaces = new List<CreditProvidePlace>() { new CreditProvidePlace() { Title = "انتخاب کنید..." } };
            cpvm.CreditProvidePlaces.AddRange(_CreditProvidePlaceService.Get().ToList());

            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();

            C9 selected = db.Database.SqlQuery<C9>(@"    select top(1)
            
            (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSectionMadeh46s] p  ) as one,
           (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSectionMadeh48s] p  ) as two,
          (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSectionDeliveryTemprories] p ) as there,
  (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSectionDeliveryFixdes] p ) as four,
  (select COUNT(PDPS.Id)
    From ProjectDevision.ProjectSections PDPS
                             inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId
                             inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4)) as five").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;
            ViewBag.fourthNumber = selected.four;
            ViewBag.fifthNumber = selected.five - (selected.four + selected.there + selected.two + selected.one);
            ViewBag.sixthNumber = selected.six;

            return PartialView(cpvm);
        }

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C5Update(Int32? state, Int32? type, Int32? AssimentType, Int32? Plan)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and pr.StateId=" + state;
            if (type != null)
                query += " and pr.ProjectTypeId=" + type;
            if (AssimentType != null)
                query += "  and ps.AssignmentTypeId=" + AssimentType;
            if (Plan != null)
                query += " and pp.ProgramPlanId=" + Plan;


            C9 selected = db.Database.SqlQuery<C9>(@"select top(1)
            
            (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSectionMadeh46s] p
 inner join  [ProjectDevision].[ProjectSections] ps on ps.Id=p.ProjectSectionId
 inner join [ProjectDefine].[Projects] pr on ps.ProjectId=pr.Id 
 inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=pr.Id where 1=1" + query + ") as one,  (select COUNT(p.Id) FROM [ProjectDevision].[ProjectSectionMadeh48s] p inner join  [ProjectDevision].[ProjectSections] ps on ps.Id=p.ProjectSectionId  inner join [ProjectDefine].[Projects] pr on ps.ProjectId=pr.Id  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=pr.Id where 1=1" + query + "  ) as two,  (select COUNT(p.Id) FROM [ProjectDevision].[ProjectSectionDeliveryTemprories] p  inner join  [ProjectDevision].[ProjectSections] ps on ps.Id=p.ProjectSectionId  inner join [ProjectDefine].[Projects] pr on ps.ProjectId=pr.Id   inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=pr.Id where 1=1" + query + " ) as there,  (select COUNT(p.Id)  FROM [ProjectDevision].[ProjectSectionDeliveryFixdes] p  inner join  [ProjectDevision].[ProjectSections] ps on ps.Id=p.ProjectSectionId  inner join [ProjectDefine].[Projects] pr on ps.ProjectId=pr.Id   inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=pr.Id where 1=1" + query + " ) as four,  From ProjectDevision.ProjectSections PDPS inner join ProjectDevision.ProjectSectionContracts PDPSC on PDPS.Id = PDPSC.ProjectSectionId inner join ProjectDevision.ProjectSectionWinners PDPSW on PDPS.Id = PDPSW.ProjectSectionId  And (RankId = 1 Or RankId = 4) inner join [ProjectDefine].[Projects] pr on pr.Id=PDPS.ProjectId  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=pr.Id where 1=1" + query + ") as five").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;
            ViewBag.fourthNumber = selected.four;
            ViewBag.fifthNumber = selected.five;
            ViewBag.sixthNumber = selected.six;
            return PartialView("_C5Chart");
        }

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C6Update(Int32? type, Int32? state, Int32? model)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and p.StateId=" + state;
            if (type != null)
                query += "and ps.AssignmentTypeId=" + type;
            if (model != null)
                query += "and psc.ContractTypeId=" + model;

           List<C6> selected = db.Database.SqlQuery<C6>(@"/****** Script for SelectTopNRows command from SSMS (Tender) ******/
SELECT
abs((((100* ISNULL(pspp.PaidPrice,0))*1.00)/psc.ContractPrice)-100) as b,
p.ProjectTitle as a
  FROM [ProjectDevision].[ProjectSectionContracts] psd
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psd.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
  left join [ProjectDevision].[ProjectSectionPaidPrices] pspp on pspp.ProjectSectionId=ps.Id
  left join [ProjectDevision].[ProjectSectionContracts] psc on psc.ProjectSectionId=ps.Id where 1=1 "+query).ToList();
           int i = 0;
           string projectCosts = "";
            foreach (var item in selected)
           {
               //projectNames += "\"" + selected[i].a + "\",";
               projectCosts += selected[i].b + ",";
               i++;

           }
           if (i > 0)
               ViewBag.q0 = selected[0].a + "-1";
           if (i > 1)
               ViewBag.q1 = selected[1].a + "-2";
           if (i > 2)
               ViewBag.q2 = selected[2].a + "-3";
           if (i > 3)
               ViewBag.q3 = selected[3].a + "-4";
           if (i > 4)
               ViewBag.q4 = selected[4].a + "-5";
           if (i > 5)
               ViewBag.q5 = selected[5].a + "-6";
           if (i > 6)
               ViewBag.q6 = selected[6].a + "-7";
           if (i > 7)
               ViewBag.q7 = selected[7].a + "-8";
           if (i > 8)
               ViewBag.q8 = selected[8].a + "-9";
           if (i > 9)
               ViewBag.q9 = selected[9].a + "-10";
           if (i > 10)
               ViewBag.q10 = selected[10].a + "-11";
           if (i > 11)
               ViewBag.q11 = selected[11].a + "-12";
           if (i > 12)
               ViewBag.q12 = selected[12].a + "-13";
           if (i > 13)
               ViewBag.q13 = selected[13].a + "-14";
           if (i > 14)
               ViewBag.q14 = selected[14].a + "-15";
           if (i > 15)
               ViewBag.q15 = selected[15].a + "-16";
           if (i > 16)
               ViewBag.q16 = selected[16].a + "-17";
           if (i > 17)
               ViewBag.q17 = selected[17].a + "-18";
           if (i > 18)
               ViewBag.q18 = selected[18].a + "-19";
           if (i > 19)
               ViewBag.q19 = selected[19].a + "-20";
           if (i > 20)
               ViewBag.q20 = selected[20].a + "-21";
           if (i > 21)
               ViewBag.q21 = selected[21].a + "-22";
           if (i > 22)
               ViewBag.q22 = selected[22].a + "-23";
           if (i > 23)
               ViewBag.q23 = selected[23].a + "-24";
           if (i > 24)
               ViewBag.q24 = selected[24].a + "-25";
           if (i > 25)
               ViewBag.q25 = selected[25].a + "-26";
           if (i > 26)
               ViewBag.q26 = selected[26].a + "-27";
           if (i > 27)
               ViewBag.q27 = selected[27].a + "-28";
           if (i > 28)
               ViewBag.q28 = selected[28].a + "-29";
           if (i > 29)
               ViewBag.q29 = selected[29].a;
           projectCosts = projectCosts.Remove(projectCosts.Length - 1);
          
           ViewBag.SecoundNumber = projectCosts;
            return PartialView("_C6Chart");
        }

        [AllowAnonymous]

        public PartialViewResult _C6()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.AssignmentType = new List<AssignmentType>() { new AssignmentType() { Title = "انتخاب کنید..." } };
            cpvm.AssignmentType.AddRange(_AssignmentTypeService.Get().ToList());
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            cpvm.ContractType = new List<ContractType>() { new ContractType() { Title = "انتخاب کنید..." } };
            cpvm.ContractType.AddRange(_ContractTypeService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            List<C6> selected = db.Database.SqlQuery<C6>(@"/****** Script for SelectTopNRows command from SSMS (Tender) ******/

SELECT
abs((((100* ISNULL(pspp.PaidPrice,0))*1.00)/psc.ContractPrice)-100) as b,
p.ProjectTitle as a
  FROM [ProjectDevision].[ProjectSectionContracts] psd
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psd.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
  left join [ProjectDevision].[ProjectSectionPaidPrices] pspp on pspp.ProjectSectionId=ps.Id
  left join [ProjectDevision].[ProjectSectionContracts] psc on psc.ProjectSectionId=ps.Id").ToList();
            string projectNames = "";
            string projectCosts = "";
            int i = 0;
            foreach (var item in selected)
            {
                //projectNames += "\"" + selected[i].a + "\",";
                projectCosts += selected[i].b + ",";
                i++;

            }
            if (i > 0)
                ViewBag.q0 = selected[0].a + "-1";
            if (i > 1)
                ViewBag.q1 = selected[1].a + "-2";
            if (i > 2)
                ViewBag.q2 = selected[2].a + "-3";
            if (i > 3)
                ViewBag.q3 = selected[3].a + "-4";
            if (i > 4)
                ViewBag.q4 = selected[4].a + "-5";
            if (i > 5)
                ViewBag.q5 = selected[5].a + "-6";
            if (i > 6)
                ViewBag.q6 = selected[6].a + "-7";
            if (i > 7)
                ViewBag.q7 = selected[7].a + "-8";
            if (i > 8)
                ViewBag.q8 = selected[8].a + "-9";
            if (i > 9)
                ViewBag.q9 = selected[9].a + "-10";
            if (i > 10)
                ViewBag.q10 = selected[10].a + "-11";
            if (i > 11)
                ViewBag.q11 = selected[11].a + "-12";
            if (i > 12)
                ViewBag.q12 = selected[12].a + "-13";
            if (i > 13)
                ViewBag.q13 = selected[13].a + "-14";
            if (i > 14)
                ViewBag.q14 = selected[14].a + "-15";
            if (i > 15)
                ViewBag.q15 = selected[15].a + "-16";
            if (i > 16)
                ViewBag.q16 = selected[16].a + "-17";
            if (i > 17)
                ViewBag.q17 = selected[17].a + "-18";
            if (i > 18)
                ViewBag.q18 = selected[18].a + "-19";
            if (i > 19)
                ViewBag.q19 = selected[19].a + "-20";
            if (i > 20)
                ViewBag.q20 = selected[20].a + "-21";
            if (i > 21)
                ViewBag.q21 = selected[21].a + "-22";
            if (i > 22)
                ViewBag.q22 = selected[22].a + "-23";
            if (i > 23)
                ViewBag.q23 = selected[23].a + "-24";
            if (i > 24)
                ViewBag.q24 = selected[24].a + "-25";
            if (i > 25)
                ViewBag.q25 = selected[25].a + "-26";
            if (i > 26)
                ViewBag.q26 = selected[26].a + "-27";
            if (i > 27)
                ViewBag.q27 = selected[27].a + "-28";
            if (i > 28)
                ViewBag.q28 = selected[28].a + "-29";
            if (i > 29)
                ViewBag.q29 = selected[29].a;
            projectCosts = projectCosts.Remove(projectCosts.Length - 1);
            ViewBag.SecoundNumber = projectCosts;
            var nb = ViewBag.FirstNum;
            return PartialView(cpvm);
        }



        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C7Update(Int32? type, Int32? state, Int32? model)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and p.StateId=" + state;
            if (type != null)
                query += "and ps.AssignmentTypeId=" + type;
            if (model != null)
                query += "and psc.ContractTypeId=" + model;

            List<C8> selected = db.Database.SqlQuery<C8>(@"/****** Script for SelectTopNRows command from SSMS (Tender) ******/

SELECT
psc.ContractPrice- ISNULL(pspp.PaidPrice,0) as b,
p.ProjectTitle as a
  FROM [ProjectDevision].[ProjectSectionContracts] psd
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psd.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
   left join [ProjectDevision].[ProjectSectionPaidPrices] pspp on pspp.ProjectSectionId=ps.Id
  left join [ProjectDevision].[ProjectSectionContracts] psc on psc.ProjectSectionId=ps.Id where 1=1"+query).ToList();
            int i = 0;
            string projectCosts = "";
            foreach (var item in selected)
            {
                //projectNames += "\"" + selected[i].a + "\",";
                projectCosts += selected[i].b + ",";
                i++;

            }
            if (i > 0)
                ViewBag.q0 = selected[0].a + "-1";
            if (i > 1)
                ViewBag.q1 = selected[1].a + "-2";
            if (i > 2)
                ViewBag.q2 = selected[2].a + "-3";
            if (i > 3)
                ViewBag.q3 = selected[3].a + "-4";
            if (i > 4)
                ViewBag.q4 = selected[4].a + "-5";
            if (i > 5)
                ViewBag.q5 = selected[5].a + "-6";
            if (i > 6)
                ViewBag.q6 = selected[6].a + "-7";
            if (i > 7)
                ViewBag.q7 = selected[7].a + "-8";
            if (i > 8)
                ViewBag.q8 = selected[8].a + "-9";
            if (i > 9)
                ViewBag.q9 = selected[9].a + "-10";
            if (i > 10)
                ViewBag.q10 = selected[10].a + "-11";
            if (i > 11)
                ViewBag.q11 = selected[11].a + "-12";
            if (i > 12)
                ViewBag.q12 = selected[12].a + "-13";
            if (i > 13)
                ViewBag.q13 = selected[13].a + "-14";
            if (i > 14)
                ViewBag.q14 = selected[14].a + "-15";
            if (i > 15)
                ViewBag.q15 = selected[15].a + "-16";
            if (i > 16)
                ViewBag.q16 = selected[16].a + "-17";
            if (i > 17)
                ViewBag.q17 = selected[17].a + "-18";
            if (i > 18)
                ViewBag.q18 = selected[18].a + "-19";
            if (i > 19)
                ViewBag.q19 = selected[19].a + "-20";
            if (i > 20)
                ViewBag.q20 = selected[20].a + "-21";
            if (i > 21)
                ViewBag.q21 = selected[21].a + "-22";
            if (i > 22)
                ViewBag.q22 = selected[22].a + "-23";
            if (i > 23)
                ViewBag.q23 = selected[23].a + "-24";
            if (i > 24)
                ViewBag.q24 = selected[24].a + "-25";
            if (i > 25)
                ViewBag.q25 = selected[25].a + "-26";
            if (i > 26)
                ViewBag.q26 = selected[26].a + "-27";
            if (i > 27)
                ViewBag.q27 = selected[27].a + "-28";
            if (i > 28)
                ViewBag.q28 = selected[28].a + "-29";
            if (i > 29)
                ViewBag.q29 = selected[29].a;
            if(projectCosts.Length>2)
            projectCosts = projectCosts.Remove(projectCosts.Length - 1);

            ViewBag.SecoundNumber = projectCosts;
            return PartialView("_C7Chart");
        }

        [AllowAnonymous]

        public PartialViewResult _C7()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.AssignmentType = new List<AssignmentType>() { new AssignmentType() { Title = "انتخاب کنید..." } };
            cpvm.AssignmentType.AddRange(_AssignmentTypeService.Get().ToList());
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            cpvm.ContractType = new List<ContractType>() { new ContractType() { Title = "انتخاب کنید..." } };
            cpvm.ContractType.AddRange(_ContractTypeService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            List<C8> selected = db.Database.SqlQuery<C8>(@"/****** Script for SelectTopNRows command from SSMS (Tender) ******/

SELECT
psc.ContractPrice- ISNULL(pspp.PaidPrice,0) as b,
p.ProjectTitle as a
  FROM [ProjectDevision].[ProjectSectionContracts] psd
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psd.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
  left join [ProjectDevision].[ProjectSectionPaidPrices] pspp on pspp.ProjectSectionId=ps.Id
  left join [ProjectDevision].[ProjectSectionContracts] psc on psc.ProjectSectionId=ps.Id").ToList();
        
            string projectCosts = "";
            int i = 0;
            foreach (var item in selected)
            {
                //projectNames += "\"" + selected[i].a + "\",";
                projectCosts += selected[i].b + ",";
                i++;

            }
            if (i > 0)
                ViewBag.q0 = selected[0].a + "-1";
            if (i > 1)
                ViewBag.q1 = selected[1].a + "-2";
            if (i > 2)
                ViewBag.q2 = selected[2].a + "-3";
            if (i > 3)
                ViewBag.q3 = selected[3].a + "-4";
            if (i > 4)
                ViewBag.q4 = selected[4].a + "-5";
            if (i > 5)
                ViewBag.q5 = selected[5].a + "-6";
            if (i > 6)
                ViewBag.q6 = selected[6].a + "-7";
            if (i > 7)
                ViewBag.q7 = selected[7].a + "-8";
            if (i > 8)
                ViewBag.q8 = selected[8].a + "-9";
            if (i > 9)
                ViewBag.q9 = selected[9].a + "-10";
            if (i > 10)
                ViewBag.q10 = selected[10].a + "-11";
            if (i > 11)
                ViewBag.q11 = selected[11].a + "-12";
            if (i > 12)
                ViewBag.q12 = selected[12].a + "-13";
            if (i > 13)
                ViewBag.q13 = selected[13].a + "-14";
            if (i > 14)
                ViewBag.q14 = selected[14].a + "-15";
            if (i > 15)
                ViewBag.q15 = selected[15].a + "-16";
            if (i > 16)
                ViewBag.q16 = selected[16].a + "-17";
            if (i > 17)
                ViewBag.q17 = selected[17].a + "-18";
            if (i > 18)
                ViewBag.q18 = selected[18].a + "-19";
            if (i > 19)
                ViewBag.q19 = selected[19].a + "-20";
            if (i > 20)
                ViewBag.q20 = selected[20].a + "-21";
            if (i > 21)
                ViewBag.q21 = selected[21].a + "-22";
            if (i > 22)
                ViewBag.q22 = selected[22].a + "-23";
            if (i > 23)
                ViewBag.q23 = selected[23].a + "-24";
            if (i > 24)
                ViewBag.q24 = selected[24].a + "-25";
            if (i > 25)
                ViewBag.q25 = selected[25].a + "-26";
            if (i > 26)
                ViewBag.q26 = selected[26].a + "-27";
            if (i > 27)
                ViewBag.q27 = selected[27].a + "-28";
            if (i > 28)
                ViewBag.q28 = selected[28].a + "-29";
            if (i > 29)
                ViewBag.q29 = selected[29].a+"-30";
            projectCosts = projectCosts.Remove(projectCosts.Length - 1);
          
            ViewBag.SecoundNumber = projectCosts;
            var nb = ViewBag.FirstNum;
            return PartialView(cpvm);
        }



        [AllowAnonymous]
        public PartialViewResult _C9()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            cpvm.ProjectTypes = new List<ProjectType>() { new ProjectType() { Title = "انتخاب کنید..." } };
            cpvm.ProjectTypes.AddRange(_ProjectTypeService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();

            C9 selected = db.Database.SqlQuery<C9>(@"   select top(1)
            
            (select COUNT(p.Id)
 FROM [ProjectDefine].Projects p 
  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=1 ) as one,
           (select COUNT(p.Id)
 FROM [ProjectDefine].Projects p 
  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=2 ) as two,
          (select COUNT(p.Id)
 FROM [ProjectDefine].Projects p 
  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=3) as there,
  (select COUNT(p.Id)
 FROM [ProjectDefine].Projects p 
  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=4) as four,
  (select COUNT(p.Id)
 FROM [ProjectDefine].Projects p 
  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=5) as five,
  (select COUNT(p.Id)
 FROM [ProjectDefine].Projects p 
  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=6  ) as six").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;
            ViewBag.fourthNumber = selected.four;
            ViewBag.fifthNumber = selected.five;
            ViewBag.sixthNumber = selected.six;

            return PartialView(cpvm);
        }
        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C9Update(Int32? state, Int32? type)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and p.StateId=" + state;
            if (type != null)
                query += " and p.ProjectTypeId=" + type;


            C9 selected = db.Database.SqlQuery<C9>(@"select top(1)
            
            (select COUNT(p.Id) FROM [ProjectDefine].Projects p inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=1" + query + " ) as one, (select COUNT(p.Id)  FROM [ProjectDefine].Projects p   inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=2" + query + " ) as two,  (select COUNT(p.Id)  FROM [ProjectDefine].Projects p   inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=3" + query + ") as there, (select COUNT(p.Id)  FROM [ProjectDefine].Projects p   inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=4" + query + ") as four,  (select COUNT(p.Id)  FROM [ProjectDefine].Projects p   inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=5" + query + ") as five,(select COUNT(p.Id)  FROM [ProjectDefine].Projects p  inner join [ProjectDefine].[ProjectPlans] pp on pp.ProjectId=p.Id where pp.CreditProvidePlaceId=6" + query + "  ) as six").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;
            ViewBag.fourthNumber = selected.four;
            ViewBag.fifthNumber = selected.five;
            ViewBag.sixthNumber = selected.six;
            return PartialView("_C9Chart");
        }

        [AllowAnonymous]
        public PartialViewResult _C10()
        {
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            cpvm.AssignmentType = new List<AssignmentType>() { new AssignmentType() { Title = "انتخاب کنید..." } };
            cpvm.AssignmentType.AddRange(_AssignmentTypeService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();

            C9 selected = db.Database.SqlQuery<C9>(@" select top(1)      
            (select COUNT(p.Id)
  FROM [ProjectDevision].[ProjectSectionContracts] psc 
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psc.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
  where psc.ContractTypeId=1 ) as one,
           (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSectionContracts] psc 
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psc.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
  where psc.ContractTypeId=2 ) as two,
          (select COUNT(p.Id)
 FROM [ProjectDevision].[ProjectSectionContracts] psc 
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psc.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
  where psc.ContractTypeId=3) as there").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;

            return PartialView(cpvm);
        }
        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C10Update(Int32? state, Int32? type)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and p.StateId=" + state;
            if (type != null)
                query += " and  ps.AssignmentTypeId=" + type;


            C9 selected = db.Database.SqlQuery<C9>(@"select top(1)      
            (select COUNT(p.Id) FROM [ProjectDevision].[ProjectSectionContracts] psc   inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psc.ProjectSectionId inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId  where psc.ContractTypeId=1" + query + " ) as one,  (select COUNT(p.Id)  FROM [ProjectDevision].[ProjectSectionContracts] psc   inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psc.ProjectSectionId inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId  where psc.ContractTypeId=2" + query + " ) as two, (select COUNT(p.Id)  FROM [ProjectDevision].[ProjectSectionContracts] psc  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psc.ProjectSectionId inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId  where psc.ContractTypeId=3 " + query + ") as there").FirstOrDefault();
            ViewBag.FirstNumber = selected.one;
            ViewBag.SecoundNumber = selected.two;
            ViewBag.ThirdNumber = selected.there;
            return PartialView("_C10Chart");
        }


        [AllowAnonymous]
        public PartialViewResult _Alarms()
        {
            return PartialView();
        }

        [AllowAnonymous]
        public PartialViewResult _Sms()
        {
            return PartialView();
        }





        //---------------------------


        [AllowAnonymous]
        public PartialViewResult _F1()
        {
            return PartialView();
        }


        [AllowAnonymous]
        public PartialViewResult _F2()
        {
            return PartialView();
        }


        [AllowAnonymous]
        public PartialViewResult _F3()
        {
            return PartialView();
        }


        [AllowAnonymous]
        public PartialViewResult _F4()
        {
            return PartialView();
        }


        [AllowAnonymous]
        public PartialViewResult _F5()
        {
            return PartialView();
        }


        [AllowAnonymous]
        public PartialViewResult _F6()
        {
            return PartialView();
        }

       
      
        [AllowAnonymous]

        public PartialViewResult _C8()
        {
          
            CreateProjectViewModel cpvm = new CreateProjectViewModel();
            cpvm.AssignmentType = new List<AssignmentType>() { new AssignmentType() { Title = "انتخاب کنید..." } };
            cpvm.AssignmentType.AddRange(_AssignmentTypeService.Get().ToList());
            cpvm.States = new List<State>() { new State() { Title = "انتخاب کنید..." } };
            cpvm.States.AddRange(_StateService.Get().ToList());
            cpvm.ContractType = new List<ContractType>() { new ContractType() { Title = "انتخاب کنید..." } };
            cpvm.ContractType.AddRange(_ContractTypeService.Get().ToList());
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            List<C8> selected = db.Database.SqlQuery<C8>(@"/****** Script for SelectTopNRows command from SSMS (Tender) ******/

SELECT
psd.ContractPrice as b,
p.ProjectTitle as a
  FROM [ProjectDevision].[ProjectSectionContracts] psd
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psd.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId").ToList();

            string projectCosts = "";
            int i = 0;
            foreach (var item in selected)
            {
                //projectNames += "\"" + selected[i].a + "\",";
                projectCosts += selected[i].b + ",";
                i++;

            }
            if (i > 0)
                ViewBag.q0 = selected[0].a + "-1";
            if (i > 1)
                ViewBag.q1 = selected[1].a + "-2";
            if (i > 2)
                ViewBag.q2 = selected[2].a + "-3";
            if (i > 3)
                ViewBag.q3 = selected[3].a + "-4";
            if (i > 4)
                ViewBag.q4 = selected[4].a + "-5";
            if (i > 5)
                ViewBag.q5 = selected[5].a + "-6";
            if (i > 6)
                ViewBag.q6 = selected[6].a + "-7";
            if (i > 7)
                ViewBag.q7 = selected[7].a + "-8";
            if (i > 8)
                ViewBag.q8 = selected[8].a + "-9";
            if (i > 9)
                ViewBag.q9 = selected[9].a + "-10";
            if (i > 10)
                ViewBag.q10 = selected[10].a + "-11";
            if (i > 11)
                ViewBag.q11 = selected[11].a + "-12";
            if (i > 12)
                ViewBag.q12 = selected[12].a + "-13";
            if (i > 13)
                ViewBag.q13 = selected[13].a + "-14";
            if (i > 14)
                ViewBag.q14 = selected[14].a + "-15";
            if (i > 15)
                ViewBag.q15 = selected[15].a + "-16";
            if (i > 16)
                ViewBag.q16 = selected[16].a + "-17";
            if (i > 17)
                ViewBag.q17 = selected[17].a + "-18";
            if (i > 18)
                ViewBag.q18 = selected[18].a + "-19";
            if (i > 19)
                ViewBag.q19 = selected[19].a + "-20";
            if (i > 20)
                ViewBag.q20 = selected[20].a + "-21";
            if (i > 21)
                ViewBag.q21 = selected[21].a + "-22";
            if (i > 22)
                ViewBag.q22 = selected[22].a + "-23";
            if (i > 23)
                ViewBag.q23 = selected[23].a + "-24";
            if (i > 24)
                ViewBag.q24 = selected[24].a + "-25";
            if (i > 25)
                ViewBag.q25 = selected[25].a + "-26";
            if (i > 26)
                ViewBag.q26 = selected[26].a + "-27";
            if (i > 27)
                ViewBag.q27 = selected[27].a + "-28";
            if (i > 28)
                ViewBag.q28 = selected[28].a + "-29";
            if (i > 29)
                ViewBag.q29 = selected[29].a + "-30";
            projectCosts = projectCosts.Remove(projectCosts.Length - 1);

            ViewBag.SecoundNumber = projectCosts;
            var nb = ViewBag.FirstNum;
            return PartialView(cpvm);
        }

        [AllowAnonymous]
        [HttpGet]
        public PartialViewResult _C8Update(Int32? type, Int32? state, Int32? model)
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            string query = "";
            if (state != null)
                query += " and p.StateId=" + state;
            if (type != null)
                query += "and ps.AssignmentTypeId=" + type;
            if (model != null)
                query += "and psc.ContractTypeId=" + model;

            List<C8> selected = db.Database.SqlQuery<C8>(@"/****** Script for SelectTopNRows command from SSMS (Tender) ******/
SELECT
psd.ContractPrice as b,
p.ProjectTitle as a
  FROM [ProjectDevision].[ProjectSectionContracts] psd
  inner join [ProjectDevision].[ProjectSections] ps on ps.Id=psd.ProjectSectionId
  inner join [ProjectDefine].[Projects] p on p.Id=ps.ProjectId
   left join [ProjectDevision].[ProjectSectionPaidPrices] pspp on pspp.ProjectSectionId=ps.Id
  left join [ProjectDevision].[ProjectSectionContracts] psc on psc.ProjectSectionId=ps.Id where 1=1" + query).ToList();
            int i = 0;
            string projectCosts = "";
            foreach (var item in selected)
            {
                //projectNames += "\"" + selected[i].a + "\",";
                projectCosts += selected[i].b + ",";
                i++;

            }
            if (i > 0)
                ViewBag.q0 = selected[0].a + "-1";
            if (i > 1)
                ViewBag.q1 = selected[1].a + "-2";
            if (i > 2)
                ViewBag.q2 = selected[2].a + "-3";
            if (i > 3)
                ViewBag.q3 = selected[3].a + "-4";
            if (i > 4)
                ViewBag.q4 = selected[4].a + "-5";
            if (i > 5)
                ViewBag.q5 = selected[5].a + "-6";
            if (i > 6)
                ViewBag.q6 = selected[6].a + "-7";
            if (i > 7)
                ViewBag.q7 = selected[7].a + "-8";
            if (i > 8)
                ViewBag.q8 = selected[8].a + "-9";
            if (i > 9)
                ViewBag.q9 = selected[9].a + "-10";
            if (i > 10)
                ViewBag.q10 = selected[10].a + "-11";
            if (i > 11)
                ViewBag.q11 = selected[11].a + "-12";
            if (i > 12)
                ViewBag.q12 = selected[12].a + "-13";
            if (i > 13)
                ViewBag.q13 = selected[13].a + "-14";
            if (i > 14)
                ViewBag.q14 = selected[14].a + "-15";
            if (i > 15)
                ViewBag.q15 = selected[15].a + "-16";
            if (i > 16)
                ViewBag.q16 = selected[16].a + "-17";
            if (i > 17)
                ViewBag.q17 = selected[17].a + "-18";
            if (i > 18)
                ViewBag.q18 = selected[18].a + "-19";
            if (i > 19)
                ViewBag.q19 = selected[19].a + "-20";
            if (i > 20)
                ViewBag.q20 = selected[20].a + "-21";
            if (i > 21)
                ViewBag.q21 = selected[21].a + "-22";
            if (i > 22)
                ViewBag.q22 = selected[22].a + "-23";
            if (i > 23)
                ViewBag.q23 = selected[23].a + "-24";
            if (i > 24)
                ViewBag.q24 = selected[24].a + "-25";
            if (i > 25)
                ViewBag.q25 = selected[25].a + "-26";
            if (i > 26)
                ViewBag.q26 = selected[26].a + "-27";
            if (i > 27)
                ViewBag.q27 = selected[27].a + "-28";
            if (i > 28)
                ViewBag.q28 = selected[28].a + "-29";
            if (i > 29)
                ViewBag.q29 = selected[29].a;
            if (projectCosts.Length > 2)
                projectCosts = projectCosts.Remove(projectCosts.Length - 1);

            ViewBag.SecoundNumber = projectCosts;
            return PartialView("_C8Chart");
        }




    }
}