﻿using DatabaseContext.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.ProjectDevision;
using Services.Interfaces.General;
using Services.Interfaces.ProjectDevision;
using ViewModels.ProjectDevision.ProjectSectionOpenDateViewModel;

namespace Services.EfServices.ProjectDevision
{
    public class ProjectSectionOpenDateService : GenericService<ProjectSectionOpenDate>, IProjectSectionOpenDateService
    {
        public ProjectSectionOpenDateService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListProjectSectionOpenDateViewModel> GetAll(ListProjectSectionOpenDateViewModel latvm, ref Paging pg)
        {
            pg._orderColumn = pg._orderColumn.Replace("ProjectSectionOpenDate.", "");
            DynamicFiltering.GetFilter<ListProjectSectionOpenDateViewModel>(latvm, ref pg);
            pg._filter = pg._filter.Replace("ProjectSectionOpenDate.", "");

            var a = _uow.Set<ProjectSectionOpenDate>().AsQueryable<ProjectSectionOpenDate>();

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListProjectSectionOpenDateViewModel()
            {
                ProjectSectionOpenDate = new ProjectSectionOpenDate() { Id = Result.Id, ProjectSectionId = Result.ProjectSectionId, OpenDate = Result.OpenDate, MessageContent = Result.MessageContent, IsSend = Result.IsSend }
            });
        }

    }
}