using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rongbo.Common.Utilities
{
    public static class Log
    {
        private static ILoggerFactory _loggerFactory;

        public static void SetLoggerFactory(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public static ILoggerFactory GetLoggerFactory()
        {
            return _loggerFactory;
        }

        public static void AddProvider(ILoggerProvider loggerProvider)
        {
            _loggerFactory.AddProvider(loggerProvider);
        }

        public static ILogger Create(string categoryName)
        {
            return _loggerFactory.CreateLogger(categoryName);
        }

        public static ILogger<TCategoryName> Create<TCategoryName>()
        {
            return _loggerFactory.CreateLogger<TCategoryName>();
        }
    }
}
