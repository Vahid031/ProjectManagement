using DatabaseContext.Context;
using DomainModels.Entities;
using DomainModels.Entities.General;
using Services.Interfaces;
using Services.Interfaces.General;
using System;
using System.Linq;
using System.Collections.Generic;
using ViewModels.General.PermissionViewModel;
using ViewModels.General.RolePermissionViewModel;

namespace Services.EfServices.General
{
    
    public class PermissionService : GenericService<Permission>, IPermissionService
    {
        public PermissionService(IUnitOfWork uow)
            : base(uow)
        {
            
        }


        string tree = "";
        List<Permission> ListPermissions;


        public string FillTreeChecked(Int64 RoleId)
        {
            try
            {
                var a =
                    _uow.Set<Role>()
                        .Join(_uow.Set<RolePermission>(), R => R.Id, RP => RP.RoleId, (Role, RolePermission) => new { Role, RolePermission })
                        .Join(_uow.Set<Permission>(), R => R.RolePermission.PermissionId, P => P.Id, (Role, Permission) => new { Role.Role, Role.RolePermission, Permission});

                a = a.Where(i => i.Role.Id == RoleId);

                ListPermissions = new List<Permission>();
                ListPermissions = a.Select(i => i.Permission).ToList();

                var role = ListPermissions.FirstOrDefault();

                tree = @"<div class='treeview' style='min-height: 500px;'>
                                        <ul class='first-ul' id='PermissionTree2'  style='min-width: 100px;'>
                                            <li><div class='colaps'></div><span class='tree-img'></span><span id='" + role.Id + "' class='tree-text' level='0'>" + role.Title + "</span><span></span><div class='tree-clear'></div><ul>";

                PopulateTreeViewChecked(role.Id);

                return tree + "</ul></div>";
            }
            catch (Exception) { return ""; }
        }
        public void PopulateTreeViewChecked(long id)
        {
            try
            {
                if (ListPermissions != null)
                {
                    string ul_li = "";

                    IEnumerable<Permission> _Permission = from p in ListPermissions where p.ParentId == id select p;
                    foreach (Permission item in _Permission)
                    {
                        if (ListPermissions.Any(m => m.ParentId == item.Id))
                        {
                            tree += string.Format(@"<li><div class='colaps'></div>
                                                        <span class='tree-img'></span>
                                                        <span class='{3} tree-text' id='{0}' level='{4}'>{1}</span>
                                                        <input type='Checkbox' name='PermissionTree2' value='{0}' />
                                                        <span></span><div class='tree-clear'></div><ul>", item.Id, item.Title, (item.IsActive == true) ? "(فعال)" : "(غیر فعال)", item.IsActive, item.Level);

                            ul_li = "</ul></li>";
                        }
                        else
                        {
                            tree += string.Format(@"<li><span class='tree-img'></span>
                                                        <span class='{3} tree-text' id='{0}' level='{4}'>{1}</span>
                                                        <input type='Checkbox' name='PermissionTree2' value='{0}' />
                                                        <span></span><div class='tree-clear'></div></li>", item.Id, item.Title, (item.IsActive == true) ? "(فعال)" : "(غیر فعال)", item.IsActive, item.Level);
                            ul_li = "";
                        }

                        PopulateTreeViewChecked(item.Id);
                        tree += ul_li;
                    }
                }
            }
            catch (Exception) { }
        }

        public string FillTree(Int64 ParentId, Int64 RoleId)
        {
            try
            {
                var a =
                    _uow.Set<Permission>()
                        .Join(_uow.Set<RolePermission>(), P => P.Id, RP => RP.PermissionId, (Permission, RolePermission) => new { Permission, RolePermission });

                a = a.Where(i => i.RolePermission.RoleId == RoleId && i.Permission.Level < 4);

                ListPermissions = new List<Permission>();
                ListPermissions = a.AsEnumerable().Select(Result => Result.Permission).ToList();

                PopulateTreeView(ParentId);

                return tree;
            }
            catch (Exception) { return ""; }
        }

        public void PopulateTreeView(long id)
        {
            try
            {
                if (ListPermissions != null)
                {
                    string ul_li = "";

                    IEnumerable<Permission> _Permission = from p in ListPermissions where p.ParentId == id orderby p.OrderId select p;
                    foreach (Permission item in _Permission)
                    {
                        if (ListPermissions.Any(m => m.ParentId == item.Id))
                        {
                            tree += string.Format(@"<li data-toggle='collapse' data-target='#hm{0}' class='collapsed active'>
                                                        <a><i class='{1}'></i> <span class='menu-span'>{2}</span><span class='arrow pull-left'></span></a>
                                                        <ul class='sub-menu collapse' id='hm{0}'>", item.Id, item.Icon, item.Title);
                            ul_li = "</ul></li>";
                        }
                        else
                        {
                            tree += string.Format(@"<li>
                                                        <a href='{0}'><i class='{1}'></i><span>{2}</span> </a>
                                                    </li>", item.Url, item.Icon, item.Title);
                            ul_li = "";
                        }

                        PopulateTreeView(item.Id);
                        tree += ul_li;
                    }
                }
            }
            catch (Exception) { }
        }


        public string GetPagePermissions(string Url, Int64 RoleId)
        {
            var a =
                _uow.Set<Permission>()
                    .Join(_uow.Set<RolePermission>(), P => P.Id, RP => RP.PermissionId, (Permission, RolePermission) => new { Permission, RolePermission });


            var pagePermission = Get(i => i.Url.Contains(Url)).FirstOrDefault();
            
            if (pagePermission == null)
            {
                return "";
            }
                
            var childPermission = a.Where(i => i.Permission.ParentId == pagePermission.Id && i.RolePermission.RoleId == RoleId).Select(i => i.Permission).ToList();

            string Permissions = "";

            foreach (var item in childPermission)
            {
                Permissions += item.Url + ",";
            }

            return Permissions;
        }
    }
}