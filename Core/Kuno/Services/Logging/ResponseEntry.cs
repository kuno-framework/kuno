/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Collections.Generic;
using System.Reflection;
using Kuno.Configuration;
using Kuno.Services.Messaging;
using Kuno.Utilities.NewId;
using Kuno.Validation;

namespace Kuno.Services.Logging
{
    /// <summary>
    /// A serializable record of a response.
    /// </summary>
    /// <remarks>
    /// The entry is intended to be created on the same process and thread as the executing
    /// endpoint.  It can then be passed and/or stored as needed.
    /// </remarks>
    public class ResponseEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseEntry" /> class.
        /// </summary>
        public ResponseEntry()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseEntry" /> class.
        /// </summary>
        /// <param name="context">The completed context.</param>
        /// <param name="environment">The current environment.</param>
        public ResponseEntry(ExecutionContext context, ApplicationInformation environment)
        {
            this.CorrelationId = context.Request.CorrelationId;
            this.RequestId = context.Request.Message.Id;
            this.Completed = context.Completed;
            this.Function = context.Function.FunctionType.AssemblyQualifiedName;
            this.Exception = context.Exception?.ToString();
            this.IsSuccessful = context.IsSuccessful;
            this.Started = context.Started;
            this.ValidationErrors = context.ValidationErrors;
            this.TimeStamp = DateTimeOffset.Now;
            this.MachineName = Environment.MachineName;
            this.ApplicationName = environment.Title;
            this.Path = context.Request.Path;
            this.Version = environment.Version;
            this.Channel = context.Request.Channel;
            this.Build = Assembly.GetEntryAssembly()?.GetName()?.Version.ToString();
            if (this.Completed.HasValue)
            {
                this.Elapsed = this.Completed.Value - this.Started;
            }
            this.EnvironmentName = environment.Environment;
        }

        /// <summary>
        /// Gets or sets the channel.
        /// </summary>
        /// <value>
        /// The channel.
        /// </value>
        public string Channel { get; set; }

        /// <summary>
        /// Gets or sets application name where the endpoint executed.
        /// </summary>
        /// <value>The application name where the endpoint executed.</value>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public string Build { get; set; }

        /// <summary>
        /// Gets the execution completion date and time.
        /// </summary>
        /// <value>The execution completion date and time.</value>
        public DateTimeOffset? Completed { get; set; }

        /// <summary>
        /// Gets or sets the request correlation identifier.
        /// </summary>
        /// <value>The request correlation identifier.</value>
        public string CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the execution time elapsed from start to end.
        /// </summary>
        /// <value>The execution time elapsed from start to end.</value>
        public TimeSpan Elapsed { get; set; }

        /// <summary>
        /// Gets the type of the function.
        /// </summary>
        /// <value>The type of the function.</value>
        public string Function { get; set; }

        /// <summary>
        /// Gets or sets the name of the environment.
        /// </summary>
        /// <value>
        /// The name of the environment.
        /// </value>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// Gets the exception that was raised, if any.
        /// </summary>
        /// <value>The exception that was raised, if any.</value>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; set; } = NewId.NextId();

        /// <summary>
        /// Gets a value indicating whether execution was successful.
        /// </summary>
        /// <value><c>true</c> if execution was; otherwise, <c>false</c>.</value>
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the name of the machine that executed the endpoint.
        /// </summary>
        /// <value>The name of the machine that executed the endpoint.</value>
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the path that the endpoint was listening on, if any.
        /// </summary>
        /// <value>The path that the endpoint was listening on.</value>
        public string Path { get; set; }

        /// <summary>
        /// Gets the request message identifier.
        /// </summary>
        /// <value>The request message identifier.</value>
        public string RequestId { get; set; }

        /// <summary>
        /// Gets the date and time that the request was received.
        /// </summary>
        /// <value>The date and time that the request was received.</value>
        public DateTimeOffset Started { get; set; }

        /// <summary>
        /// Gets or sets the date and time when this entry was created.
        /// </summary>
        /// <value>The date and time when this entry was created.</value>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets the validation errors that were raised, if any.
        /// </summary>
        /// <value>The validation errors that were raised.</value>
        public IEnumerable<ValidationError> ValidationErrors { get; set; }

        /// <summary>
        /// Gets or sets the version number.
        /// </summary>
        /// <value>
        /// The version number.
        /// </value>
        public string Version { get; set; }
    }
}