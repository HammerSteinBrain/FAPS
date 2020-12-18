using Faps.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Faps.Controllers
{
    public class AdminController : Controller
    {


        //Valida se o usuario esta "logado" e retorna seu id
        public int? User_id()
        {
            //validação usuario logado
            //Copular Log do Sistema
            int user_id;

            if (Session["id_admin"] != null)
            {
                user_id = (int)Session["id_admin"];
                return user_id;
            }
            else
            {
                //se retornar null manda pra tela de erro
                return null;
            }
        }



        // Home admin
        public ActionResult Admin_home()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }

                FAPSEntities db = new FAPSEntities();
                var getVagasLista = db.Vagas.ToList();

                return View(getVagasLista);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //Carrega a view do cadastrar vagas---------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Cadastrar_vaga()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //BackEnd do cadastrar vagas (salvar)
        [HttpPost]
        public ActionResult Confirmar_vaga(Vagas vagas)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                //salvando a vaga nova no db
                FAPSEntities db = new FAPSEntities();
                db.Vagas.Add(vagas);
                db.SaveChanges();


                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + " cadastrou a vaga " + vagas.Vaga;
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                return RedirectToAction("Admin_home", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //Recebe da view admin o id da vaga que precisa alterar : UPDATE VAGA
        [HttpGet]
        public ActionResult Listar_vaga_to_update(int id_vaga)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                var vaga_to_update = db.Vagas.Where(f => f.Codigo_vaga == id_vaga).FirstOrDefault();


                return View("Alterar_vaga", vaga_to_update);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //Recebe a vaga editada da view e salva ela
        [HttpPost]
        public ActionResult Alterar_vaga(Vagas vaga_to_update)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                //Procura a vaga a ser salva a altera item por item conforme oque veio da view
                var to_update = db.Vagas.Where(f => f.Codigo_vaga == vaga_to_update.Codigo_vaga).FirstOrDefault();
                to_update.Codigo_vaga = vaga_to_update.Codigo_vaga;
                to_update.Vaga = vaga_to_update.Vaga;
                to_update.Vaga_descricao = vaga_to_update.Vaga_descricao;

                TryUpdateModel(to_update);
                db.SaveChanges();

                return RedirectToAction("Admin_home", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //Deletar vagas, espera o id da vaga ou codigo_vaga
        [HttpGet]
        public ActionResult Deletar_vaga(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities vagas_Entity = new FAPSEntities();

                Vagas v = vagas_Entity.Vagas.Find(id);
                vagas_Entity.Vagas.Remove(v);
                vagas_Entity.SaveChanges();


                return RedirectToAction("Admin_home", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //Chama a view ver candidaturas da vaga e espera receber o codigo da vaga "id"---------------------------------------------------------------------------------------------------------
        [HttpGet]
        public ActionResult Ver_candidaturas(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                var getCandidaturasLista = db.Candidaturas.Where(f => f.Codigo_Vaga == id && f.Status_candidatura < 3).ToList();


                //Cogido_vaga da tabela vagas é com o "v" minusculo
                string vaga = db.Vagas.Where(f => f.Codigo_vaga == id).FirstOrDefault()?.Vaga;
                ViewBag.NomeVaga = vaga;



                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + " Listou as candidaturas da vaga de "+vaga;
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################


                return View(getCandidaturasLista);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }


        }


        //Interrompe ou recusa o processo seletivo do candidato / deleta candidatura            ESPERA UM ID COM O NOME ID_CANDIDATURA
        [HttpGet]
        public ActionResult Deletar_candidatura(int id_candidatura)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                //pego o user antes de rodar o delete
                var c_user = db.Candidaturas.Where(linha => linha.Codigo_Candidatura == id_candidatura).FirstOrDefault().Codigo_user;

                //pego o codigo da vaga antes de rodar o delete
                var codigo_vaga = db.Candidaturas.Where(linha => linha.Codigo_user == c_user).FirstOrDefault().Codigo_Vaga;


                //valida se a candidatura tem alguma entrevista relacionada, se tiver ela precisa ser deletada
                var id_interview = db.Interview.Where(l => l.Codigo_user == c_user).FirstOrDefault()?.Codigo_entrevista;
                if (id_interview != null)
                {
                    //agora podemos deletar a entrevista
                    Interview i = db.Interview.Find(id_interview);
                    db.Interview.Remove(i);
                }


                Candidaturas c = db.Candidaturas.Find(id_candidatura);
                db.Candidaturas.Remove(c);
                db.SaveChanges();


                return RedirectToAction("Admin_home", "Admin");

            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }


        }





        //Carrega a view Analisar_curriculo/ Backend da view Analisar_curriculo e espera o id do candidato---------------------------ESPERA UM ID COM O NOME ID_CANDIDATO------------------------------------------------------
        [HttpGet]
        public ActionResult Analisar_curriculo(int id_candidato)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                //Consulta no db o curriculo do candidato
                var getCurriculo = db.Curriculo.Where(f => f.codigo_user == id_candidato);

                ViewBag.nome = getCurriculo.FirstOrDefault()?.Nome + " " + getCurriculo.FirstOrDefault()?.SobreNome; ;
                ViewBag.CodigoCandidatura = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault()?.Codigo_Candidatura;


                //Altera o status do curriculo para 2 = em analise pela equipe // somente se for menor que 3 pq 3 é o status da entrevista
                var status_candidatura = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault()?.Status_candidatura;
                if (status_candidatura < 3)
                {
                    var Candidatura_to_update = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault();
                    Candidatura_to_update.Status_candidatura = 2;
                    TryUpdateModel(Candidatura_to_update);
                    db.SaveChanges();
                }

                //tratamento de null exception e carregar na view o curriculo do candidato
                if (getCurriculo.Any())
                {
                    return View(getCurriculo);
                }
                else
                {
                    //não posso retornar nulo pra view, ela exige o codigo da vaga da qual esse null curriculo seria
                    return RedirectToAction("Ver_candidaturas", "Admin", new { id = db.Candidaturas.Where(linha => linha.Codigo_user == id_candidato).FirstOrDefault().Codigo_Vaga });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //Carrega a view agendarmento da entrevista com as informacoes do candidato------------------------------------------------ ESPERA UM ID COM O NOME ID_CANDIDATO----------------------------------------
        [HttpGet]
        public ActionResult Agendar_entrevista(int id_candidato)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }




                FAPSEntities db = new FAPSEntities();

                //Coloca na view bag o nome do candidato confirme o nome que esta no curriculo
                ViewBag.Candidato = db.Curriculo.FirstOrDefault()?.Nome + " " + db.Curriculo.FirstOrDefault()?.SobreNome;

                //instancia e copula a model interview q vai ser enviada para a Agendar Entrevista
                Interview entrevista = new Interview();
                entrevista.Codigo_user = id_candidato;

                //tratamento null / preenche o entrevistador na model interview
                if (db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario != null)
                {
                    entrevista.Entrevistador = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                }
                else
                {
                    entrevista.Entrevistador = "nenhum";
                }

                entrevista.Data_criacao = DateTime.Now;


                entrevista.Data_Entrevista = DateTime.Now;

                entrevista.Status_interview = "Em aberto";

                //pega a vaga que esse candidato esta concorrendo
                entrevista.Vaga = db.Candidaturas.Where(f => f.Codigo_user == id_candidato).FirstOrDefault().Vagas.Vaga;


                //*Manda pra a propria view a MODEL "entrevista" com as alteracoes somente faltando o preencimento da data da entrevista
                //**Na view deve ter HiddenFor para cada item da model acima para que esses campos não sejam editados pelo usuario e para que sejam salvos no modelo que a view vai enviar de voltar pra proxima action
                return View(entrevista);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //Agendamento da entrevista e salva a model que vem da view no db
        [HttpPost]
        public ActionResult Marcar_entrevista(Interview entrevista)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();


                //status 3 = aprovado para entrevista
                var Candidatura_to_update = db.Candidaturas.Where(f => f.Codigo_user == entrevista.Codigo_user).FirstOrDefault();
                Candidatura_to_update.Status_candidatura = 3;

                TryUpdateModel(Candidatura_to_update);


                db.Interview.Add(entrevista);

                db.SaveChanges();

                return RedirectToAction("Listar_interviews","Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }






        //Lista e controla entrevistas agendadas--------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Listar_interviews()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                var getInterviewsList = db.Interview.Where(f => f.Status_interview != "Concluido").ToList();


                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + " Listou as entrevistas";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                return View(getInterviewsList);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }
        }



        //Recebe um form do tipo feedback que vem de uma modal da view Listar_interviews
        [HttpPost]
        public ActionResult Feedback(Feedback fb)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();
                db.Feedback.Add(fb);
                db.SaveChanges();


                return RedirectToAction("Concluir_interview", "Admin", new { id = fb.Codigo_entrevista });
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //Concluir Interview : Remove do sistema a entrevista e a candidatura relacionada
        [HttpGet]
        public ActionResult Concluir_interview(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                //Abaixo eu busco pelo id do canditato que será necessario para remover a candidatura na linha seguinte a essa
                var id_user = db.Interview.Where(f => f.Codigo_entrevista == id).FirstOrDefault()?.Codigo_user;

                //Removendo a candidatura do sistema
                var Candidatura_to_delete = db.Candidaturas.Where(f => f.Codigo_user == id_user)?.FirstOrDefault();
                db.Candidaturas.Remove(Candidatura_to_delete);

                //Mudando o status da interview para Concluido
                Interview i = db.Interview.Find(id);
                i.Status_interview = "Concluido";

                TryUpdateModel(i);

                db.SaveChanges();

                return RedirectToAction("Listar_interviews", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //Deletar Interview
        [HttpGet]
        public ActionResult Deletar_interview(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                //Para deletar uma entrevista eu preciso regressar a candidatura do candidato ao status 2 (em analise)
                //Abaixo eu busco pelo id do canditato que será necessario para alterar a candidatura na linha seguinte a essa
                var id_user = db.Interview.Where(f => f.Codigo_entrevista == id).FirstOrDefault()?.Codigo_user;

                //Alteração do status da candidatura para 2
                var Candidatura_to_update = db.Candidaturas.Where(f => f.Codigo_user == id_user)?.FirstOrDefault();
                Candidatura_to_update.Status_candidatura = 2;
                TryUpdateModel(Candidatura_to_update);


                //agora podemos deletar a entrevista
                Interview i = db.Interview.Find(id);
                db.Interview.Remove(i);


                db.SaveChanges();


                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + " Deletou a entrevista do candidato " + id_user;
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################



                return RedirectToAction("Listar_interviews", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //Chama a partial view que carregada com a interview que sera editada
        [HttpGet]
        public ActionResult Editar_interview(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                Interview i = db.Interview.Where(f => f.Codigo_entrevista == id).FirstOrDefault();


                return PartialView("_Editar_interview", i);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //BackEnd do _Editar_interview (salvar)
        [HttpPost]
        public ActionResult Salvar_interview(Interview i)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                //Procura a vaga a ser salva a altera item por item conforme oque veio da view
                var to_update = db.Interview.Where(f => f.Codigo_entrevista == i.Codigo_entrevista).FirstOrDefault();

                to_update.Data_Entrevista = i.Data_Entrevista;


                TryUpdateModel(to_update);
                db.SaveChanges();

                return RedirectToAction("Listar_interviews", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //Lista e controla Usuarios---------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Listar_users()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                var getUserList = db.Usuarios.ToList();

                return View(getUserList);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //Recebe da view admin o id da vaga que precisa alterar : UPDATE
        [HttpGet]
        public ActionResult Listar_usuario_to_update(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                var user_to_update = db.Usuarios.Where(f => f.Codigo_user == id).FirstOrDefault();

                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + "Alterou informacoes do usuario " + user_to_update.Usuario;
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################


                return View("Alterar_usuario", user_to_update);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //Recebe a vaga editada da view e salva ela
        [HttpPost]
        public ActionResult Alterar_usuario(Usuarios user_to_update)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                //Procura a vaga a ser salva a altera item por item conforme oque veio da view
                var to_update = db.Usuarios.Where(f => f.Codigo_user == user_to_update.Codigo_user).FirstOrDefault();
                to_update.Codigo_user = user_to_update.Codigo_user;
                to_update.Usuario = user_to_update.Usuario;
                to_update.Senha = user_to_update.Senha;
                to_update.role = user_to_update.role;

                TryUpdateModel(to_update);
                db.SaveChanges();

                return RedirectToAction("Listar_users", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //Deletar Usuario : DELETE
        [HttpGet]
        public ActionResult Deletar_user(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                Usuarios u = db.Usuarios.Find(id);
                db.Usuarios.Remove(u);
                db.SaveChanges();


                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + "deletou o usuario " + u.Usuario;
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################


                return RedirectToAction("Listar_users", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //Carrega a view do cadastrar usuario : CREATE
        public ActionResult Cadastrar_usuario()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }

                FAPSEntities db = new FAPSEntities();
                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + "Cadastrou um curriculo";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //BackEnd do cadastrar vagas (salvar)
        [HttpPost]
        public ActionResult Confirmar_usuario(Usuarios user)
        {
            try
            {
                FAPSEntities db = new FAPSEntities();
                db.Usuarios.Add(user);
                db.SaveChanges();

                return RedirectToAction("Listar_users", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }




        //Listar Curriculos-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Listar_curriculos()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                var getCurriculoList = db.Curriculo.ToList();


                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + "Listou os curriculos";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################


                return View(getCurriculoList);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //Detalhes do curriculo
        [HttpGet]
        public ActionResult Detalhes_curriculo(int id)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }




                FAPSEntities db = new FAPSEntities();


                //Consulta no db o curriculo do candidato
                var getCurriculo = db.Curriculo.Where(f => f.codigo_user == id);

                ViewBag.nome = getCurriculo.FirstOrDefault()?.Nome + " " + getCurriculo.FirstOrDefault()?.SobreNome;


                return View(getCurriculo);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }



        //Deletar Curriculo : DELETE  -------- ESPERA UM ID COM O NOME ID_CODIGO_USER
        [HttpGet]
        public ActionResult Deletar_curriculo(int id_codigo_user)
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                Curriculo c = db.Curriculo.Where(f => f.codigo_user == id_codigo_user).FirstOrDefault();


                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + " deletou o curriculo do usuario " + c.Nome;
                log.Data = DateTime.Now;
                db.Log.Add(log);
                //#################################-log-#######################################################



                db.Curriculo.Remove(c);
                db.SaveChanges();



                return RedirectToAction("Listar_curriculos", "Admin");
            }
            catch (Exception ex)
            {
                ViewBag.Erro = ex.Message;
                return View("Erro_generico");
            }

        }


        //Listar Log-----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult Listar_log()
        {
            try
            {
                //Valida se a sessão do usuario ainda existe e se ele esta logado
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }



                FAPSEntities db = new FAPSEntities();

                var getLogList = db.Log.OrderByDescending(f => f.Data).ToList();

                return View(getLogList);
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
                int? admin_id;
                if (User_id() == null)
                {
                    return View("Error");
                }
                else
                {
                    admin_id = User_id();
                }


                FAPSEntities db = new FAPSEntities();

                //#############################Registrando log administrador no DB#############################
                var Usuario = db.Usuarios.Where(f => f.Codigo_user == admin_id).FirstOrDefault()?.Usuario;
                Log log = new Log();
                log.Codigo_user = (int)admin_id;
                log.Log1 = "Adminstrador " + Usuario + " Saiu do sistema (Logout)";
                log.Data = DateTime.Now;
                db.Log.Add(log);
                db.SaveChanges();
                //#################################-log-#######################################################

                Session["id_admin"] = null;

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