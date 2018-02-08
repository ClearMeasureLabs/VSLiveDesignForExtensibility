using System;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(ClearMeasure.Bootcamp.UI.App_Start.SessionAuthenticationConfig), "PreAppStart")]

namespace ClearMeasure.Bootcamp.UI.App_Start
{
    public static class SessionAuthenticationConfig
    {
        public static void PreAppStart()
        {
            DynamicModuleUtility.RegisterModule(typeof(System.IdentityModel.Services.SessionAuthenticationModule));
        }
    }
}