using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Threading.Tasks;

namespace CosmixERP.AccessLayer
{
    public class AccessDbContext
    {
        #region Constants

        //private const string CONN_STRING = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\temp\\DESS_Autopartes_DB.mdb;Jet OLEDB:Database Password=acs114;";
        private const string CONN_STRING = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\DESS_Refaccionaria_2\\Db\\DESS_Autopartes_DB.mdb;Jet OLEDB:Database Password=acs114;";

        #endregion

        #region Fields

        protected readonly IDbConnection connection;

        #endregion

        public AccessDbContext()
        {
            connection = new OleDbConnection(CONN_STRING);
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(string sql) 
        {
            var query = await connection.QueryAsync<T>(sql);

            return query;
        }
    }
}
