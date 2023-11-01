using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentUpload
{
    class HC_DBConnection
    {
        //ApplicationName = "HireCraft";
        //    ConnectionTimeout = 180;
       
        public static SqlConnection GetsqlConnection()
        {
            SqlConnection con = new SqlConnection(getConnectionString);

            con.Open();
            return con;
        }


        public static string ParserUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ParserUrl"];
            }
        }


        public static string ApplicationName { get; set; }
        public static int ConnectionTimeout { get; set; }

        public static string getConnectionString
        {

            get
            {

                try
                {
                    ApplicationName = "HireCraft";
                    ConnectionTimeout = 180;
                    // connection pool size problem
                    //string sqlConnection = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    string sqlConnection = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

                    SqlConnectionStringBuilder sqlConnectionBuilder = new SqlConnectionStringBuilder(sqlConnection);
                    sqlConnectionBuilder.ApplicationName = ApplicationName ?? sqlConnectionBuilder.ApplicationName;
                    sqlConnectionBuilder.ConnectTimeout = ConnectionTimeout > 0 ? ConnectionTimeout : sqlConnectionBuilder.ConnectTimeout;
                    sqlConnectionBuilder.MaxPoolSize = 500;

                    return sqlConnectionBuilder.ToString();
                    

                }
                catch (Exception ex)
                {
                    //beErrorLog.opWriteErrorLog(HCErrorCodes.DBConnection, getWebConfig, ex.Message, ex.StackTrace, beErrorLog.LogLevel.Error);
                    return "";
                }


            }

        }


    }
}
