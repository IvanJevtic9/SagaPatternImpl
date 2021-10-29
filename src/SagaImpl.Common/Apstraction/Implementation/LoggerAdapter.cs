using SagaImpl.Common.Abstraction.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace SagaImpl.Common.Apstraction.Implementation
{
    public class LoggerAdapter<T> : ILoggerAdapter<T>
    {
        private readonly ILogger<T> logger;

        public LoggerAdapter()
        { }

        public LoggerAdapter(ILogger<T> logger)
        {
            this.logger = logger;
        }

        private string GetMessageFormat(string message)
        {
            var st = new StackTrace();

            return $"[{typeof(T).FullName}.{st.GetFrame(2).GetMethod().Name}] {message}";
        }

        public void LogError(Exception ex, string message, params object[] args)
        {
            logger.LogError(ex, GetMessageFormat(message), args);
        }

        public void LogInformation(string message, params object[] args)
        {
            logger.LogInformation(GetMessageFormat(message), args);
        }

        public void LogWarning(string message, params object[] args)
        {
            logger.LogWarning(GetMessageFormat(message), args);
        }
    }
}
