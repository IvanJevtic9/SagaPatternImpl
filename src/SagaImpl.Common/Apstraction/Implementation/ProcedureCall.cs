using AutoMapper;
using Dapper;
using Microsoft.Data.SqlClient;
using SagaImpl.Common.Abstraction.Interface;
using SagaImpl.Common.Apstraction.Interface;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace SagaImpl.Common.Apstraction.Implementation
{
    public class ProcedureCall : IProcedureCall
    {
        private readonly IDbContext db;
        private readonly string connectionString;

        public ProcedureCall(IDbContext db)
        {
            this.db = db;

            connectionString = db.GetConnectionString();
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task ExecuteProcedureAsync(string procedureName, object parameters = null)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                await sqlCon.ExecuteAsync(
                    sql: procedureName,
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                transaction.Complete();
            }
        }

        public async Task<List<IDictionary<string, object>>> ExecuteReaderProcedureAsync(string procedureName, object parameters = null)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                var res = await sqlCon.QueryAsync(
                    sql: procedureName,
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                transaction.Complete();

                // Has to be done like this because Dapper for some reason does not retrieve the object 
                // in the same way in case of QueryAsync and Query. Due to that, the cast cannot be done in an easier way
                // and has to be done element by element
                var resConverted = res.Select(x => (IDictionary<string, object>)x).ToList();

                return resConverted;
            }
        }

        public async Task<List<T>> ExecuteReaderProcedureAsync<T>(string procedureName, object parameters = null)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                var res = await sqlCon.QueryAsync<T>(
                    sql: procedureName,
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                transaction.Complete();

                return res.ToList();
            }
        }

        public async Task<List<T>> ExecuteReaderProcedureAsync<T>(string procedureName, IMapper mapper, object parameters = null)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            using (var sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                var res = await sqlCon.QueryAsync(
                    sql: procedureName,
                    param: parameters,
                    commandType: CommandType.StoredProcedure);

                transaction.Complete();

                return mapper.Map<List<T>>(res);
            }
        }
    }
}
