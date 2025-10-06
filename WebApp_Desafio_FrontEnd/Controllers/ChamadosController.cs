using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApp_Desafio_FrontEnd.ApiClients.Desafio_API;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;

        public ChamadosController(IHostingEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public IActionResult Listar()
        {
            // Busca de dados está na Action Datatable()
            var chamadosApiClient = new ChamadosApiClient();
            ViewData["ApiBaseUrl"] = chamadosApiClient.GetApiBaseUrl();
            return View();
        }

        [HttpGet]
        public IActionResult Datatable()
        {
            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var lstChamados = chamadosApiClient.ChamadosListar();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    length = lstChamados.Count,
                    data = lstChamados
                };

                return Ok(dataTableVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            var chamadoVM = new ChamadoViewModel()
            {
                DataAbertura = DateTime.Now
            };
            ViewData["Title"] = "Cadastrar Novo Chamado";

            var chamadosApiClient = new ChamadosApiClient();
            var teste = chamadosApiClient.GetApiBaseUrl();
            ViewData["ApiBaseUrl"] = chamadosApiClient.GetApiBaseUrl();
            var lstChamados = chamadosApiClient.ChamadosListar();
            

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            // inicializa a lista de solicitantes
            chamadoVM.listaSolicitantes = new List<ListaSolicitantes>();

            // adiciona os solicitantes distintos (pelo nome)
            var solicitantesUnicos = lstChamados
                .GroupBy(c => c.Solicitante)
                .Select(g => new ListaSolicitantes
                {
                    Id = g.First().ID, // ou 0 se o ID do chamado não for o do solicitante
                    Solicitante = g.Key
                })
                .OrderBy(s => s.Solicitante)
                .ToList();

            chamadoVM.listaSolicitantes.AddRange(solicitantesUnicos);

            return View("Cadastrar", chamadoVM);
        }

        [HttpPost]
        public IActionResult Cadastrar(ChamadoViewModel chamadoVM)
        {
            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var response = chamadosApiClient.ChamadoGravar(chamadoVM);

                return Json(response); // retorna JSON direto pro JS
            }
            catch (Exception ex)
            {
                return Json(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var chamadoVM = new ChamadoViewModel()
            {
                DataAbertura = DateTime.Now
            };
            ViewData["Title"] = "Editar Chamado";

            var chamadosApiClient = new ChamadosApiClient();
            var chamado = chamadosApiClient.ChamadoObter(id);
            var lstChamados = chamadosApiClient.ChamadosListar();
            ViewData["ApiBaseUrl"] = chamadosApiClient.GetApiBaseUrl();

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            // inicializa a lista de solicitantes
            chamadoVM.listaSolicitantes = new List<ListaSolicitantes>();

            // adiciona os solicitantes distintos (pelo nome)
            var solicitantesUnicos = lstChamados
                .GroupBy(c => c.Solicitante)
                .Select(g => new ListaSolicitantes
                {
                    Id = g.First().ID, // ou 0 se o ID do chamado não for o do solicitante
                    Solicitante = g.Key
                })
                .OrderBy(s => s.Solicitante)
                .ToList();

            chamadoVM.listaSolicitantes.AddRange(solicitantesUnicos);
            chamadoVM.ID = id;
            chamadoVM.IdDepartamento = chamado.IdDepartamento;
            chamadoVM.Assunto = chamado.Assunto;
            chamadoVM.Solicitante = chamado.Solicitante;
            chamadoVM.DataAbertura = chamado.DataAbertura;
            chamadoVM.Departamento = chamado.Departamento;

            return View("Editar", chamadoVM);
        }

        //[HttpGet]
        //public IActionResult Editar(int id)
        //{
        //    try
        //    {
        //        var chamadosApiClient = new ChamadosApiClient();
        //        var chamado = chamadosApiClient.ChamadoObter(id);

        //        var departamentosApiClient = new DepartamentosApiClient();
        //        ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();

        //        return View("Editar", chamado);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseViewModel(ex));
        //    }
        //}


        //[HttpGet]
        //public IActionResult Editar([FromRoute] int id)
        //{
        //    //ViewData["Title"] = "Cadastrar Novo Chamado";

        //    //try
        //    //{
        //    //    var chamadosApiClient = new ChamadosApiClient();
        //    //    var chamadoVM = chamadosApiClient.ChamadoObter(id);

        //    //    var departamentosApiClient = new DepartamentosApiClient();
        //    //    ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();

        //    //    return View("Cadastrar", chamadoVM);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    return BadRequest(new ResponseViewModel(ex));
        //    //}

        //    try
        //    {
        //        var chamadosApiClient = new ChamadosApiClient();
        //        var chamado = chamadosApiClient.ChamadoObter(id);
        //        var departamentosApiClient = new DepartamentosApiClient();

        //        ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();

        //        return View("Editar", chamado);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new ResponseViewModel(ex));
        //    }
        //}

        [HttpPost]
        public IActionResult Excluir([FromRoute] int id)
        {
            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Chamados",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Chamado {id}.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Report()
        {
            string mimeType = string.Empty;
            int extension = 1;
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptChamados.rdlc");
            //
            // ... parameters
            //
            LocalReport localReport = new LocalReport(path);

            // Carrega os dados que serão apresentados no relatório
            var chamadosApiClient = new ChamadosApiClient();
            var lstChamados = chamadosApiClient.ChamadosListar();

            localReport.AddDataSource("dsChamados", lstChamados);

            // Renderiza o relatório em PDF
            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            //return File(reportResult.MainStream, "application/pdf");
            return File(reportResult.MainStream, "application/octet-stream", "rptChamados.pdf");
        }

    }
}
