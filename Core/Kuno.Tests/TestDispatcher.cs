using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Kuno.Services.Registry;
using Kuno.Services.Messaging;
using ExecutionContext = Kuno.Services.Messaging.ExecutionContext;

namespace Kuno.Tests
{
    /// <summary>
    /// The test dispatcher allows tests to intercept calls to the messaging system.
    /// </summary>
    public class TestDispatcher : RequestRouter, IRemoteRouter
    {
        private readonly Dictionary<string, Func<object, Request, object>> _endPoints = new Dictionary<string, Func<object, Request, object>>();

        /// <summary>
        /// Uses the specified action when the endpoint is called.
        /// </summary>
        /// <typeparam name="T">The endpoint request type.</typeparam>
        /// <param name="action">The action to run.</param>
        public void UseEndPoint<T>(Action<T, Request> action)
        {
            _endPoints.Add(typeof(T).FullName, (a, b) =>
            {
                action((T)a, b);
                return null;
            });
        }

        public void UseEndPoint<T>(Func<T, Request, object> action)
        {
            _endPoints.Add(typeof(T).FullName, (a, b) => action((T)a, b));
        }

        public TestDispatcher(IComponentContext components) : base(components)
        {
        }

        public override Task<MessageResult> Route(Request request, FunctionInfo endPoint, ExecutionContext parentContext, TimeSpan? timeout = null)
        {
            if (request.Message.MessageType != null && _endPoints.ContainsKey(request.Message.MessageType))
            {
                var context = new ExecutionContext(request, endPoint, CancellationToken.None, parentContext);
                context.Response = _endPoints[request.Message.MessageType](request.Message.Body, request);
                return Task.FromResult(new MessageResult(context));
            }

            return base.Route(request, endPoint, parentContext, timeout);
        }

        public bool CanRoute(Request request)
        {
            return true;
        }

        public Task<MessageResult> Route(Request request, ExecutionContext parentContext, TimeSpan? timeout = null)
        {
            if (request.Message.MessageType != null && _endPoints.ContainsKey(request.Message.MessageType))
            {
                var context = new ExecutionContext(request, parentContext);
                context.Response = _endPoints[request.Message.MessageType](request.Message.Body, request);
                return Task.FromResult(new MessageResult(context));
            }

            return Task.FromResult(new MessageResult(parentContext));
        }
    }
}
