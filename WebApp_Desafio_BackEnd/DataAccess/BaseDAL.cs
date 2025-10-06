using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    //public abstract class BaseDAL
    //{
    //    //protected static string CONNECTION_STRING = $"Data Source=\"{AppDomain.CurrentDomain.BaseDirectory}Dados\\DesafioDB.db\";Version=3;";
    //    //protected static string CONNECTION_STRING = @"Data Source=..\..\..\WebApp_Desafio_BackEnd\Dados\DesafioDB.db;Version=3;";
    //    //protected static string CONNECTION_STRING = $@"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\Dados\DesafioDB.db")};Version=3;";
    //    protected static string CONNECTION_STRING = @"Data Source=Dados\DesafioDB.db;Version=3;";
    //}

    public abstract class BaseDAL
    {
        private static readonly string DbFile =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dados", "DesafioDB.db");

        protected static readonly string CONNECTION_STRING =
            $@"Data Source={DbFile};Version=3;";
    }
}
