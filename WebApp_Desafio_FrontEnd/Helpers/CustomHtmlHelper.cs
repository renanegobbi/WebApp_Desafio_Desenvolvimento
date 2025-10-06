using Microsoft.AspNetCore.Html;

namespace WebApp_Desafio_FrontEnd.Helpers
{
    public static class CustomHtmlHelper
    {
        public static HtmlString IconeCampoObrigatorio(string idControle)
        {
            return new HtmlString($"<i class=\"fa fa-exclamation-circle\" style=\"color:#D2312D;\"></i>&nbsp;&nbsp;<label for=\"{idControle}\" class=\"error\"></label>");
        }

        public static HtmlString IconeInformacao(string titulo, string mensagem, string posicao)
        {
            return new HtmlString($"<i class=\"fas fa-info-circle text-primary\" data-toggle =\"popover\" data-placement=\"{posicao}\" data-content=\"{mensagem}\" data-trigger=\"hover\" data-original-title=\"{titulo}\" data-container=\"body\" data-html=\"true\" style=\"cursor:help;\"></i>");
        }
    }

}
