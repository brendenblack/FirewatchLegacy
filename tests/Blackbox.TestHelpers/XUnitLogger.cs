using Microsoft.Extensions.Logging;
using System;
using Xunit;
using Xunit.Abstractions;

namespace Xunit
{
    /// <summary>
    /// Provides a concrete implementation of <see cref="ILogger&lt;T>"/> that allows capturing of log output
    /// by XUnit.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XUnitLogger<T> : ILogger<T>, IDisposable
    {
        private ITestOutputHelper _output;

        public XUnitLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _output.WriteLine(state.ToString());
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}