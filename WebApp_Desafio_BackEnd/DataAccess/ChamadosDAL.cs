using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;
using System.Data.Common;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class ChamadosDAL : BaseDAL
    {
        private const string ANSI_DATE_FORMAT = "yyyy-MM-dd";

        public IEnumerable<Chamado> ListarChamados()
        {
            IList<Chamado> lstChamados = new List<Chamado>();

            DataTable dtChamados = new DataTable();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {

                    dbCommand.CommandText = 
                        "SELECT chamados.ID, " + 
                        "       Assunto, " +
                        "       Solicitante, " +
                        "       IdDepartamento, " +
                        "       departamentos.Descricao AS Departamento, " + 
                        "       DataAbertura " + 
                        "FROM chamados " + 
                        "INNER JOIN departamentos " +
                        "   ON chamados.IdDepartamento = departamentos.ID ";

                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        var chamado = Chamado.Empty;

                        while (dataReader.Read())
                        {
                            chamado = new Chamado();

                            if (!dataReader.IsDBNull(0))
                                chamado.ID = dataReader.GetInt32(0);
                            if (!dataReader.IsDBNull(1))
                                chamado.Assunto = dataReader.GetString(1);
                            if (!dataReader.IsDBNull(2))
                                chamado.Solicitante = dataReader.GetString(2);
                            if (!dataReader.IsDBNull(3))
                                chamado.IdDepartamento = dataReader.GetInt32(3);
                            if (!dataReader.IsDBNull(4))
                                chamado.Departamento = dataReader.GetString(4);
                            if (!dataReader.IsDBNull(5))
                                chamado.DataAbertura = DateTime.Parse(dataReader.GetString(5));

                            lstChamados.Add(chamado);
                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }

            }

            return lstChamados;
        }

        public Chamado ObterChamado(int idChamado)
        {
            Chamado chamado = null;

            DataTable dtChamados = new DataTable();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "SELECT chamados.ID, " +
                        "       Assunto, " +
                        "       Solicitante, " +
                        "       IdDepartamento, " +
                        "       departamentos.Descricao AS Departamento, " +
                        "       DataAbertura " +
                        "FROM chamados " +
                        "INNER JOIN departamentos " +
                        "   ON chamados.IdDepartamento = departamentos.ID " +
                        $"WHERE chamados.ID = {idChamado}";

                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            chamado = new Chamado();

                            if (!dataReader.IsDBNull(0))
                                chamado.ID = dataReader.GetInt32(0);
                            if (!dataReader.IsDBNull(1))
                                chamado.Assunto = dataReader.GetString(1);
                            if (!dataReader.IsDBNull(2))
                                chamado.Solicitante = dataReader.GetString(2);
                            if (!dataReader.IsDBNull(3))
                                chamado.IdDepartamento = dataReader.GetInt32(3);
                            if (!dataReader.IsDBNull(4))
                                chamado.Departamento = dataReader.GetString(4);
                            if (!dataReader.IsDBNull(5))
                                chamado.DataAbertura = DateTime.Parse(dataReader.GetString(5));

                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }

            }

            return chamado;
        }

        public IEnumerable<Solicitante> ObterNomeSolicitante(string nome)
        {
            var lista = new List<Solicitante>();

            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open();
                using (var cmd = dbConnection.CreateCommand())
                {
                    cmd.CommandText = "SELECT DISTINCT ID, Solicitante AS Nome FROM chamados WHERE Solicitante LIKE @nome LIMIT 10";
                    cmd.Parameters.AddWithValue("@nome", $"%{nome}%");

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Solicitante
                            {
                                ID = reader.GetInt32(0),
                                Nome = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public int GravarChamado(int ID, string Assunto, string Solicitante, int IdDepartamento, DateTime DataAbertura)
        {
            int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open(); // <-- ESSENCIAL

                using (var dbCommand = dbConnection.CreateCommand())
                {
                    if (ID == 0)
                    {
                        dbCommand.CommandText =
                            "INSERT INTO chamados (Assunto, Solicitante, IdDepartamento, DataAbertura) " +
                            "VALUES (@Assunto, @Solicitante, @IdDepartamento, @DataAbertura)";
                        //"SELECT last_insert_rowid();";

                        dbCommand.Parameters.AddWithValue("@Assunto", Assunto);
                        dbCommand.Parameters.AddWithValue("@Solicitante", Solicitante);
                        dbCommand.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);
                        dbCommand.Parameters.AddWithValue("@DataAbertura", DataAbertura.ToString(ANSI_DATE_FORMAT));

                        dbCommand.ExecuteNonQuery();

                        // agora pega o id da mesma conexão
                        dbCommand.CommandText = "SELECT last_insert_rowid()";
                        dbCommand.Parameters.Clear(); // limpa os parâmetros, senão ele reclama
                        return Convert.ToInt32(dbCommand.ExecuteScalar());
                    }
                    else
                    {
                        dbCommand.CommandText =
                            "UPDATE chamados " +
                            "SET Assunto=@Assunto, " +
                            "    Solicitante=@Solicitante, " +
                            "    IdDepartamento=@IdDepartamento, " +
                            "    DataAbertura=@DataAbertura " +
                            "WHERE ID=@ID";

                        dbCommand.Parameters.AddWithValue("@ID", ID);
                        dbCommand.Parameters.AddWithValue("@Assunto", Assunto);
                        dbCommand.Parameters.AddWithValue("@Solicitante", Solicitante);
                        dbCommand.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);
                        dbCommand.Parameters.AddWithValue("@DataAbertura", DataAbertura.ToString(ANSI_DATE_FORMAT));

                        int rows = dbCommand.ExecuteNonQuery();
                        return rows > 0 ? ID : -1; // se atualizou retorna o mesmo ID, senão -1
                    }

                }
            }
        }

        public int ExcluirChamado(int idChamado)
        {
            //int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = "DELETE FROM chamados WHERE ID = @ID";
                    dbCommand.Parameters.AddWithValue("@ID", idChamado);

                    dbConnection.Open();
                    int regsAfetados = dbCommand.ExecuteNonQuery();

                    // se excluiu, retorna o ID; senão, retorna -1
                    return regsAfetados > 0 ? idChamado : -1;
                }

            }

           // return (regsAfetados > 0);
        }
    }
}