/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using Akka.Actor;
using Akka.DI.Core;
using Akka.Routing;
using Kuno.Akka.EndPoints;
using Kuno.Services.Messaging;

namespace Kuno.Akka.Messaging
{
    public class ScheduleRunner : UntypedActor
    {
        private readonly IMessageGateway _messages;

        public ScheduleRunner(IMessageGateway messages)
        {
            _messages = messages;
        }

        protected override void OnReceive(object message)
        {
            _messages.Send(message);
        }
    }

    public class ServicesCoordinator : ReceiveActor
    {
        protected override void PreStart()
        {
            base.PreStart();

            Context.ActorOf(Context.DI().Props<ScheduleRunner>().WithRouter(new RoundRobinPool(15)), "schedule");
        }
    }
}