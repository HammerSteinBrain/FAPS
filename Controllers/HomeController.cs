using Faps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faps.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home_Vagas
        public ActionResult Home_Vagas()
        {
            try
            {
                FAPSEntities vagas_Entity = new FAPSEntities();
                var getVagasLista = vagas_Entity.Vagas.ToList();

                return View(getVagasLista);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }
    }
}