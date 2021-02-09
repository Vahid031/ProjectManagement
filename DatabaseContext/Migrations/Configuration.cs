using DatabaseContext.Context;
using DomainModels.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;


namespace DatabaseContext.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Context.DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Context.DatabaseContext context)
        {

        }
    }
}
