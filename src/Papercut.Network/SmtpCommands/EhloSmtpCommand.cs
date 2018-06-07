﻿namespace Papercut.Network.SmtpCommands
{
    using System.Collections.Generic;
    using System.Linq;

    using Papercut.Core.Infrastructure.Network;
    using Papercut.Network.Protocols;

    public class EhloSmtpCommand : BaseSmtpCommand
    {
        protected override bool RequiresAuthentication { get { return false; } }
    
        protected override IEnumerable<string> GetMatchCommands()
        {
            return new[] { "EHLO" };
        }

        protected override void Run(string command, string[] args)
        {
            Session.Sender = args.FirstOrDefault() ?? string.Empty;

            Connection.SendLine("250-{0}", NetworkHelper.GetLocalDnsHostName());
            Connection.SendLine("250-SMTPUTF8");
            Connection.SendLine("250-8BITMIME");

            // NOTE: if implementing TLS/STARTTLS, we may wish to hide this capability until a TLS
            // session is established.  I expect this would be along the lines of:
            //
            // if (Connection.RequiresTls && Connection.IsSecure) { ... }            
            Connection.SendLine("250-AUTH LOGIN PLAIN");
            
            Connection.SendLine("250 OK");
        }
    }
}
