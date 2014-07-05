using System;
using System.Data.SQLite;
using System.IO;
using System.Web;
using DapperExtensions;
using DapperExtensions.Sql;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace BuildNumberService
{
    public class Bootstrap : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            //base.ApplicationStartup(container, pipelines);

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);          

            container.Register(new ConnectionFactory(context));
        }
    }

    public class ConnectionFactory : IDisposable
    {
        private readonly NancyContext context;
        private SQLiteConnection connection;

        public ConnectionFactory(NancyContext context)
        {
            this.context = context;
        }

        public SQLiteConnection GetConnection()
        {
            if (connection == null)
            {
                var connectionString = new SQLiteConnectionStringBuilder
                {
                    DataSource = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data\data.db"), 
                    FailIfMissing = false
                };

                connection = new SQLiteConnection(connectionString.ConnectionString);

                connection.Open();
            }

            return connection;
        }

        public void Dispose()
        {
            if (this.connection != null)
            {
                this.connection.Dispose();
            }
        }
    }
}