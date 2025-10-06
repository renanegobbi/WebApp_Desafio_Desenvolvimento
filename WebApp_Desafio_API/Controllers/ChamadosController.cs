using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp_Desafio_API.ViewModels;
using WebApp_Desafio_API.ViewModels.Enums;
using WebApp_Desafio_BackEnd.Business;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_API.Controllers
{
    /// <summary>
    /// ChamadosController
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : Controller
    {
        private ChamadosBLL bll = new ChamadosBLL();

        /// <summary>
        /// Lista todos os chamados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ChamadoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Listar")]
        public IActionResult Listar()
        {
            try
            {
                var _lst = this.bll.ListarChamados();

                var lst = from chamado in _lst
                          select new ChamadoResponse()
                          {
                              id = chamado.ID,
                              assunto = chamado.Assunto,
                              solicitante = chamado.Solicitante,
                              idDepartamento = chamado.IdDepartamento,
                              departamento = chamado.Departamento,
                              dataAbertura = chamado.DataAbertura
                          };

                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Type = AlertTypes.error
                });
            }
        }

        /// <summary>
        /// Obtém dados de um chamado específico
        /// </summary>
        /// <param name="idChamado">O ID do chamado a ser obtido</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ChamadoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Obter")]
        public IActionResult Obter([FromQuery] int idChamado)
        {
            try
            {
                var _chamado = this.bll.ObterChamado(idChamado);

                if (_chamado == null)
                {
                    return BadRequest(new ErrorViewModel
                    {
                        Message = "Chamado não encontrado.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Type = AlertTypes.error
                    });
                }

                var chamado = new ChamadoResponse()
                {
                    id = _chamado.ID,
                    assunto = _chamado.Assunto,
                    solicitante = _chamado.Solicitante,
                    idDepartamento = _chamado.IdDepartamento,
                    departamento = _chamado.Departamento,
                    dataAbertura = _chamado.DataAbertura
                };

                return Ok(chamado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Type = AlertTypes.error
                });
            }
        }

        /// <summary>
        /// Obtém o nome do solicitante
        /// </summary>
        /// <param name="nome">Obtém os nomes dos solicitantes</param>
        /// <returns></returns>
        [HttpGet("solicitantes")]
        [ProducesResponseType(typeof(ChamadoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        public IActionResult BuscarSolicitantes([FromQuery] string nome)
        {
            try
            {
                var dal = new ChamadosDAL();
                var solicitantes = dal.ObterNomeSolicitante(nome);

                var result = solicitantes.Select(s => new { id = s.ID, nome = s.Nome }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Type = AlertTypes.error
                });
            }
        }

        /// <summary>
        /// Grava os dados de um chamado
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ChamadoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ChamadoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Gravar")]
        public IActionResult Gravar([FromBody] ChamadoRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var modelStateError = ModelState.Values.SelectMany(v => v.Errors)
                                                      .FirstOrDefault()?.ErrorMessage ?? "Request inválido.";
                    return BadRequest(new ErrorViewModel
                    {
                        Message = modelStateError,
                        StatusCode = StatusCodes.Status400BadRequest,
                        Type = AlertTypes.warning
                    });
                }

                if (request == null)
                {
                    return BadRequest(new ErrorViewModel
                    {
                        Message = "Request não informado.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Type = AlertTypes.error
                    });
                }

                if (request.dataAbertura.Date < DateTime.Today.Date)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrorViewModel
                    {
                        Message = "Não é permitido abrir chamados com data retroativa.",
                        StatusCode = StatusCodes.Status422UnprocessableEntity,
                        Type = AlertTypes.warning
                    });
                }

                var departamento = new DepartamentosDAL().ObterDepartamento(request.idDepartamento);
                if (departamento == null || departamento.ID == 0)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrorViewModel
                    {
                        Message = $"Departamento {request.idDepartamento} não existe.",
                        StatusCode = StatusCodes.Status422UnprocessableEntity,
                        Type = AlertTypes.warning
                    });
                }

                // grava chamado
                var id = this.bll.GravarChamado(
                    request.id,
                    request.assunto,
                    request.solicitante,
                    request.idDepartamento,
                    request.dataAbertura
                );

                if (id <= 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                    {
                        Message = "Erro ao gravar o chamado.",
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Type = AlertTypes.error
                    });
                }

                var response = new ChamadoResponse
                {
                    id = id,
                    assunto = request.assunto,
                    solicitante = request.solicitante,
                    idDepartamento = request.idDepartamento,
                    dataAbertura = request.dataAbertura,
                    departamento = departamento.Descricao
                };

                if (request.id == 0)
                    return CreatedAtAction(nameof(Obter), new { idChamado = id }, response);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Type = AlertTypes.error
                });
            }
        }

        /// <summary>
        /// Exclui um chamado específico
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(Chamado), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Excluir/{idChamado}")]
        public IActionResult Excluir([FromRoute] int idChamado)
        {
            try
            {
                var chamado = this.bll.ObterChamado(idChamado);

                if (chamado == null || chamado.ID == 0)
                {
                    return BadRequest(new ErrorViewModel
                    {
                        Message = $"Chamado {idChamado} não encontrado.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Type = AlertTypes.error
                    });
                }

                var resultado = this.bll.ExcluirChamado(idChamado);

                if (resultado == null)
                {
                    return NotFound(new ErrorViewModel
                    {
                        Message = $"Chamado {idChamado} não encontrado.",
                        StatusCode = StatusCodes.Status404NotFound,
                        Type = AlertTypes.error
                    });
                }

                var chamadoResponse = new ChamadoResponse()
                {
                    id = chamado.ID,
                    assunto = chamado.Assunto,
                    solicitante = chamado.Solicitante,
                    idDepartamento = chamado.IdDepartamento,
                    departamento = chamado.Departamento,
                    dataAbertura = chamado.DataAbertura
                };

                return Ok(chamadoResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                {
                    Message = ex.Message,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Type = AlertTypes.error
                });
            }
        }
    }
}
