# WebApp_Desafio_Desenvolvimento

Este projeto é um desenvolvimento web em ASP.NET Core MVC, utilizando SQLite como banco de dados.
O objetivo foi analisar, corrigir e aprimorar uma aplicação de registro de chamados, incluindo a implementação de novos módulos, relatórios e validações.

A aplicação é composta por telas CRUD, relatórios RDLC e uma camada de acesso a dados simplificada.

<br/>

---

## 🎯 Objetivos do Desafio
As principais tarefas propostas foram:

1. Implementar o módulo **Departamentos** (CRUD completo).  
2. Corrigir erros existentes no módulo **Chamados**.  
3. Implementar o **relatório de Departamentos** (chamado via interface).  
4. Adicionar **duplo clique** nas telas de listagem para editar registros.  
5. Implementar **validações de entrada** (tamanho de campos, restrição numérica, etc.).

### 🔹 Desafio Extra (opcional)
- Bloquear criação de chamados com **data retroativa**.  
- Implementar **autocomplete** para pesquisa de solicitantes.  
- Outras melhorias de usabilidade e código limpo.

---

## 🛠️ Tecnologias Utilizadas
### 🧩 Back-end
- **C# / ASP.NET Core 2.1**
- **Entity Framework Core (SQLite)**
- **System.Data.SQLite.Core 1.0.116**
- **Newtonsoft.Json 13.0.1**
- **Microsoft.Windows.Compatibility 3.1.1**
- **Swagger (Swashbuckle.AspNetCore 4.0.1)**
- **System.ServiceModel.Http / Primitives 4.7.0**

### 🎨 Front-end (WebApp)
- **ASP.NET Core MVC 2.1**
- **AspNetCore.Reporting 2.1.0** (para geração de relatórios RDLC)
- **System.Drawing.Common 4.5.1**
- **System.CodeDom / System.Text.Encoding.CodePages**
- **Bootstrap 4 + jQuery**
- **Bootstrap Datepicker** (em `wwwroot/lib/bootstrap-datepicker`)

### 🗄️ Banco de Dados
- **SQLite**  

---

## 💻 Funcionalidades Implementadas
- ✅ CRUD completo para **Departamentos**  
- ✅ Correções no CRUD de **Chamados**  
- ✅ Integração de **relatórios RDLC** (Chamados e Departamentos)  
- ✅ Validação de campos (textos e números)  
- ✅ Duplo clique para edição de registros  
- ✅ Melhorias visuais e de usabilidade  
- ⚙️ (Opcional) Validação de data não retroativa para novos chamados  

---

## 🧪 Como Executar o Projeto

### 🔧 Pré-requisitos
- **Visual Studio 2022 ou 2019**
- **SDK .NET Core 2.1**
- **Extensão RDLC Report Designer**  
  🔗 [Microsoft RDLC Report Designer 2022](https://marketplace.visualstudio.com/items?itemName=ProBITools.MicrosoftRdlcReportDesignerforVisualStudio2022)

---

### ▶️ Passos

1. Clone o repositório:

```bash
git clone https://github.com/renanegobbi/WebApp_Desafio_Desenvolvimento.git
```

2. Abra o projeto no Visual Studio.

3. Restaure os pacotes NuGet: Tools → NuGet Package Manager → Restore Packages

4. Compile e execute o projeto (Ctrl + F5).

5. O banco SQLite já vem com dados de exemplo (App_Data/WebApp.db).

  - O projeto utiliza .NET Core 2.1 e o banco de dados SQLite.

  - Durante a execução, o arquivo do banco é automaticamente copiado para o diretório:
  \WebApp_Desafio_Desenvolvimento\WebApp_Desafio_API\bin\Debug\netcoreapp2.1\Dados\DesafioDB.db

