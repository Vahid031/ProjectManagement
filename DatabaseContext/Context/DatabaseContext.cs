using System;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using DomainModels.Entities.BaseInformation;
using DomainModels.Entities.General;
using DomainModels.Entities.ProjectDefine;
using DomainModels.Entities.ProjectDevision;

namespace DatabaseContext.Context
{
    public class DatabaseContext : DbContext, IUnitOfWork
    {
        static DatabaseContext()
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
            Database.SetInitializer<DatabaseContext>(null);
            
        }

        //protected override void OnModelCreating(DbModelBuilder builder)
        //{
        //    Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        //}

        public DatabaseContext(): base("ProjectManagement")
        { 
            
        }


        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<FileType> FileTypes { get; set; }
        public DbSet<AssignmentType> AssignmentTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CommissionMember> CommissionMembers { get; set; }
        public DbSet<Contractor> Contractors { get; set; }
        public DbSet<ContractorLevel> ContractorLevels { get; set; }
        public DbSet<ContractType> ContractTypes { get; set; }
        public DbSet<CreditProvidePlace> CreditProvidePlaces { get; set; }
        public DbSet<DeductionPercent> DeductionPercents { get; set; }
        public DbSet<Defect> Defects { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<ExecutionType> ExecutionTypes { get; set; }
        public DbSet<FinantialYear> FinantialYears { get; set; }
        public DbSet<InvestmentChapter> InvestmentChapters { get; set; }
        public DbSet<InvestmentChapterType> InvestmentChapterTypes { get; set; }
        public DbSet<LetterType> LetterTypes { get; set; }
        public DbSet<LogDetail> LogDetails { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<MemberGroup> MemberGroups { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MonitoringType> MonitoringTypes { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Opinion> Opinions { get; set; }
        public DbSet<OwnershipType> OwnershipTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<PlanType> PlanTypes { get; set; }
        public DbSet<ProgramPlan> ProgramPlans { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<ProjectAllocate> ProjectAllocates { get; set; }
        public DbSet<ProjectAverage> ProjectAverages { get; set; }
        public DbSet<ProjectFile> ProjectFiles { get; set; }
        public DbSet<ProjectFunding> ProjectFundings { get; set; }
        public DbSet<ProjectFundingFile> ProjectFundingFiles { get; set; }
        public DbSet<ProjectInvestmentChapter> ProjectInvestmentChapters { get; set; }
        public DbSet<ProjectQuantityGoal> ProjectQuantityGoals { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectPlan> ProjectPlans { get; set; }
        public DbSet<ProjectSectionAdvisorVisitFile> ProjectSectionAdvisorVisitFiles { get; set; }
        public DbSet<ProjectSectionAdvisorVisit> ProjectSectionAdvisorVisits { get; set; }
        public DbSet<ProjectSectionAssignmentState> ProjectSectionAssignmentStates { get; set; }
        public DbSet<ProjectSectionCeremonialFile> ProjectSectionCeremonialFiles { get; set; }
        public DbSet<ProjectSectionCeremonial> ProjectSectionCeremonials { get; set; }
        public DbSet<ProjectSectionInquiryFile> ProjectSectionInquiryFiles { get; set; }
        public DbSet<ProjectSectionInquiry> ProjectSectionInquiries { get; set; }
        public DbSet<ProjectSectionOperationTitle> ProjectSectionOperationTitles { get; set; }
        public DbSet<ProjectSectionComplementarityDateFile> ProjectSectionComplementarityDateFiles { get; set; }
        public DbSet<ProjectSectionComplementarityDate> ProjectSectionComplementarityDates { get; set; }
        public DbSet<ProjectSectionComplementarityPriceFile> ProjectSectionComplementarityPriceFiles { get; set; }
        public DbSet<ProjectSectionComplementarityPrice> ProjectSectionComplementarityPrices { get; set; }
        public DbSet<ProjectSectionContractDetail> ProjectSectionContractDetails { get; set; }
        public DbSet<ProjectSectionContractFile> ProjectSectionContractFiles { get; set; }
        public DbSet<ProjectSectionContract> ProjectSectionContracts { get; set; }
        public DbSet<ProjectSectionCorrespondence> ProjectSectionCorrespondences { get; set; }
        public DbSet<ProjectSectionCorrespondenceFile> ProjectSectionCorrespondencFiles { get; set; }
        public DbSet<ProjectSectionDeliveryFixd> ProjectSectionDeliveryFixdes { get; set; }
        public DbSet<ProjectSectionDeliveryFixdFile> ProjectSectionDeliveryFixdFiles { get; set; }
        public DbSet<ProjectSectionDeliveryTemprory> ProjectSectionDeliveryTemprories { get; set; }
        public DbSet<ProjectSectionDeliveryTemproryFile> ProjectSectionDeliveryTemproryFiles { get; set; }
        public DbSet<ProjectSectionDepositeLiberalizationFile> ProjectSectionDepositeLiberalizationFiles { get; set; }
        public DbSet<ProjectSectionDepositeLiberalization> ProjectSectionDepositeLiberalizations { get; set; }
        public DbSet<ProjectSectionDraftFile> ProjectSectionDraftFiles { get; set; }
        public DbSet<ProjectSectionDraft> ProjectSectionDrafts { get; set; }
        public DbSet<ProjectSectionFinalParticipantFile> ProjectSectionFinalParticipantFiles { get; set; }
        public DbSet<ProjectSectionFinalParticipant> ProjectSectionFinalParticipants { get; set; }
        public DbSet<ProjectSectionFinanceEvaluation> ProjectSectionFinanceEvaluations { get; set; }
        public DbSet<ProjectSectionMadeh46File> ProjectSectionMadeh46Files { get; set; }
        public DbSet<ProjectSectionMadeh46> ProjectSectionMadeh46s { get; set; }
        public DbSet<ProjectSectionMadeh48File> ProjectSectionMadeh48Files { get; set; }
        public DbSet<ProjectSectionMadeh48> ProjectSectionMadeh48s { get; set; }
        public DbSet<ProjectSectionMember> ProjectSectionMembers { get; set; }
        public DbSet<ProjectSectionOpenDate> ProjectSectionOpenDates { get; set; }
        public DbSet<ProjectSectionOperationState> ProjectSectionOperationStates { get; set; }
        public DbSet<ProjectSectionStatementState> ProjectSectionStatementStates { get; set; }
        public DbSet<ProjectSectionPaidPriceFile> ProjectSectionPaidPriceFiles { get; set; }
        public DbSet<ProjectSectionPaidPrice> ProjectSectionPaidPrices { get; set; }
        public DbSet<ProjectSectionParticipant> ProjectSectionParticipants { get; set; }
        public DbSet<ProjectSectionProduct> ProjectSectionProducts { get; set; }
        public DbSet<ProjectSectionProductFile> ProjectSectionProductFiles { get; set; }
        public DbSet<ProjectSectionRecoupmentFile> ProjectSectionRecoupmentFiles { get; set; }
        public DbSet<ProjectSectionRecoupment> ProjectSectionRecoupments { get; set; }
        public DbSet<ProjectSection> ProjectSections { get; set; }
        public DbSet<ProjectSectionScheduleFile> ProjectSectionScheduleFiles { get; set; }
        public DbSet<ProjectSectionSchedule> ProjectSectionSchedules { get; set; }
        public DbSet<ProjectSectionStatementConfirm> ProjectSectionStatementConfirms { get; set; }
        public DbSet<ProjectSectionStatementConfirmFile> ProjectSectionStatementConfirmFiles { get; set; }
        public DbSet<ProjectSectionStatementFile> ProjectSectionStatementFiles { get; set; }
        public DbSet<ProjectSectionStatement> ProjectSectionStatements { get; set; }
        public DbSet<ProjectSectionSupervisorVisitFile> ProjectSectionSupervisorVisitFiles { get; set; }
        public DbSet<ProjectSectionSupervisorVisit> ProjectSectionSupervisorVisits { get; set; }
        public DbSet<ProjectSectionTenderFile> ProjectSectionTenderFiles { get; set; }
        public DbSet<ProjectSectionTender> ProjectSectionTenders { get; set; }
        public DbSet<ProjectSectionWinnerDegree> ProjectSectionWinnerDegrees { get; set; }
        public DbSet<ProjectSectionWinnerDegreeFile> ProjectSectionWinnerDegreeFiles { get; set; }
        public DbSet<ProjectSectionWinner> ProjectSectionWinners { get; set; }
        public DbSet<ProjectState> ProjectStates { get; set; }
        public DbSet<ProjectType> ProjectTypes { get; set; }
        public DbSet<Rank> Ranks { get; set; }
        public DbSet<RecoupmentType> RecoupmentTypes { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceType> ResourceTypes { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RuralDistrict> RuralDistricts { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ServiceProduct> ServiceProduct { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<StatementNumber> StatementNumbers { get; set; }
        public DbSet<StatementType> StatementTypes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<TempFixed> TempFixed { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Village> Villages { get; set; }
        public DbSet<LevelType> LevelTypes { get; set; }
        public DbSet<MajorType> MajorTypes { get; set; }
        public DbSet<ProjectAllocateFile> ProjectAllocateFiles { get; set; }
        public DbSet<ProjectSectionGovermentWorthyDocumentsPaidPrice> ProjectSectionGovermentWorthyDocumentsPaidPrice { get; set; }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        

        public int SaveChanges(Int64 MemberId)
        {
            using (var scope = this.Database.BeginTransaction())
            {
                try
                {
                    var addedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
                    var modifiedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
                    var deletedEntries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();

                    foreach (var entry in modifiedEntries)
                    {
                        //ApplyAuditLog(entry, "Modified", MemberId);
                    }

                    foreach (var entry in deletedEntries)
                    {
                        //ApplyAuditLog(entry, "Deleted", MemberId);
                    }

                    int changes = base.SaveChanges();

                    foreach (var entry in addedEntries)
                    {
                        //ApplyAuditLog(entry, "Added", MemberId);
                    }

                    changes += base.SaveChanges();
                    scope.Commit();

                    return changes;
                }
                catch (Exception)
                {
                    scope.Rollback();
                    return 0;
                }
            }
        }
        



        private void ApplyAuditLog(DbEntityEntry entry, string operationName, Int64 MemberId)
        {
            Log log = new Log();
            log.Id = -1;
            log.MemberId = MemberId;
            log.TableName = entry.Entity.GetType().Name;
            log.OperationName = operationName;
            log.Date = DateTime.Now;
            log.RowId = Convert.ToInt64(entry.Entity.GetType().GetProperty("Id").GetValue(entry.Entity));

            Logs.Add(log);

            foreach (var item in entry.Entity.GetType().GetProperties())
            {
                LogDetail logDetail = new LogDetail();
                logDetail.LogId = log.Id;
                logDetail.PropertyName = item.Name;
                if (item.GetValue(entry.Entity) == null)
                    logDetail.PropertyValue = "";
                else
                    logDetail.PropertyValue = item.GetValue(entry.Entity).ToString();

                LogDetails.Add(logDetail);
            }
        }
        

        public DatabaseContext GetInstance() 
        {
            return this;
        }
    }
}
