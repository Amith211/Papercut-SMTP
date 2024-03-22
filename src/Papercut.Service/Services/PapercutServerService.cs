﻿// Papercut
// 
// Copyright © 2008 - 2012 Ken Robertson
// Copyright © 2013 - 2021 Jaben Cargman
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


namespace Papercut.Service.Services
{
    using Papercut.Core.Domain.WebServer;
    using Papercut.Infrastructure.IPComm.Network;
    using Papercut.Infrastructure.Smtp;

    using Serilog;

    public class PapercutServerService
    {
        readonly IAppMeta _applicationMetaData;

        readonly PapercutIPCommServer _ipCommServer;

        private readonly PapercutSmtpServer _smtpServer;

        readonly ILogger _logger;

        readonly IMessageBus _messageBus;

        private readonly PapercutIPCommEndpoints _papercutIpCommEndpoints;

        private readonly IPapercutWebServer _papercutWebServer;

        public PapercutServerService(
            PapercutIPCommServer ipCommServer,
            PapercutSmtpServer smtpServer,
            PapercutIPCommEndpoints papercutIpCommEndpoints,
            IPapercutWebServer papercutWebServer,
            IAppMeta applicationMetaData,
            ILogger logger,
            IMessageBus messageBus)
        {
            this._papercutIpCommEndpoints = papercutIpCommEndpoints;
            this._papercutWebServer = papercutWebServer;
            this._applicationMetaData = applicationMetaData;
            this._logger = logger;
            this._messageBus = messageBus;
            this._ipCommServer = ipCommServer;
            this._smtpServer = smtpServer;
        }

        public async Task Start()
        {
            await this._messageBus.PublishAsync(
                new PapercutServicePreStartEvent { AppMeta = this._applicationMetaData });

            try
            {
                await this._ipCommServer.StopAsync();
                await this._ipCommServer.StartAsync(this._papercutIpCommEndpoints.Service);
            }
            catch (Exception ex)
            {
                this._logger.Warning(
                    ex,
                    "Unable to Create Papercut IPComm Server Listener on {IP}:{Port}. After 5 Retries. Failing",
                    this._ipCommServer.ListenIpAddress,
                    this._ipCommServer.ListenPort);
            }

            await this._papercutWebServer.StartAsync();

            await this._messageBus.PublishAsync(
                new PapercutServiceReadyEvent { AppMeta = this._applicationMetaData });
        }

        public async Task Stop()
        {
            await Task.WhenAll(this._smtpServer.StopAsync(), this._ipCommServer.StopAsync());

            await this._messageBus.PublishAsync(new PapercutServiceExitEvent { AppMeta = this._applicationMetaData });
        }
    }
}