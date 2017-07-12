/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Reflection;
using System.Threading.Tasks;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Services.Pipeline
{
    /// <summary>
    /// The handle exception step of the endpoint execution pipeline.  This attempt to unwrap the exception if it can.
    /// </summary>
    /// <seealso cref="Kuno.Services.Pipeline.IMessageExecutionStep" />
    internal class HandleException : IMessageExecutionStep
    {
        /// <inheritdoc />
        public Task Execute(ExecutionContext context)
        {
            var exception = context.Exception;
            var validationException = exception as ValidationException;
            if (validationException != null)
            {
                context.AddValidationErrors(validationException.ValidationErrors);
                context.SetException(null);
            }
            else if (exception is AggregateException)
            {
                var innerException = exception.InnerException as ValidationException;
                if (innerException != null)
                {
                    context.AddValidationErrors(innerException.ValidationErrors);
                    context.SetException(null);
                }
                else if (exception.InnerException is TargetInvocationException)
                {
                    context.SetException(((TargetInvocationException) exception.InnerException).InnerException);
                }
                else if (((AggregateException) exception).InnerExceptions.Count == 1)
                {
                    context.SetException(exception.InnerException);
                }
            }
            else if (exception is TargetInvocationException)
            {
                context.SetException(exception.InnerException);
            }
            else
            {
                context.SetException(exception);
            }
            return Task.FromResult(0);
        }
    }
}