using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;

namespace WebApp_Desafio_FrontEnd.ApiClients.Desafio_API
{
    public class DepartamentosApiClient : BaseClient
    {
        private const string tokenAutenticacao = "AEEFC184-9F62-4B3E-BB93-BE42BF0FFA36";

        private const string departamentosListUrl = "api/Departamentos/Listar";
        private const string departamentosObterUrl = "api/Departamentos/Obter";
        private const string departamentosGravarUrl = "api/Departamentos/Gravar";
        private const string departamentosExcluirUrl = "api/Departamentos/Excluir";

        private string desafioApiUrl = "https://localhost:44388/"; 
        public string GetApiBaseUrl() => desafioApiUrl;

        public DepartamentosApiClient() : base()
        {
            //TODO
        }

        public List<DepartamentoViewModel> DepartamentosListar()
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = default(Dictionary<string, object>);

            var response = base.Get($"{desafioApiUrl}{departamentosListUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(json);
        }

        public DepartamentoViewModel DepartamentoObter(int idDepartamento)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idDepartamento", idDepartamento }
            };

            var response = base.Get($"{desafioApiUrl}{departamentosObterUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<DepartamentoViewModel>(json);
        }

        public ResponseViewModel DepartamentoGravar(DepartamentoViewModel departamento)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var response = base.Post($"{desafioApiUrl}{departamentosGravarUrl}", departamento, headers);
            string json = base.ReadHttpWebResponseMessage(response);

            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                var departamentoVM = JsonConvert.DeserializeObject<DepartamentoViewModel>(json);

                return new ResponseViewModel(
                    $"Departamento gravado com sucesso! (ID: {departamentoVM.ID})",
                    AlertTypes.success,
                    "Departamentos",
                    "Listar"
                );
            }

            try
            {
                dynamic errorObj = JsonConvert.DeserializeObject(json);

                string message = errorObj?.Message ?? "Erro ao processar requisição.";
                string typeStr = errorObj?.Type ?? "error";

                Enum.TryParse(typeStr, true, out AlertTypes alertType);

                return new ResponseViewModel(
                    message,
                    alertType == 0 ? AlertTypes.error : alertType,
                    "Departamentos",
                    "Cadastrar"
                );
            }
            catch (Exception)
            {
                return new ResponseViewModel(
                    "Erro inesperado ao processar resposta da API.",
                    AlertTypes.error,
                    "Departamentos",
                    "Cadastrar"
                );
            }
        }

        public bool DepartamentoExcluir(int idDepartamento)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idDepartamento", idDepartamento }
            };

            var response = base.Delete($"{desafioApiUrl}{departamentosExcluirUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
