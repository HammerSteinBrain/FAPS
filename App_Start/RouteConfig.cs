using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Faps
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home_VagasController", action = "Home_Vagas", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Apply",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User",action = "Apply", id_vaga = UrlParameter.Optional, id_applyer = UrlParameter.Optional }
            );
            //Configuração de rota para permitir receber dois parametros no get

        }
    }
}
