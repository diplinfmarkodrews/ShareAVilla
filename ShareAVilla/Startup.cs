﻿using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ShareAVilla.Startup))]
namespace ShareAVilla
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //app.MapSignalR();
        }
    }
}
