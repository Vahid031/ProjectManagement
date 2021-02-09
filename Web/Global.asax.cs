using DatabaseContext.Context;
using Services.Interfaces.BaseInformation;
using Services.Interfaces.General;
using StructureMap.Web.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ViewModels.Other;
using Web.DependencyResolver;
using Web.Schedule;
using Web.Utilities.AppCode;

namespace Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static class App
        {
            public static string SiteRootUrl;
        }


        #region Application_Start

        protected void Application_Start()
        {
            // Area
            AreaRegistration.RegisterAllAreas();

            // MVC
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            // Remove Extra Engines
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            //StructureMap
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());

            // -------- schedule --------
            //ScheduledTasksRegistry.Init();
        }

        #endregion


        #region Application_End

        protected void Application_End()
        {
            // -------- schedule --------
            ScheduledTasksRegistry.End();
            //نکته مهم این روش نیاز به سرویس پینگ سایت برای زنده نگه داشتن آن است
            ScheduledTasksRegistry.WakeUp(App.SiteRootUrl);
        }

        #endregion


        #region Application_EndRequest

        protected virtual void Application_EndRequest()
        {
            HttpContextLifecycle.DisposeAndClearAll();
        }

        #endregion


        #region Application_BeginRequest

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // -------- schedule --------
            if (string.IsNullOrWhiteSpace(App.SiteRootUrl))
            {
                App.SiteRootUrl = Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath;
            }
        }

        #endregion


        protected void Session_End(object sender, EventArgs e)
        {
            try
            {
                AdminViewModel Member = (AdminViewModel)Session["Member"];
                IFileService _fileService = DependencyResolver.StructureMapObjectFactory.Container.GetInstance<IFileService>();
                _fileService.DeleteFiles(Member.Id);
            }
            catch { }

        }
    }
}
