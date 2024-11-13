using System;
using Oracle.ManagedDataAccess.Client;

namespace DAL
{
    public class DatabaseConnection
    {
        private static string connectionString = "User Id=proyectoDeluxe;Password=12345;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xepdb1)))";

        public static OracleConnection GetConnection()
        {
            OracleConnection connection = new OracleConnection(connectionString);
            return connection;
        }
    }
}
