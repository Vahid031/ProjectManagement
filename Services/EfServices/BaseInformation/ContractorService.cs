using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.BaseInformation;
using Services.Interfaces.General;
using Services.Interfaces.BaseInformation;
using ViewModels.BaseInformation.ContractorViewModel;

namespace Services.EfServices.BaseInformation
{
    public class ContractorService : GenericService<Contractor>, IContractorService
    {
        public ContractorService(IUnitOfWork uow)
            : base(uow)
        {

        }
        public IEnumerable<ListContractorViewModel> GetAll(ListContractorViewModel lcvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("Contractor.", "");
            DynamicFiltering.GetFilter<ListContractorViewModel>(lcvm, ref pg);
            pg._filter = pg._filter.Replace("Contractor.", "");

            var a = _uow.Set<Contractor>().AsQueryable<Contractor>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListContractorViewModel()
            {
                Contractor = new Contractor() { Id = Result.Id, Address = Result.Address, AgentFirstName = Result.AgentFirstName, AgentLastName = Result.AgentLastName, AgentMobileNumber = Result.AgentMobileNumber, CompanyName = Result.CompanyName, Email = Result.Email, FaxNumber = Result.FaxNumber, IsActive = Result.IsActive, ManagerFirstName = Result.ManagerFirstName, ManagerLastName = Result.ManagerLastName, ManagerMobileNumber = Result.ManagerMobileNumber, ManagerNationalCode = Result.ManagerNationalCode, PhoneNumber = Result.PhoneNumber, Website = Result.Website }
            });
        }


        public IEnumerable<ReportContractorViewModel> GetAllReport(ReportParameterContractorViewModel parameters)
        {
            string tempFilter = " ";
            string and = "";
            
            if (parameters.ContractorType != null)
            {
                tempFilter += and;
                tempFilter += "Result.ContractorType Like N'" + parameters.ContractorType + "'";
                and = " AND ";
            }

            if (parameters.IsActive != null)
            {
                tempFilter += and;
                tempFilter += "Result.IsActive = '" + parameters.IsActive.Value.ToString() + "'";
                and = " AND ";
            }

            if (parameters.LevelTypeId != null)
            {
                tempFilter += and;
                tempFilter += "Result.Id in (Select ContractorId from BaseInformation.ContractorLevels Where LevelTypeId = " + parameters.LevelTypeId + ")";
                and = " AND ";
            }

            if (parameters.MajorTypeId != null)
            {
                tempFilter += and;
                tempFilter += "Result.Id in (Select ContractorId from BaseInformation.ContractorLevels Where MajorTypeId = " + parameters.MajorTypeId + ")";
                and = " AND ";
            }

            string filter = "";
            if (!String.IsNullOrEmpty(tempFilter.Trim()))
                filter = " where " + tempFilter;

            string allField = @"SELECT * FROM (";
            string query = "";
            
            query += @"Select Id, ContractorType, CompanyName, ManagerFirstName + ' ' + ManagerLastName ManagerName, ManagerMobileNumber, 
                       	   [Address], Website, Email, PhoneNumber, FaxNumber, IsActive, case IsActive when '1' then 'فعال' else 'غیر فعال' end IsActiveTitle
                       From BaseInformation.Contractors BIC
                       ) Result"
            + filter;
            
            IEnumerable<ReportContractorViewModel> result = _uow.GetInstance().Database.SqlQuery<ReportContractorViewModel>(allField + query)
                .Select(Result => new ReportContractorViewModel()
                {
                    Address = Result.Address,
                    CompanyName = Result.CompanyName,
                    ContractorType = Result.ContractorType,
                    Email = Result.Email,
                    FaxNumber = Result.FaxNumber,
                    IsActive = Result.IsActive,
                    IsActiveTitle = Result.IsActiveTitle,
                    ManagerMobileNumber = Result.ManagerMobileNumber,
                    ManagerName = Result.ManagerName,
                    PhoneNumber = Result.PhoneNumber,
                    Website = Result.Website
                });

            return result;
        }


        public List<Contractor> SelectAll()
        {
            DatabaseContext.Context.DatabaseContext db = new DatabaseContext.Context.DatabaseContext();
            return db.Contractors.ToList();
        }
    }
}
