using System;
using System.IO;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public abstract class BaseDAL
    {
        private static readonly string DbFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dados", "DesafioDB.db");

        protected static readonly string CONNECTION_STRING =
            $@"Data Source={DbFile};Version=3;";
    }
}
