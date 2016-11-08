using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Hangfire;
using Hangfire.Azure.ServiceBusQueue;
using Hangfire.SqlServer;
using Microsoft.WindowsAzure;
using Owin;

namespace MatchFM
{
    public partial class Startup
    {
        public void ConfigureHangfire(IAppBuilder app)
        {
            var sqlStorage = new SqlServerStorage("DefaultConnection");
            var connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
            sqlStorage.UseServiceBusQueues(connectionString);

            GlobalConfiguration.Configuration.UseStorage(sqlStorage);
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}