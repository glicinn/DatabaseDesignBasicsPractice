using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace DB_MIA
{
    class sqlConnectionClass
    {
        public SqlConnection SQLConnect(string datasource, string datatable)
        {
            SqlConnection connection = new SqlConnection(string.Format(@"Data Source = {0}; Initial Catalog = {1}; Integrated Security = true; ", datasource, datatable));
            return connection;
        }
        public SqlDependency dependency = new SqlDependency();


        public bool Connection(SqlConnection connection)
        {
            try
            {
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                connection.Close();
            }
        }

        public enum act { select, update, delete };

        public DataTable SQLCommand(string command, act act, string[] values)
        {
            DataTable table = new DataTable();
            SqlConnection sqlconn = SQLConnect(Data.Server, Data.Database);
            SqlCommand commandSql = new SqlCommand();
            commandSql.Connection = sqlconn;
            commandSql.CommandText = $"{command}";
            commandSql.Notification = null;
            switch (act)
            {
                case act.select:
                    dependency.AddCommandDependency(commandSql);
                    SqlDependency.Start(sqlconn.ConnectionString);
                    sqlconn.Open();
                    table.Load(commandSql.ExecuteReader());
                    sqlconn.Close();
                    return table;
                case act.update:
                    sqlconn.Open();
                    try
                    {
                    commandSql.ExecuteNonQuery();
                    }
                    catch { MessageBox.Show("Ошибка введенных данных", "ERROR");  }
                    sqlconn.Close();
                    break;
                case act.delete:
                    sqlconn.Open();
                    try
                    {
                        commandSql.ExecuteNonQuery();
                    }
                    catch { MessageBox.Show("Невозможно удалить эти данные.", "DELETE ERROR"); }
                    sqlconn.Close();
                    break;
            }
            return null;
        }
    }
}
