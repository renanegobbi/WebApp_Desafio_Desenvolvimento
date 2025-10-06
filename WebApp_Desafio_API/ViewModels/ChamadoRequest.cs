using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Solicitação da chamada
    /// </summary>
    public class ChamadoRequest
    {
        /// <summary>
        /// ID do Chamado (0 para inserir)
        /// </summary>
        public int id { get; set; }

        [Required(ErrorMessage = "O Assunto é obrigatório")]
        [MaxLength(120, ErrorMessage = "O Assunto não pode ultrapassar 120 caracteres")]
        public string assunto { get; set; }

        [Required(ErrorMessage = "O Solicitante é obrigatório")]
        [MaxLength(100, ErrorMessage = "O Solicitante não pode ultrapassar 100 caracteres")]
        public string solicitante { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "O Departamento é obrigatório")]
        public int idDepartamento { get; set; }

        [Required(ErrorMessage = "A Data de Abertura é obrigatória")]
        public DateTime dataAbertura { get; set; }
    }
}
