using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Faps.Controllers;
using Faps.Models;
using System.Data.SqlClient;


namespace Faps.Controllers
{
    public class AccountController : Controller
    {



        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        //Chama a view Do registro
        public ActionResult Register()
        {
            return View();
        }

        //Salva o registro do usuario
        [HttpPost]
        public ActionResult Salvar_registro(Curriculo resume)
        {


            try
            {
                Usuarios user = new Usuarios();
                user.role = "user";
                user.Usuario = resume.Usuario.ToString();
                user.Senha = resume.Senha.ToString();

                FAPSEntities db = new FAPSEntities();
                db.Usuarios.Add(user);
                db.Curriculo.Add(resume);
                db.SaveChanges();

                return View("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }



        }



        //Valida se a conta existe e faz login dos usuarios admin e user
        [HttpPost]
        public ActionResult Verify(Usuarios acc)
        {
            try
            {
                FAPSEntities db = new FAPSEntities();
                var user_logado = db.Usuarios.Where(f => f.Usuario == acc.Usuario && f.Senha == acc.Senha)?.FirstOrDefault();

                if (user_logado != null)
                {
                    if (user_logado.role == "admin")
                    {

                        Session["id_admin"] = (int)user_logado.Codigo_user;


                        //#############################Registrando log administrador no DB#############################
                        var Usuario = db.Usuarios.Where(f => f.Codigo_user == (int)user_logado.Codigo_user).FirstOrDefault()?.Usuario;
                        Log log = new Log();
                        log.Codigo_user = (int)user_logado.Codigo_user;
                        log.Log1 = "Adminstrador " + Usuario + " Entrou no sistema (Login)";
                        log.Data = DateTime.Now;
                        db.Log.Add(log);
                        db.SaveChanges();
                        //#################################-log-#######################################################


                        return RedirectToAction("Admin_home", "Admin");

                    }
                    else
                    {

                        Session["id_user"] = (int)user_logado.Codigo_user;

                        int codigo = (int)user_logado.Codigo_user;


                        //#############################Registrando log usuario no DB#############################
                        var Usuario = db.Usuarios.Where(f => f.Codigo_user == (int)user_logado.Codigo_user).FirstOrDefault()?.Usuario;
                        Log log = new Log();
                        log.Codigo_user = (int)user_logado.Codigo_user;
                        log.Log1 = "Usuario " + Usuario + " Entrou no sistema (Login)";
                        log.Data = DateTime.Now;
                        db.Log.Add(log);
                        db.SaveChanges();
                        //#################################-log-#################################################


                        return RedirectToAction("User_home", "User", new { id = codigo });
                    }
                }
                else
                {
                    ViewBag.SenhaInvalida = true;
                    return View("Login");
                }

            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }






            /*SqlConnection con = new SqlConnection();
            SqlCommand com = new SqlCommand();
            SqlDataReader dr;
            con.ConnectionString = "Data Source=GI-PC;Initial Catalog=FAPS;User ID=sa;Password=root";
            con.Open();
            com.Connection = con;
            com.CommandText = "select * from dbo.Usuarios where Usuario='"+acc.Name+ "' and Senha='"+acc.Password+"'";
            dr = com.ExecuteReader();


            if (dr.Read())
            {

                if (dr.GetValue(3).Equals("admin"))
                {
                    
                    Session["id_admin"] = (int)dr.GetValue(0);

                    con.Close();
                    return RedirectToAction("Admin_home", "Admin");
                }
                else if (dr.GetValue(3).Equals("user"))
                {
                    Session["id_user"] = (int)dr.GetValue(0);

                    int codigo = (int)dr.GetValue(0);

                    con.Close();
                    return RedirectToAction("User_home", "User", new { id = codigo });
                }
                else {
                    con.Close();
                    return View("Error");
                }

            }else
            {
               con.Close();
               return View("Login");
            }*/


        }


        //Efetua o logout do usuario
        public ActionResult Logout()
        {
            try
            {
                //reset session
                Session["id_admin"] = null;
                Session["id_user"] = null;

                //implementar tabela Sessao



                return View("Login");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }


        }





    }
}