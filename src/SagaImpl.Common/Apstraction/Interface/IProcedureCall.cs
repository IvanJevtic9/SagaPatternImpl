using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SagaImpl.Common.Abstraction.Interface
{
    public interface IProcedureCall : IDisposable
    {
        Task ExecuteProcedureAsync(string procedureName, object parameters = null);
        Task<List<IDictionary<string, object>>> ExecuteReaderProcedureAsync(string procedureName, object parameters = null);
        Task<List<T>> ExecuteReaderProcedureAsync<T>(string procedureName, object parameters = null);
        Task<List<T>> ExecuteReaderProcedureAsync<T>(string procedureName, IMapper mapper, object parameters = null);
    }
}
