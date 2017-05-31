/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Text;

namespace Kuno.Logging
{
    /// <summary>
    /// Provides a simple console logger that should only be used in development.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        private static readonly string Separater = new string('-', 100);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <inheritdoc />
        public void Debug(Exception exception, string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "DEBUG", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            if (exception != null)
            {
                builder.AppendLine("- " + exception);
            }
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Debug(string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "DEBUG", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Error(Exception exception, string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "ERROR", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            if (exception != null)
            {
                builder.AppendLine("- " + exception);
            }
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Error(string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "ERROR", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Fatal(Exception exception, string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "FATAL", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            if (exception != null)
            {
                builder.AppendLine("- " + exception);
            }
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Fatal(string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "FATAL", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Information(Exception exception, string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "INFO", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            if (exception != null)
            {
                builder.AppendLine("- " + exception);
            }
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Information(string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "INFO", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Verbose(Exception exception, string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "VERBOSE", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            if (exception != null)
            {
                builder.AppendLine("- " + exception);
            }
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Verbose(string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "VERBOSE", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Warning(Exception exception, string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "WARN", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            if (exception != null)
            {
                builder.AppendLine("- " + exception);
            }
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }

        /// <inheritdoc />
        public void Warning(string template, params object[] properties)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("[{0}][{1}][Thread {2:0000}]", "WARN", DateTime.UtcNow, Environment.CurrentManagedThreadId);
            builder.AppendLine(template);
            builder.AppendLine(Separater);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(builder.ToString());
            Console.ResetColor();
        }
    }
}