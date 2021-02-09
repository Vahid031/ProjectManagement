using System.Web.Mvc;

namespace Web.Areas.ProjectDefine
{
    public class ProjectDefineAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ProjectDefine";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ProjectDefine_default",
                "ProjectDefine/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}