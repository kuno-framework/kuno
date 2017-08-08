/* 
 * Copyright (c) Kuno Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

using System.Linq;
using Akka.Actor;
using Autofac;
using Kuno.Services;

namespace Kuno.Akka.EndPoints
{
    [EndPoint("_system/akka")]
    public class GetAkkaStatus : Function
    {
        private readonly IComponentContext _components;

        public GetAkkaStatus(IComponentContext components)
        {
            _components = components;
        }

        public override void Receive()
        {
            this.Respond(_components.ResolveAll<ActorSystem>()
                .Select(e => new
                {
                    e.Name,
                    e.StartTime,
                    e.Uptime
                }));
        }
    }
}