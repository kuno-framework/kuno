/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System;
using System.Linq;
using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using Autofac;
using Kuno.Reflection;
using Kuno.Services.Messaging;
using Kuno.Validation;

namespace Kuno.Akka.Messaging
{
    /// <summary>
    /// A default Akka.NET supervisor.
    /// </summary>
    /// <seealso cref="ReceiveActor" />
    public class CommandCoordinator : ReceiveActor
    {
        private readonly IComponentContext _components;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCoordinator" /> class.
        /// </summary>
        /// <param name="components">The configured <see cref="IComponentContext" />.</param>
        public CommandCoordinator(IComponentContext components)
        {
            Argument.NotNull(components, nameof(components));

            _components = components;

            this.Receive<ExecutionContext>(e => this.Execute(e));
        }

        /// <summary>
        /// Gets the current path.
        /// </summary>
        /// <value>The current path.</value>
        protected string Path => this.Self.Path.ToString().Substring(this.Self.Path.ToString().IndexOf("user/commands", StringComparison.Ordinal) + 13).Trim('/');

        /// <summary>
        /// Executes the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns><c>true</c> if the request was successful, <c>false</c> otherwise.</returns>
        protected virtual bool Execute(ExecutionContext request)
        {
            var types = _components.Resolve<IDiscoverTypes>();
            var endPoint = request.Function;

            var name = "";
            if (string.IsNullOrWhiteSpace(name))
            {
                name = endPoint.FunctionType.Name.Split(' ')[0].Replace(".", "_");
            }
            if (name.Split('/').Length > 1)
            {
                var parent = name.Split('/')[0].Trim('/');
                if (Context.Child(parent).Equals(ActorRefs.Nobody))
                {
                    var full = (this.Path + "/" + parent.Split('/').Last()).Trim('/');

                    var firstOrDefault = types.Find<CommandCoordinator>().FirstOrDefault(e => e.GetAllAttributes<EndPointHostAttribute>().Any(x => x.Paths.Contains(full)));
                    var target = firstOrDefault
                                 ?? typeof(CommandCoordinator);

                    Context.ActorOf(Context.DI().Props(target), parent.Split('/').Last());
                }
                Context.Child(parent.Split('/').Last()).Forward(request);
            }
            else
            {
                if (Context.Child(name).Equals(ActorRefs.Nobody))
                {
                    var type = types.Find<ActorBase>().FirstOrDefault(e => e.GetAllAttributes<EndPointHostAttribute>().Any(x => x.Paths.Contains(this.Path + "/" + name)))
                               ?? typeof(EndPointHost);
                    var instance = request.Function.FunctionType.GetAllAttributes<RoutingAttribute>().FirstOrDefault()?.Instances;
                    if (instance.HasValue)
                    {
                        Context.ActorOf(Context.DI().Props(type).WithRouter(new RoundRobinPool(instance.Value)), name);
                    }
                    else
                    {
                        Context.ActorOf(Context.DI().Props(type), name);
                    }
                }
                Context.Child(name).Forward(request);
            }

            return true;
        }

        /// <inheritdoc />
        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(10, TimeSpan.FromSeconds(10),
                Decider.From(x =>
                {
                    var result = Directive.Restart;
                    return result;
                }));
        }
    }

    public class RoutingAttribute : Attribute
    {
        public int Instances { get; }

        public RoutingAttribute(int instances)
        {
            this.Instances = instances;
        }
    }
}