using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApp_Desafio_API.ViewModels;
using WebApp_Desafio_API.ViewModels.Enums;
using WebApp_Desafio_BackEnd.Business;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_API.Controllers
{
    /// <summary>
    /// DepartamentosController
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentosController : Controller
    {
        private DepartamentosBLL bll = new DepartamentosBLL();

        /// <summary>
        /// Lista todos os departamentos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DepartamentoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Listar")]
        public IActionResult Listar()
        {
            try
            {
                var _lst = this.bll.ListarDepartamentos();

                var lst = from d in _lst
                          select new DepartamentoResponse()
                          {
                              id = d.ID,
                              descricao = d.Descricao
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
        /// Obtém um departamento específico
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(DepartamentoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Obter")]
        public IActionResult Obter([FromQuery] int idDepartamento)
        {
            try
            {
                var departamento = this.bll.ObterDepartamento(idDepartamento);
                if (departamento == null)
                {
                    return BadRequest(new ErrorViewModel
                    {
                        Message = "Departamento não encontrado.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Type = AlertTypes.error
                    });
                }

                return Ok(new DepartamentoResponse
                {
                    id = departamento.ID,
                    descricao = departamento.Descricao
                });
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
        /// Insere/Atualiza um departamento
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(DepartamentoResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(DepartamentoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Gravar")]
        public IActionResult Gravar([FromBody] DepartamentoRequest request)
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

                var departamentos = this.bll.ListarDepartamentos();
                bool nomeDuplicado = departamentos
                      .Any(d => d.Descricao.Trim().ToUpper() == request.descricao.Trim().ToUpper()
                      && d.ID != request.id);

                if (nomeDuplicado)
                {
                    return StatusCode(StatusCodes.Status422UnprocessableEntity, new ErrorViewModel
                    {
                        Message = $"O departamento '{request.descricao}' já existe.",
                        StatusCode = StatusCodes.Status422UnprocessableEntity,
                        Type = AlertTypes.warning
                    });
                }

                var idNovo = this.bll.GravarDepartamento(
                    request.id,
                    request.descricao
                );

                if (idNovo <= 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new ErrorViewModel
                    {
                        Message = "Erro ao gravar o chamado.",
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Type = AlertTypes.error
                    });
                }

                var response = new DepartamentoResponse
                {
                    id = idNovo,
                    descricao = request.descricao
                };

                if (request.id == 0)
                    return CreatedAtAction(nameof(Obter), new { idDepartamento = idNovo }, response);

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
        /// Exclui um departamento específico
        /// </summary>
        [HttpDelete]
        [ProducesResponseType(typeof(Departamento), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorViewModel), StatusCodes.Status500InternalServerError)]
        [Route("Excluir/{idDepartamento}")]
        public IActionResult Excluir([FromRoute] int idDepartamento)
        {
            try
            {
                var departamento = this.bll.ObterDepartamento(idDepartamento);

                if (departamento == null || departamento.ID == 0)
                {
                    return BadRequest(new ErrorViewModel
                    {
                        Message = $"Departamento {idDepartamento} não encontrado.",
                        StatusCode = StatusCodes.Status400BadRequest,
                        Type = AlertTypes.error
                    });
                }

                var resultado = this.bll.ExcluirDepartamento(idDepartamento);

                if (resultado == null)
                {
                    return NotFound(new ErrorViewModel
                    {
                        Message = $"Departamento {idDepartamento} não encontrado.",
                        StatusCode = StatusCodes.Status404NotFound,
                        Type = AlertTypes.error
                    });
                }

                var chamadoResponse = new DepartamentoResponse()
                {
                    id = departamento.ID,
                    descricao = departamento.Descricao
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
