using Faps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faps.Controllers
{
    public class UserController : Controller
    {
        //Valida se o usuario esta "logado" e retorna seu id
        public int? User_id()
        {
            //validação usuario logado
            //Copular Log do Sistema
            int user_id;

            if (Session["id_user"] != null)
            {
                user_id = (int)Session["id_user"];
                return user_id;
            }
            else
            {
                //se retornar null manda pra tela de erro
                return null;
            }
        }




        // GET: User
        public ActionResult User_home()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                //Responsavel por colocar o nome do usuario nas views User
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                ViewBag.nome = nome;
                ViewBag.id_applyer = id_usuario;

                //pega o status da candidatura do usuario
                var Applyed_Status = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Status_candidatura;


                //Validando se o usuario quer colocar uma imagem no seu curriculo
                var img_on_cv = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.imageUrl;


                //Copula a tela home com as vagas disponiveis
                var getVagasLista = db.Vagas.ToList();



                //verifica se o usuario tem algum curriculo cadastrado
                if (db.Curriculo.Where(f => f.codigo_user == id_usuario).Any())
                {
                    //veririca se o usuario esta candidatado em alguma vaga---------------------------------------------------
                    if (Applyed_Status == 1)
                    {

                        //Candidatura realizada
                        return RedirectToAction("User_home_1", "User");

                    }
                    else if (Applyed_Status == 2)
                    {

                        //Curriculo em Analise pela equipe
                        return RedirectToAction("User_home_2", "User");

                    }
                    else if (Applyed_Status == 3)
                    {
                        //Entrevista
                        return RedirectToAction("User_home_3", "User");
                    }
                    else
                    {
                        if (img_on_cv != null)
                        {
                            //Status vaga = 0 SEM CANDIDATURA A NENHUMA VAGA
                            return View(getVagasLista);
                        }
                        else
                        {
                            //Mostra msg de colocar imagem no curriculo na view
                            ViewBag.Img_on_Cv = false;

                            //Status vaga = 0 SEM CANDIDATURA A NENHUMA VAGA
                            return View(getVagasLista);
                        }
                    }

                }
                else
                {
                    return RedirectToAction("Cadastro_curriculo", "User");

                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }


        }



        //view Candidatura realizada STATUS CANDIDATURA = 1
        public ActionResult User_home_1()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                //Responsavel por colocar o nome do usuario nas views User
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                ViewBag.nome = nome;

                //Responsavel por colocar o nome do usuario nas views User
                var candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Vagas.Vaga;
                ViewBag.vaga = candidatura;


                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //view CANDIDATURA EM ANALISE == STATUS CANDIDATURA = 2
        public ActionResult User_home_2()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                //Responsavel por colocar o nome do usuario nas views User
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                ViewBag.nome = nome;

                //Responsavel por colocar o nome do usuario nas views User
                var candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Vagas.Vaga;
                ViewBag.vaga = candidatura;


                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //view CANDIDATURA EM ANALISE == STATUS CANDIDATURA = 2
        public ActionResult User_home_3()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                //Responsavel por colocar o nome do usuario nas views User
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                ViewBag.nome = nome;

                //Responsavel por colocar o nome do usuario nas views User
                var candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Vagas.Vaga;
                ViewBag.vaga = candidatura;

                var DataEntrevista = db.Interview.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Data_Entrevista;
                ViewBag.dataEntrevista = DataEntrevista;


                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }




        //Aplica para a vaga, recebe o id da vaga e o id do usuario que esta aplicando para a vaga
        [HttpGet]
        public ActionResult Apply(int id_vaga, int id_applyer)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }



                FAPSEntities db = new FAPSEntities();


                Candidaturas cd = new Candidaturas();

                cd.Codigo_user = id_applyer;
                //STATUS DA CANDIDATURA == 1 OU SEJA "CANDIDATOU SE COM SUCESSO PARA A VAGA"
                cd.Status_candidatura = 1;
                cd.Codigo_Vaga = id_vaga;

                db.Candidaturas.Add(cd);
                db.SaveChanges();


                //#############################Registrando log no DB###########################################
                var nome_vaga = db.Vagas.Where(f => f.Codigo_vaga == id_vaga).FirstOrDefault()?.Vaga;


                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                Log log = new Log();
                log.Codigo_user = (int)id_usuario;
                log.Log1 = "Usuario " + nome + " Aplicou para a vaga " + nome_vaga;
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                //retorna a para a home e carrega ela com o id do usuario
                return RedirectToAction("User_home", "User", new { id = id_applyer });
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }






        //Chama a view de cadastro do curriculo SOMENTE PARA USUARIOS SEM CURRICULO CADASTRADO - SOMENTE USUARIO CADASTRADO PELO ADMIN
        public ActionResult Cadastro_curriculo()
        {
            //Valida se a sessão do usuario ainda existe e se ele esta logado
            int? id_usuario;
            if (User_id() == null)
            {
                return View("Error");
            }
            else
            {
                id_usuario = User_id();
            }


            return View();
        }




        //Salva o cadastro do usuario
        [HttpPost]
        public ActionResult Salvar_registro(Curriculo resume)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                resume.codigo_user = (int)id_usuario;
                resume.Usuario = db.Usuarios.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Usuario;
                resume.Senha = db.Usuarios.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Senha;

                db.Curriculo.Add(resume);
                db.SaveChanges();


                //#############################Registrando log no DB###########################################
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                Log log = new Log();
                log.Codigo_user = (int)id_usuario;
                log.Log1 = "Usuario " + nome + " Cadastrou seu curriculo";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                return RedirectToAction("User_home", "User");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }




        //Permite o usuario listar seu curriculo
        public ActionResult Listar_curriculo()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }



                FAPSEntities db = new FAPSEntities();
                //Responsavel por colocar o nome do usuario nas views User
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                ViewBag.nome = nome;



                //Consulta no db o curriculo do candidato
                var getCurriculo = db.Curriculo.Where(f => f.codigo_user == id_usuario);


                //#############################Registrando log no DB###########################################
                Log log = new Log();
                log.Codigo_user = (int)id_usuario;
                log.Log1 = "Usuario " + nome + " Listou os curriculos";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                return View("Listar_curriculo_user", getCurriculo);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }


        //Chama a partial view que carregada com o curriculo que sera editado
        [HttpGet]
        public ActionResult Editar_curriculo(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                Curriculo c = db.Curriculo.Where(f => f.codigo_curriculo == id).FirstOrDefault();

                //#############################Registrando log no DB###########################################
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                Log log = new Log();
                log.Codigo_user = (int)id_usuario;
                log.Log1 = "Usuario " + nome + " Editou seu curriculo";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                return PartialView("_Editar_curriculo", c);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }


        //Recebe o curriculo editado da partial acima view e salva ele
        [HttpPost]
        public ActionResult Salvar_curriculo(Curriculo c)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                //Procura a vaga a ser salva a altera item por item conforme oque veio da view
                var to_update = db.Curriculo.Where(f => f.codigo_curriculo == c.codigo_curriculo).FirstOrDefault();
                to_update.codigo_curriculo = c.codigo_curriculo;
                to_update.codigo_user = c.codigo_user;

                to_update.Nome = c.Nome;
                to_update.SobreNome = c.SobreNome;
                to_update.Email = c.Email;
                to_update.Telefone = c.Telefone;
                to_update.Genero = c.Genero;
                to_update.DataNascimento = c.DataNascimento;
                to_update.Endereco = c.Endereco;
                to_update.Cidade = c.Cidade;
                to_update.Estado = c.Estado;
                to_update.CEP = c.CEP;
                to_update.Pais = c.Pais;
                to_update.Curso = c.Curso;
                to_update.Curso_status = c.Curso_status;
                to_update.TituloCargo = c.TituloCargo;
                to_update.Empresa = c.Empresa;
                to_update.Data_inicio = c.Data_inicio;
                to_update.DataTermino = c.DataTermino;
                to_update.DescricaoAtividades = c.DescricaoAtividades;


                TryUpdateModel(to_update);
                db.SaveChanges();

                return RedirectToAction("Listar_curriculo", "User");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }


        //Adicionar foto no curriculo
        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }




                if (file != null)
                {
                    FAPSEntities db = new FAPSEntities();
                    string ImageName = System.IO.Path.GetFileName(file.FileName);
                    string physicalPath = Server.MapPath("~/imagesDB/" + ImageName);

                    // save image in folder
                    file.SaveAs(physicalPath);

                    //save new record in database
                    Curriculo to_add_image = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault();
                    to_add_image.imageUrl = ImageName;

                    TryUpdateModel(to_add_image);
                    db.SaveChanges();

                    //#############################Registrando log no DB###########################################
                    var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                    Log log = new Log();
                    log.Codigo_user = (int)id_usuario;
                    log.Log1 = "Usuario " + nome + " Adicionou foto ao seu curriculo";
                    log.Data = DateTime.Now;
                    db.Log.Add(log);
                    db.SaveChanges();
                    //#################################-log-#######################################################

                }


                return RedirectToAction("Listar_curriculo", "User");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }




        public ActionResult Listar_Feedback()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }



                FAPSEntities db = new FAPSEntities();
                //Responsavel por colocar o nome do usuario nas views User
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                ViewBag.nome = nome;



                //Consulta no db o feedback do candidato
                var codigo_entrevista = db.Interview.Where(f => f.Codigo_user == id_usuario).FirstOrDefault()?.Codigo_entrevista;

                var getFeedbackList = db.Feedback.Where(f => f.Codigo_entrevista == codigo_entrevista).ToList();


                //#############################Registrando log no DB###########################################
                Log log = new Log();
                log.Codigo_user = (int)id_usuario;
                log.Log1 = "Usuario " + nome + " Listou os feedbacks recebidos";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                return View(getFeedbackList);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }





        public ActionResult Logout()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? id_usuario;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    id_usuario = User_id();
                }


                FAPSEntities db = new FAPSEntities();
                //Responsavel por colocar o nome do usuario nas views User
                var nome = db.Curriculo.Where(f => f.codigo_user == id_usuario).FirstOrDefault()?.Nome;
                ViewBag.nome = nome;




                //#############################Registrando log no DB###########################################
                Log log = new Log();
                log.Codigo_user = (int)id_usuario;
                log.Log1 = "Usuario " + nome + " Saiu do sistema (Logout)";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                Session["id_user"] = null;

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }


    }
}