using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SQLite;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class DepartamentosDAL : BaseDAL
    {
        public IEnumerable<Departamento> ListarDepartamentos()
        {
            IList<Departamento> lst = new List<Departamento>();

            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = "SELECT ID, Descricao FROM departamentos";
                dbConnection.Open();

                using (var reader = dbCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var d = new Departamento
                        {
                            ID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,
                            Descricao = !reader.IsDBNull(1) ? reader.GetString(1) : null
                        };
                        lst.Add(d);
                    }
                }
            }

            return lst;
        }

        public Departamento ObterDepartamento(int idDepartamento)
        {
            Departamento d = Departamento.Empty;

            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = "SELECT ID, Descricao FROM departamentos WHERE ID = @ID";
                dbCommand.Parameters.AddWithValue("@ID", idDepartamento);

                dbConnection.Open();
                using (var reader = dbCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        d = new Departamento
                        {
                            ID = !reader.IsDBNull(0) ? reader.GetInt32(0) : 0,
                            Descricao = !reader.IsDBNull(1) ? reader.GetString(1) : null
                        };
                    }
                }
            }

            return d;
        }

        public int GravarDepartamento(int ID, string Descricao)
        {
            //int regsAfetados;

            //using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            //using (var dbCommand = dbConnection.CreateCommand())
            //{
            //    if (ID == 0)
            //    {
            //        dbCommand.CommandText = "INSERT INTO departamentos (Descricao) VALUES (@Descricao)";
            //    }
            //    else
            //    {
            //        dbCommand.CommandText = "UPDATE departamentos SET Descricao=@Descricao WHERE ID=@ID";
            //        dbCommand.Parameters.AddWithValue("@ID", ID);
            //    }

            //    dbCommand.Parameters.AddWithValue("@Descricao", Descricao);

            //    dbConnection.Open();
            //    regsAfetados = dbCommand.ExecuteNonQuery();
            //}

            ////return regsAfetados > 0;
            //return regsAfetados > 0 ? ID : -1; // se atualizou retorna o mesmo ID, senão -1

            int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                dbConnection.Open(); // <-- ESSENCIAL

                using (var dbCommand = dbConnection.CreateCommand())
                {
                    if (ID == 0)
                    {
                        dbCommand.CommandText =
                            dbCommand.CommandText = "INSERT INTO departamentos (Descricao) VALUES (@Descricao)"; 

                        dbCommand.Parameters.AddWithValue("@Descricao", Descricao);

                        dbCommand.ExecuteNonQuery();

                        // agora pega o id da mesma conexão
                        dbCommand.CommandText = "SELECT last_insert_rowid()";
                        dbCommand.Parameters.Clear(); // limpa os parâmetros, senão ele reclama
                        return Convert.ToInt32(dbCommand.ExecuteScalar());
                    }
                    else
                    {
                        dbCommand.CommandText = "UPDATE departamentos SET Descricao=@Descricao WHERE ID=@ID";
                        dbCommand.Parameters.AddWithValue("@ID", ID);
                        dbCommand.Parameters.AddWithValue("@Descricao", Descricao);

                        int rows = dbCommand.ExecuteNonQuery();
                        return rows > 0 ? ID : -1; // se atualizou retorna o mesmo ID, senão -1
                    }

                }
            }
        }

        public bool ExcluirDepartamento(int idDepartamento)
        {
            int regsAfetados;

            using (var dbConnection = new SQLiteConnection(CONNECTION_STRING))
            using (var dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = "DELETE FROM departamentos WHERE ID=@ID";
                dbCommand.Parameters.AddWithValue("@ID", idDepartamento);

                dbConnection.Open();
                regsAfetados = dbCommand.ExecuteNonQuery();
            }

            return regsAfetados > 0;
        }
    }
}
