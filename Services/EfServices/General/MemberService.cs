using DatabaseContext.Context;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using DomainModels.Entities.General;
using Services.Interfaces.General;
using ViewModels.General.MemberViewModel;
using DomainModels.Entities.BaseInformation;

namespace Services.EfServices.General
{
    public class MemberService : GenericService<Member>, IMemberService
    {
        public MemberService(IUnitOfWork uow)
            : base(uow)
        {

        }

        public IEnumerable<ListMemberViewModel> GetAll(ListMemberViewModel memberViewModel, ref Paging pg, string roleCode)
        {
            DynamicFiltering.GetFilter<ListMemberViewModel>(memberViewModel, ref pg);

            var a =
            _uow.Set<Member>()
                .Join(_uow.Set<Role>(), Member => Member.RoleId, Role => Role.Id, (Member, Role) => new { Member, Role })
                .Join(_uow.Set<MemberGroup>(), MR => MR.Member.MemberGroupId, MemberGroup => MemberGroup.Id, (MR, MemberGroup) => new { MR.Member, MR.Role, MemberGroup});

            if (pg._filter != "")
                a = a.Where(pg._filter, pg._values.ToArray());

            a = a.Where(i => i.Role.Code.StartsWith(roleCode));

            pg._rowCount = a.Count();

            a = a.OrderBy(pg._orderColumn).Skip((pg._pageNumber - 1) * pg._pageSize).Take(pg._pageSize);


            return a.AsEnumerable().Select(Result => new ListMemberViewModel()
            {
                Member = new Member() { Id = Result.Member.Id, FirstName = Result.Member.FirstName, LastName = Result.Member.LastName, NationalCode = Result.Member.NationalCode, UserName = Result.Member.UserName, Password = Result.Member.Password, Email = Result.Member.Email, Address = Result.Member.Address, IsActive = Result.Member.IsActive},
                Role = new Role() { Id = Result.Role.Id, Title = Result.Role.Title, IsActive = Result.Role.IsActive },
                MemberGroup = new MemberGroup() { Id = Result.MemberGroup.Id, Title = Result.MemberGroup.Title }
            });
        }

        public Member GetMemberById(Int64 Id)
        {
            return Get(i => i.Id == Id).AsEnumerable().Select(i => new Member() { Id = i.Id, FirstName = i.FirstName, FatherName = i.FatherName, LastName = i.LastName, UserName = i.UserName, Password = i.Password, Email = i.Email, Address = i.Address, BirthdayDate= i.BirthdayDate, Description = i.Description, Gender = i.Gender, Job = i.Job, MemberGroupId = i.MemberGroupId, NationalCode = i.NationalCode, RoleId = i.RoleId, IsActive = i.IsActive }).FirstOrDefault();
        }

        public Member AuthenticateUser(string UserName, string Password)
        {
            return Get(i => i.UserName == UserName && i.Password == Password).FirstOrDefault();
        }

        public bool IsValid(string Url, Int64 UserId, Int64 RoleId)
        {
            var query = _uow.Set<RolePermission>()
                .Join(_uow.Set<Permission>(), rp => rp.PermissionId, p => p.Id, (rp, p) => new { rp.RoleId, p.Url })
                .Any(i => i.Url.Contains(Url) && i.RoleId == RoleId);

            return query;
        }
    }
}
