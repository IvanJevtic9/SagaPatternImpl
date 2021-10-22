using Microsoft.EntityFrameworkCore.Storage;
using System;

namespace SagaImpl.Common.Abstraction.Interface
{
    public interface IDataConnection : IDisposable
    {
        IDbContextTransaction BeginTransaction();
    }
}
