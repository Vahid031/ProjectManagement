using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.DependencyResolver
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        protected override IController GetControllerInstance(System.Web.Routing.RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                //throw new Exception(string.Format("Page Not Found: {0}", requestContext.HttpContext.Request.RawUrl)); //WebConfig CustomeError Mode=On
                return null;
            }

            return StructureMapObjectFactory.Container.GetInstance(controllerType) as Controller;
        }
    }
}