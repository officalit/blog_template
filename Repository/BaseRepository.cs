using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace BLOG_CORE.Repository
{
    public class BaseRepository
    {
            protected T QueryFirstOrDefault<T>(string sql, object parameters = null)
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    return connection.QueryFirstOrDefault<T>(sql, parameters);
                }
            }

            protected List<T> Query<T>(string sql, object parameters = null)
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    return connection.Query<T>(sql, parameters).ToList();
                }
            }

            protected int Execute(string sql, object parameters = null)
            {
                using (var connection = CreateConnection())
                {
                    connection.Open();
                    return connection.Execute(sql, parameters);
                }
            }

            // Other Helpers...

            private IDbConnection CreateConnection()
            {
                var connection = new SQLiteConnection("Data Source = Data/IBD.db; Version = 3");
                // Properly initialize your connection here.
                return connection;
            }
    }
}
