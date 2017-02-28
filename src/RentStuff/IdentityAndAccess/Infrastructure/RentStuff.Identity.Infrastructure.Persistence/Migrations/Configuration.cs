using System.Data.Entity.Migrations;

namespace RentStuff.Identity.Infrastructure.Persistence.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<AuthContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            // register mysql code generator
            SetSqlGenerator("MySql.Data.MySqlClient", new MySql.Data.Entity.MySqlMigrationSqlGenerator());

            SetHistoryContextFactory("MySql.Data.MySqlClient", (conn, schema) => new MySqlHistoryContext(conn, schema));
        }

        protected override void Seed(AuthContext context)
        {

        }
    }
}
