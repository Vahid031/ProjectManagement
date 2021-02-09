using System.Web.Mvc;

namespace Web.Areas.ProjectDevision
{
    public class ProjectDevisionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ProjectDevision";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ProjectDevision_default",
                "ProjectDevision/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}