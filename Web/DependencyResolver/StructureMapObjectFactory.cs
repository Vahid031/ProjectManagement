using DatabaseContext.Context;
using Services.EfServices;
using Services.Interfaces;
using StructureMap;
using StructureMap.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;


namespace Web.DependencyResolver
{
    public class StructureMapObjectFactory
    {
        private static readonly Lazy<Container> _containerBuilder = new Lazy<Container>(defaultContainer, LazyThreadSafetyMode.ExecutionAndPublication);

        public static IContainer Container
        {
            get { return _containerBuilder.Value; }
        }

        private static Container defaultContainer()
        {
            return new Container(x =>
            {
                x.For<IUnitOfWork>().HybridHttpOrThreadLocalScoped().Use<DatabaseContext.Context.DatabaseContext>();

                x.For(typeof(IGenericService<>)).Use(typeof(GenericService<>));

                x.Scan(scan =>
                    {
                        scan.AssemblyContainingType<Services.Interfaces.General.IRoleService>();
                        scan.WithDefaultConventions();
                    });
            });
        }
    }
}