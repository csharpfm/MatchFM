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
            GlobalConfiguration.Configuration
                .UseSqlServerStorage("DefaultConnection")
                .UseMsmqQueues(@".\private$\matchfm");
            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}