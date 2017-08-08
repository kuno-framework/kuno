using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Kuno.AzureServiceBus.Components;
using Kuno.AzureServiceBus.Settings;

namespace Kuno.AzureServiceBus.Modules
{
    public class TopicSubscriptionModule : Module
    {
        private readonly AzureServiceBusSettings _settings;

        public TopicSubscriptionModule(AzureServiceBusSettings settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TopicSubscription>()
                   .WithParameter(new TypedParameter(typeof(AzureServiceBusSettings), _settings))
                   .AsSelf().AsImplementedInterfaces()
                   .SingleInstance()
                   .AutoActivate();
        }
    }
}
