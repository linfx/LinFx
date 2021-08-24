using Microsoft.Extensions.DependencyInjection;
using System;

namespace LinFx.Extensions.EventBus
{
    internal class IocEventHandlerFactory : IEventHandler
    {
        private IServiceScopeFactory serviceScopeFactory;
        private Type handler;

        public IocEventHandlerFactory(IServiceScopeFactory serviceScopeFactory, Type handler)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.handler = handler;
        }
    }
}