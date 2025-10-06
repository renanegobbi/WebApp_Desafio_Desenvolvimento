using System.ComponentModel.DataAnnotations;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Solicitação para criação/edição de Departamento
    /// </summary>
    public class DepartamentoRequest
    {
        /// <summary>
        /// ID do Departamento (0 para inserir)
        /// </summary>
        public int id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MaxLength(100, ErrorMessage = "A descrição não pode ultrapassar 100 caracteres")]
        public string descricao { get; set; }
    }
}
