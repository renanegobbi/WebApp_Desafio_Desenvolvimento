using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using WebApp_Desafio_FrontEnd.ApiClients.Desafio_API;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class DepartamentosController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;

        public DepartamentosController(IHostingEnvironment hostEnvironment)
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
            var departamentosApiClient = new DepartamentosApiClient();
            ViewData["ApiBaseUrl"] = departamentosApiClient.GetApiBaseUrl();
            return View();
        }

        [HttpGet]
        public IActionResult Datatable()
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var lstDepartamentos = departamentosApiClient.DepartamentosListar();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    length = lstDepartamentos.Count,
                    data = lstDepartamentos
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
            var departamentoVM = new DepartamentoViewModel();

            ViewData["Title"] = "Cadastrar Novo Departamento";

            var departamentosApiClient = new DepartamentosApiClient();
            ViewData["ApiBaseUrl"] = departamentosApiClient.GetApiBaseUrl();
            var lstDepartamentos = departamentosApiClient.DepartamentosListar();

            try
            {
                ViewData["ListaDepartamentos"] = lstDepartamentos;
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View("Cadastrar", departamentoVM);
        }

        [HttpPost]
        public IActionResult Cadastrar(DepartamentoViewModel departamentoVM)
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var response = departamentosApiClient.DepartamentoGravar(departamentoVM);

                return Json(response);
            }
            catch (Exception ex)
            {
                return Json(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var departamentoVM = new DepartamentoViewModel();
            ViewData["Title"] = "Editar Departamento";

            var departamentosApiClient = new DepartamentosApiClient();
            var departamento = departamentosApiClient.DepartamentoObter(id);

            try
            {
                var lstDepartamentos = departamentosApiClient.DepartamentosListar();
                ViewData["ApiBaseUrl"] = departamentosApiClient.GetApiBaseUrl();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            departamentoVM.ID = id;
            departamentoVM.Descricao = departamento.Descricao;

            return View("Editar", departamentoVM);
        }

        [HttpPost]
        public IActionResult Excluir([FromRoute] int id)
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentoExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Departamentos",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o departamento {id}.");
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
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptDepartamentos.rdlc");
            //
            // ... parameters
            //
            LocalReport localReport = new LocalReport(path);

            // Carrega os dados que serão apresentados no relatório
            var departamentosApiClient = new DepartamentosApiClient();
            var lstDepartamentos = departamentosApiClient.DepartamentosListar();

            localReport.AddDataSource("dsDepartamentos", lstDepartamentos);

            // Renderiza o relatório em PDF
            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            //return File(reportResult.MainStream, "application/pdf");
            return File(reportResult.MainStream, "application/octet-stream", "rptDepartamentos.pdf");
        }

    }
}
