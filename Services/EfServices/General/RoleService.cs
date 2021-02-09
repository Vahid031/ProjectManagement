using DatabaseContext.Context;
using DomainModels.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Infrastracture.AppCode;
using Services.Interfaces.General;
using ViewModels.General.RoleViewModel;

namespace Services.EfServices.General
{
    public class RoleService : GenericService<Role>, IRoleService
    {
        public RoleService(IUnitOfWork uow)
            : base(uow)
        {

        }

        //--------------------- Tree View Radio ----------------------------------------------------------------------------------
        string tree = "";
        List<Role> ListRoles;
        public string FillTree(string roleCode)
        {
            try
            {
                ListRoles = new List<Role>();
                ListRoles = Get().ToList();
                var role = ListRoles.FirstOrDefault(i => i.Code == roleCode);
                
                tree = "<li><div class='colaps'></div><span class='tree-img'></span><span id='" + role.Id + "' class='tree-text' level='0'>" + role.Title + "</span><span></span><div class='tree-clear'></div><ul>";

                PopulateTreeView(role.Id);

                return tree;
            }
            catch (Exception) { return ""; }
        }

        public void PopulateTreeView(long id)
        {
            try
            {
                if (ListRoles != null)
                {
                    string ul_li = "";

                    IEnumerable<Role> _Role = from p in ListRoles where p.ParentId == id select p;
                    foreach (Role item in _Role)
                    {
                        if (ListRoles.Any(m => m.ParentId == item.Id))
                        {
                            tree += string.Format(@"<li><div class='colaps'></div>
                                                        <span class='tree-img'></span>
                                                        <span class='{3} tree-text' id='{0}' level='{4}'>{1} &nbsp;{2}</span>
                                                        <span class='tree-delete' onmousedown = MvcAlert('DeleteTree','{0}')></span>
                                                        <span class='tree-update' onmousedown = UpdateTree({0})></span>
                                                        <span></span><div class='tree-clear'></div><ul>", item.Id, item.Title, (item.IsActive == true) ? "(فعال)" : "(غیر فعال)", item.IsActive, item.Level);

                            ul_li = "</ul></li>";
                        }
                        else
                        {
                            tree += string.Format(@"<li><span class='tree-img'></span>
                                                        <span class='{3} tree-text' id='{0}' level='{4}'>{1} &nbsp;{2}</span>
                                                        <span class='tree-delete' onmousedown = MvcAlert('DeleteTree','{0}')></span>
                                                        <span class='tree-update' onmousedown = UpdateTree({0})></span>
                                                        <span></span><div class='tree-clear'></div></li>", item.Id, item.Title, (item.IsActive == true) ? "(فعال)" : "(غیر فعال)", item.IsActive, item.Level);
                            ul_li = "";
                        }

                        PopulateTreeView(item.Id);
                        tree += ul_li;
                    }
                }
            }
            catch (Exception) { }
        }

        public string FillTreeRadio()
        {
            try
            {
                ListRoles = new List<Role>();

                ListRoles = Get().ToList();
                Int64 ParentId = Get().FirstOrDefault().Id;
                tree = @"<div class='treeview' style='min-height: 500px;'>
                                        <ul class='first-ul' id='RoleTree2'  style='min-width: 100px;'>
                                            <li><div class='colaps'></div><span class='tree-img'></span><span id='" + ParentId + "' class='tree-text' level='0'>(ریشه) &nbsp;&nbsp; (فعال)</span><span></span><div class='tree-clear'></div><ul>";

                PopulateTreeViewRadio(ParentId);

                return tree + "</ul></div>";
            }
            catch (Exception) { return ""; }
        }
        public void PopulateTreeViewRadio(long id)
        {
            try
            {
                if (ListRoles != null)
                {
                    string ul_li = "";

                    IEnumerable<Role> _Role = from p in ListRoles where p.ParentId == id select p;
                    foreach (Role item in _Role)
                    {
                        if (ListRoles.Any(m => m.ParentId == item.Id))
                        {
                            tree += string.Format(@"<li><div class='colaps'></div>
                                                        <span class='tree-img'></span>
                                                        <span class='{3} tree-text' id='{0}' level='{4}'>{1}</span>
                                                        <span></span><div class='tree-clear'></div><ul>", item.Id, item.Title, (item.IsActive == true) ? "(فعال)" : "(غیر فعال)", item.IsActive, item.Level);

                            ul_li = "</ul></li>";
                        }
                        else
                        {
                            tree += string.Format(@"<li><span class='tree-img'></span>
                                                        <span class='{3} tree-text' id='{0}' level='{4}'>{1}</span>
                                                        <input type='radio' name='RoleTree2' value='{0}' />
                                                        <span></span><div class='tree-clear'></div></li>", item.Id, item.Title, (item.IsActive == true) ? "(فعال)" : "(غیر فعال)", item.IsActive, item.Level);
                            ul_li = "";
                        }

                        PopulateTreeViewRadio(item.Id);
                        tree += ul_li;
                    }
                }
            }
            catch (Exception) { }
        }
        public Int64 SaveRolePermission(CreateRoleViewModel createRole, Int64 MemberId)
        {
            RolePermissionService rolePermissionService = new RolePermissionService(_uow);

            var Permissions = rolePermissionService.Get(i => i.RoleId == createRole.Role.Id);

            foreach (var item in Permissions)
            {
                rolePermissionService.Delete(item);
            }

            if (createRole.PermissionIds != null)
            {
                foreach (var item in createRole.PermissionIds.Split(','))
                {
                    if (item != "")
                    {
                        RolePermission rolePermission = new RolePermission();
                        rolePermission.RoleId = createRole.Role.Id;
                        rolePermission.PermissionId = Convert.ToInt64(item);

                        rolePermissionService.Insert(rolePermission);
                    }
                }
            }

            return _uow.SaveChanges(MemberId);
        }

        public Int64 DeleteRolePermission(Int64 RoleId, Int64 MemberId)
        {
            RolePermissionService rolePermissionService = new RolePermissionService(_uow);
            Delete(RoleId);

            var Permissions = rolePermissionService.Get(i => i.RoleId == RoleId);

            foreach (var item in Permissions)
            {
                rolePermissionService.Delete(item);
            }
            Delete(RoleId);

            return _uow.SaveChanges(MemberId);
        }

    }
}
