// Papercut
// 
// Copyright � 2008 - 2012 Ken Robertson
// Copyright � 2013 - 2021 Jaben Cargman
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.


namespace Papercut.Core.Infrastructure.MessageBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Autofac;

    using Papercut.Common.Domain;
    using Papercut.Common.Extensions;

    using Serilog;

    public class AutofacMessageBus : IMessageBus
    {
        readonly ILifetimeScope _lifetimeScope;

        public AutofacMessageBus(ILifetimeScope lifetimeScope)
        {
            this._lifetimeScope = lifetimeScope;
        }

        public virtual async Task PublishAsync<T>(T eventObject, CancellationToken token) where T : IEvent
        {
            foreach (var @event in this._lifetimeScope.Resolve<IEnumerable<IEventHandler<T>>>().MaybeByOrderable())
            {
                try
                {
                    await this.ExecuteEvent(eventObject, @event, token);
                }
                catch (Exception ex)
                {
                    this._lifetimeScope.Resolve<ILogger>().ForContext<AutofacMessageBus>().Error(
                        ex,
                        "Failed publishing {EventType} to {EventHandler}",
                        typeof(T),
                        @event.GetType());
                }
            }
        }

        protected virtual async Task ExecuteEvent<T>(T eventObject, IEventHandler<T> @event, CancellationToken token)
            where T : IEvent
        {
            await @event.HandleAsync(eventObject, token);
        }
    }
}