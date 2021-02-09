using Services.Interfaces.General;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ViewModels;
using ViewModels.Other;

namespace Web.Utilities.CustomFilters
{
    public class AuthenticationActionFilter : AuthorizeAttribute
    {
        readonly IMemberService _MemberService;

        public AuthenticationActionFilter()
        {
            _MemberService = DependencyResolver.StructureMapObjectFactory.Container.GetInstance<IMemberService>();
        }


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            AdminViewModel Member = (AdminViewModel)httpContext.Session["Member"];
            if (Member == null)
                return false;

            var rd = httpContext.Request.RequestContext.RouteData;

            string Url = "/" + rd.DataTokens["area"].ToString() + "/" + rd.Values["Controller"] + "/" + rd.Values["Action"];

            if (httpContext.Request.Params[0] != null && httpContext.Request.Params["_"] == null && rd.Values["Action"].ToString() == "_Create")
            {
                if (httpContext.Request.Params[0].ToString() != "-1")
                    Url = Url.Replace("_Create", "_Update");
            }

            return _MemberService.IsValid(Url, Member.Id, Member.RoleId);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            string action = filterContext.RouteData.Values["Action"].ToString();
            string c = filterContext.RouteData.Values["Controller"].ToString();

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                base.HandleUnauthorizedRequest(filterContext);
            }
            else if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.HttpContext.Response.StatusCode = 401;
                
                if (action == "_Index" || action == "_Default")
                {
                    filterContext.HttpContext.Response.StatusCode = 403;
                }
                filterContext.HttpContext.Response.End();

                base.HandleUnauthorizedRequest(filterContext);
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Index", area = "General" }));
            }
        }
    }
}