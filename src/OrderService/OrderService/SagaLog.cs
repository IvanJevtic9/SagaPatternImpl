using System;
using System.Collections.Generic;

namespace OrderService
{
    public enum LogType {
        Start,
        End
    }

    public class SagaLog
    {
        // ServiceName (Method) - Type (Start or End) - request / response
        public List<Tuple<string, LogType, string>> Logs { get; set; } = new List<Tuple<string, LogType, string>>();
    }
}
