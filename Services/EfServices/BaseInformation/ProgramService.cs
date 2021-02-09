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
using ViewModels.BaseInformation.ProgramViewModel;

namespace Services.EfServices.BaseInformation
{
    public class ProgramService : GenericService<Program>, IProgramService
    {
        public ProgramService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProgramViewModel> GetAll(ListProgramViewModel lpvm, ref Paging pg)
        {
            DynamicFiltering.GetFilter<ListProgramViewModel>(lpvm, ref pg);

            var a =
            _uow.Set<Program>()
                .Select(Program => new { Program });
                //.Join(_uow.Set<FinantialYear>(), Program => Program.FinantialYearId, FinantialYear => FinantialYear.Id, (Program, FinantialYear) => new { Program, FinantialYear});

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);

            return a.AsEnumerable().Select(Result => new ListProgramViewModel()
            {
                Program = new Program() { Id = Result.Program.Id, Title = Result.Program.Title,  FromYear = Result.Program.FromYear, ToYear = Result.Program.ToYear }
                
            });
            
        }

    }
}
