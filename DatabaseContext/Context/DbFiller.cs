using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using Infrastracture.AppCode;

namespace DatabaseContext.Context
{
    public class DbFiller
    {
        #region CreateProcedures

        public void CreateProcedures(DatabaseContext context)
        {
            // ---------------------------Elmah
            string CalcCommissionsCommand = string.Empty;

            string projectRootPath = System.AppDomain.CurrentDomain.BaseDirectory.Replace(@"bin\Debug\", "");
            var reader = new StreamReader(projectRootPath + "Elmah.sql");
            CalcCommissionsCommand = reader.ReadToEnd();

            context.Database.ExecuteSqlCommand(CalcCommissionsCommand);
        }

        #endregion


        #region InsertBaseData
        public void InsertBaseData(DatabaseContext context)
        {
            ///////////////////////// Add ProjectTypes / نوع پروژه ///////////////////////////////
            if (!context.ProjectTypes.Any())
            {
                var projectTypes = new ProjectType[]
                {
                    new ProjectType(){ Title = "احداث" },
                    new ProjectType(){ Title = "تعمیرات و تجهیز" },
                };

                context.ProjectTypes.AddOrUpdate(projectTypes);
                context.SaveChanges();
            }

            ///////////////////////// Add ExecutionTypes / نحوه اجرا ///////////////////////////////
            if (!context.ExecutionTypes.Any())
            {
                var executionTypes = new ExecutionType[]
                {
                    new ExecutionType(){ Title = "پیمانی" },
                    new ExecutionType(){ Title = "امانی" },
                    new ExecutionType(){ Title = "پیمانی / امانی" },
                };

                context.ExecutionTypes.AddOrUpdate(executionTypes);
                context.SaveChanges();
            }

            ///////////////////////// Add OwnershipTypes / وضعیت مالکین ///////////////////////////////
            if (!context.OwnershipTypes.Any())
            {
                var ownershipTypes = new OwnershipType[]
                {
                    new OwnershipType(){ Title = "سند مالکیت" },
                    new OwnershipType(){ Title = "صورت جلسه واگذاری" },
                    new OwnershipType(){ Title = "اوقافی" },
                };

                context.OwnershipTypes.AddOrUpdate(ownershipTypes);
                context.SaveChanges();
            }

            ///////////////////////// Add AssignmentTypes / نحوه واگذاری ///////////////////////////////
            if (!context.AssignmentTypes.Any())
            {
                var assignmentTypes = new AssignmentType[]
                {
                    new AssignmentType(){ Title = "استعلام" },
                    new AssignmentType(){ Title = "مناقصه" },
                    new AssignmentType(){ Title = "ترک تشریفات" },
                };

                context.AssignmentTypes.AddOrUpdate(assignmentTypes);
                context.SaveChanges();
            }


            ///////////////////////// Add ServiceProduct / کالا / خدمات///////////////////////////////
            if (!context.ServiceProduct.Any())
            {
                var serviceProduct = new ServiceProduct[]
                {
                    new ServiceProduct(){ Title = "کالا" },
                    new ServiceProduct(){ Title = "خدمات" },
                };

                context.ServiceProduct.AddOrUpdate(serviceProduct);
                context.SaveChanges();
            }


            ///////////////////////// Add Ranks / مقام ///////////////////////////////
            if (!context.Ranks.Any())
            {
                var ranks = new Rank[]
                {
                    new Rank(){ Title = "اول" },
                    new Rank(){ Title = "دوم" },
                    new Rank(){ Title = "سوم" },
                };

                context.Ranks.AddOrUpdate(ranks);
                context.SaveChanges();
            }

            ///////////////////////// Add ContractTypes / نوع قرارداد ///////////////////////////////
            if (!context.ContractTypes.Any())
            {
                var contractTypes = new ContractType[]
                {
                    new ContractType(){ Title = "کارهای ساختمانی به صورت سرجمع" },
                    new ContractType(){ Title = "کارهای ساختمانی براساس فهرست بها" },
                    new ContractType(){ Title = "سایر" },
                };

                context.ContractTypes.AddOrUpdate(contractTypes);
                context.SaveChanges();
            }

            ///////////////////////// Add Opinions / نظر ///////////////////////////////
            if (!context.Opinions.Any())
            {
                var opinions = new Opinion[]
                {
                    new Opinion(){ Title = "تایید" },
                    new Opinion(){ Title = "عدم تایید" },
                };

                context.Opinions.AddOrUpdate(opinions);
                context.SaveChanges();
            }

            ///////////////////////// Add TempFixed / موقت / قطعی ///////////////////////////////
            if (!context.TempFixed.Any())
            {
                var tempFixed = new TempFixed[]
                {
                    new TempFixed(){ Title = "موقت" },
                    new TempFixed(){ Title = "قطعی" },
                };

                context.TempFixed.AddOrUpdate(tempFixed);
                context.SaveChanges();
            }

            ///////////////////////// Add StatementNumbers / شماره صورت وضعیت ///////////////////////////////
            if (!context.StatementNumbers.Any())
            {
                List<StatementNumber>  statementNumbers = new List<StatementNumber>();

                for (int i = 1; i <= 100; i++)
			    {
			        statementNumbers.Add(new StatementNumber(){ Title ="موقت " + i, TempFixedId = 1 });
			    }

                statementNumbers.Add(new StatementNumber() { Title = "قطعی", TempFixedId = 2 });
                statementNumbers.Add(new StatementNumber() { Title = "قطعی ماده 46", TempFixedId = 2 });
                statementNumbers.Add(new StatementNumber() { Title = "قطعی ماده 48", TempFixedId = 2 });

                context.StatementNumbers.AddRange(statementNumbers);
                context.SaveChanges();
            }

            ///////////////////////// Add Defects / نواقص ///////////////////////////////
            if (!context.Defects.Any())
            {
                var defects = new Defect[]
                {
                    new Defect(){ Title = "دارد" },
                    new Defect(){ Title = "ندارد" },
                };

                context.Defects.AddOrUpdate(defects);
                context.SaveChanges();
            }

            ///////////////////////// Add LetterTypes / نوع نامه ///////////////////////////////
            if (!context.LetterTypes.Any())
            {
                var letterTypes = new LetterType[]
                {
                    new LetterType(){ Title = "وارده" },
                    new LetterType(){ Title = "صادره" },
                    new LetterType(){ Title = "داخلی" },
                };

                context.LetterTypes.AddOrUpdate(letterTypes);
                context.SaveChanges();
            }

            ///////////////////////// Add ProjectStates / وضعیت پروژه ///////////////////////////////
            if (!context.ProjectStates.Any())
            {
                var projectStates = new ProjectState[]
                {
                    new ProjectState(){ Title = "منتظر ثبت موافقت نامه", Color = "#00ff00" },
                    new ProjectState(){ Title = "منتظر تخصیص", Color = "#ff0000" },
                    new ProjectState(){ Title = "منتظر تقسیم بندی پروژه", Color = "#0000ff" },
                    new ProjectState(){ Title = "تقسیم بندی انجام شد", Color = "#f0ff0f" },
                };

                context.ProjectStates.AddOrUpdate(projectStates);
                context.SaveChanges();
            }

            ///////////////////////// Add ProjectSectionAssignmentStates / وضعیت کار در عملکرد پروژه ///////////////////////////////
            if (!context.ProjectSectionAssignmentStates.Any())
            {
                var projectSectionAssignmentState = new ProjectSectionAssignmentState[]
                {
                    new ProjectSectionAssignmentState(){ Title = "منتظر اعلام استعلام", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر اعلام مناقصه", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر ترک تشریفات", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر شرکت کنندگان", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر ارزیابی فنی و مالی", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر شرکت کنندگان نهایی", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر تاریخ بازگشایی", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر انتخاب برنده", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "منتظر مدارک برنده", Color = "#00ff00" },
                    new ProjectSectionAssignmentState(){ Title = "قرارداد", Color = "#00ff00" },

                };

                context.ProjectSectionAssignmentStates.AddOrUpdate(projectSectionAssignmentState);
                context.SaveChanges();
            }

            ///////////////////////// Add ProjectSectionOperationStates / وضعیت کار در عملکرد پروژه ///////////////////////////////
            if (!context.ProjectSectionOperationStates.Any())
            {
                var projectSectionOperationStates = new ProjectSectionOperationState[]
                {
                    new ProjectSectionOperationState(){ Title = "نامشخص...", Color = "#00ff00" },
                    new ProjectSectionOperationState(){ Title = "منتظر تایید زمانبندی", Color = "#00ff00" },
                    new ProjectSectionOperationState(){ Title = "منتظر انتخاب ناظر/مشاور", Color = "#00ff00" },
                    new ProjectSectionOperationState(){ Title = "منتظر تایید صورت وضعیت", Color = "#00ff00" },
                    new ProjectSectionOperationState(){ Title = "منتظر ثبت فرم حواله", Color = "#00ff00" },
                    new ProjectSectionOperationState(){ Title = "منتظر پرداخت صورت وضعیت", Color = "#00ff00" },
                    new ProjectSectionOperationState(){ Title = "منتظر تحویل قطعی / آزادسازی سپرده", Color = "#00ff00" },
                    new ProjectSectionOperationState(){ Title = "منتظر آزادسازی سپرده", Color = "#00ff00" },
                };

                context.ProjectSectionOperationStates.AddOrUpdate(projectSectionOperationStates);
                context.SaveChanges();
            }


            ///////////////////////// Add MemberGroup / گروه کاربری ///////////////////////////////
            if (!context.MemberGroups.Any())
            {
                var memberGroups = new MemberGroup[]
                {
                    new MemberGroup(){ Title = "پرسنل" },
                    new MemberGroup(){ Title = "ناظر" },
                    new MemberGroup(){ Title = "مشاور" },
                };

                context.MemberGroups.AddOrUpdate(memberGroups);
                context.SaveChanges();
            }
            
            ///////////////////////// Add Role / نقش ///////////////////////////////
            if (!context.Roles.Any())
            {
                var roles = new Role[]
                {
                    new Role(){ Title = "مدیر سیستم", IsActive = true, Code = "1", Level = 1},
                    new Role(){ Title = "مدیر ارشد", IsActive = true, Code = "1-2", Level = 2, ParentId = 1},
                    new Role(){ Title = "ناظر", IsActive = true, Code = "1-3", Level = 2, ParentId = 1},
                    new Role(){ Title = "مشاور", IsActive = true, Code = "1-4", Level = 2, ParentId = 1},
                    new Role(){ Title = "کارشناس بودجه", IsActive = true, Code = "1-5", Level = 2, ParentId = 1},
                    new Role(){ Title = "مدیر واحد مهندسی", IsActive = true, Code = "1-6", Level = 2, ParentId = 1},
                    new Role(){ Title = "امور قراردادها", IsActive = true, Code = "1-7", Level = 2, ParentId = 1},
                    new Role(){ Title = "ذی حساب", IsActive = true, Code = "1-8", Level = 2, ParentId = 1},
                    new Role(){ Title = "مالی", IsActive = true, Code = "1-9", Level = 2, ParentId = 1},
                };

                context.Roles.AddOrUpdate(roles);
                context.SaveChanges();
            }

            ///////////////////////// Add Members / مدیریت ///////////////////////////////
            if (!context.Members.Any())
            {
                var members = new Member[]
                {
                    new Member(){ MemberGroupId = 1, RoleId = 1, FirstName = "شرکت iTeam", LastName = "خبره", UserName = "a", Password = Hashing.MultiEncrypt("a"), PasswordCampare = Hashing.MultiEncrypt("a"), FatherName = "father", BirthdayDate = "1390/01/01", Job = "", Gender = true, NationalCode = "", Email = "", Address = "", CartNumber = "", Description  = "", IsActive = true },
                };
                context.Members.AddOrUpdate(members);
                context.SaveChanges();
            }

            ///////////////////////// Add DeductionPercents / درصد کسورات ///////////////////////////////
            if (!context.DeductionPercents.Any())
            {
                var deductionPercent = new DeductionPercent[]
                {
                    new DeductionPercent(){ Title="بیمه سهم کارفرما", PercentAmount = 10 },
                    new DeductionPercent(){ Title="بیمه سهم پیمانکار", PercentAmount = 10 },
                    new DeductionPercent(){ Title="مالیات", PercentAmount = 10 },
                    new DeductionPercent(){ Title="مالیات بر ارزش افزوده", PercentAmount = 10 },
                    new DeductionPercent(){ Title="سپرده", PercentAmount = 10 },
                };
                context.DeductionPercents.AddOrUpdate(deductionPercent);
                context.SaveChanges();
            }

            ///////////////////////// Add FileTypes / انواع فایل ها///////////////////////////////
            if (!context.FileTypes.Any())
            {
                var fileType = new FileType[]
                {
                    new FileType(){ Title = "فایل های پروژه" }
                };
                context.FileTypes.AddOrUpdate(fileType);
                context.SaveChanges();
            }
        }

        #endregion


        #region InsertTestData
        public void InsertTestData(DatabaseContext context)
        {

        }

        #endregion
    }
}
