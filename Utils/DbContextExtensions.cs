using System.Data.Common;
using System.Data;

namespace FurnitureERP.Utils
{
    public static class DbContextExtensions
    {
        public static DataTable SqlQueryTable(this DbContext context,
           string sqlQuery, params DbParameter[] parameters)
        {
            var table = new DataTable();
            var connection = context.Database.GetDbConnection();
            var dbFactory = DbProviderFactories.GetFactory(connection);
            using (var cmd = dbFactory.CreateCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sqlQuery;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(table);
                }
            }
            return table;
        }
    }
}
