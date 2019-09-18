using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.Migrations;

namespace Recargas.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Recargas.DataAccess.RecargasContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Recargas.RecargasContext";
        }

        protected override void Seed(Recargas.DataAccess.RecargasContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}