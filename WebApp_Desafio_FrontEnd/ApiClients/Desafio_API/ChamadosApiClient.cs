using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using WebApp_Desafio_FrontEnd.Models;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;

namespace WebApp_Desafio_FrontEnd.ApiClients.Desafio_API
{
    public class ChamadosApiClient : BaseClient
    {
        private const string tokenAutenticacao = "AEEFC184-9F62-4B3E-BB93-BE42BF0FFA36";

        private const string chamadosListUrl = "api/Chamados/Listar";
        private const string chamadosObterUrl = "api/Chamados/Obter";
        private const string chamadosGravarUrl = "api/Chamados/Gravar";
        private const string chamadosExcluirUrl = "api/Chamados/Excluir";

        private string desafioApiUrl = "https://localhost:44388/"; // Endereço API IIS-Express
        public string GetApiBaseUrl() => desafioApiUrl;

        public ChamadosApiClient() : base()
        {
            //TODO
        }

        public List<ChamadoViewModel> ChamadosListar()
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = default(Dictionary<string, object>); // Não há parâmetros para essa chamada

            var response = base.Get($"{desafioApiUrl}{chamadosListUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<List<ChamadoViewModel>>(json);
        }

        public ChamadoViewModel ChamadoObter(int idChamado)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idChamado", idChamado }
            };

            var response = base.Get($"{desafioApiUrl}{chamadosObterUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<ChamadoViewModel>(json);
        }

        //public ChamadoViewModel ChamadoGravar(ChamadoViewModel chamado)
        public ResponseViewModel ChamadoGravar(ChamadoViewModel chamado)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            //var response = base.Post($"{desafioApiUrl}{chamadosGravarUrl}", chamado, headers);

            //base.EnsureSuccessStatusCode(response);

            //string json = base.ReadHttpWebResponseMessage(response);

            //return JsonConvert.DeserializeObject<ChamadoViewModel>(json);

            //var response = base.Post($"{desafioApiUrl}{chamadosGravarUrl}", chamado, headers);

            //string json = base.ReadHttpWebResponseMessage(response);

            //if (response.StatusCode.Equals(200))
            //{
            //    // sucesso
            //    var chamadoCriado = JsonConvert.DeserializeObject<ChamadoViewModel>(json);
            //    return new ResponseViewModel(
            //        $"Chamado {chamado.ID} gravado com sucesso!",
            //        AlertTypes.success,
            //        "Chamados",
            //        "Listar");
            //}
            //else
            //{
            //    // erro → já vem no formato ResponseViewModel do backend
            //    var erro = JsonConvert.DeserializeObject<ResponseViewModel>(json);
            //    return erro;
            //}

            //var response = base.Post($"{desafioApiUrl}{chamadosGravarUrl}", chamado, headers);

            //string json = base.ReadHttpWebResponseMessage(response);

            //if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            //{
            //    var chamadoVM = JsonConvert.DeserializeObject<ChamadoViewModel>(json);

            //    return new ResponseViewModel(
            //        $"Chamado gravado com sucesso! (ID: {chamadoVM.ID})",
            //        AlertTypes.success,
            //        "Chamados",
            //        "Listar"
            //    );
            //}
            //else
            //{
            //    var error = JsonConvert.DeserializeObject<ResponseViewModel>(json);

            //    return new ResponseViewModel(
            //        error.Message,
            //        error.Type,
            //        "Chamados",
            //        "Cadastrar"
            //    );
            //}

            var response = base.Post($"{desafioApiUrl}{chamadosGravarUrl}", chamado, headers);
            string json = base.ReadHttpWebResponseMessage(response);

            // Sucesso → retorna mensagem padronizada
            if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
            {
                var chamadoVM = JsonConvert.DeserializeObject<ChamadoViewModel>(json);

                return new ResponseViewModel(
                    $"Chamado gravado com sucesso! (ID: {chamadoVM.ID})",
                    AlertTypes.success,
                    "Chamados",
                    "Listar"
                );
            }

            // Erro → API devolve ErrorViewModel (da API, não do Front)
            try
            {
                dynamic errorObj = JsonConvert.DeserializeObject(json);

                string message = errorObj?.Message ?? "Erro ao processar requisição.";
                string typeStr = errorObj?.Type ?? "error";

                // Converte string em enum AlertTypes
                Enum.TryParse(typeStr, true, out AlertTypes alertType);

                return new ResponseViewModel(
                    message,
                    alertType == 0 ? AlertTypes.error : alertType,
                    "Chamados",
                    "Cadastrar"
                );
            }
            catch (Exception)
            {
                return new ResponseViewModel(
                    "Erro inesperado ao processar resposta da API.",
                    AlertTypes.error,
                    "Chamados",
                    "Cadastrar"
                );
            }
        }

        public bool ChamadoExcluir(int idChamado)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idChamado", idChamado }
            };

            var response = base.Delete($"{desafioApiUrl}{chamadosExcluirUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }

    }
}
