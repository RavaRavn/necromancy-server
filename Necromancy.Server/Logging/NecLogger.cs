using System;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Logging
{
    public class NecLogger : Logger
    {
        private NecSetting _setting;

        public NecLogger() : this(null)
        {
        }

        public NecLogger(string identity, string zone = null) : base(identity, zone)
        {
        }

        public override void Initialize(string identity, string zone, object configuration)
        {
            base.Initialize(identity, zone, configuration);
            _setting = configuration as NecSetting;
            if (_setting == null)
            {
                Error("Couldn't apply NecLogger configuration");
            }
        }

        public void Info(NecClient client, string message, params object[] args)
        {
            Write(LogLevel.Info, null, $"{client.Identity} {message}", args);
        }

        public void Info(NecConnection connection, string message, params object[] args)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                Info(client, message, args);
                return;
            }

            Write(LogLevel.Info, null, $"{connection.Identity} {message}", args);
        }

        public void Debug(NecClient client, string message, params object[] args)
        {
            Write(LogLevel.Debug, null, $"{client.Identity} {message}", args);
        }

        public void Error(NecClient client, string message, params object[] args)
        {
            Write(LogLevel.Error, null, $"{client.Identity} {message}", args);
        }

        public void Error(NecConnection connection, string message, params object[] args)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                Error(client, message, args);
                return;
            }

            Write(LogLevel.Error, null, $"{connection.Identity} {message}", args);
        }

        public void Exception(NecClient client, Exception exception)
        {
            Write(LogLevel.Error, null, $"{client.Identity} {exception}");
        }

        public void Exception(NecConnection connection, Exception exception)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                Exception(client, exception);
                return;
            }

            Write(LogLevel.Error, null, $"{connection.Identity} {exception}");
        }

        public void Info(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Info, null, $"[{socket.Identity}] {message}", args);
        }

        public void Debug(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Debug, null, $"[{socket.Identity}] {message}", args);
        }

        public void Error(ITcpSocket socket, string message, params object[] args)
        {
            Write(LogLevel.Error, null, $"[{socket.Identity}] {message}", args);
        }

        public void Exception(ITcpSocket socket, Exception exception)
        {
            Write(LogLevel.Error, null, $"[{socket.Identity}] {exception}");
        }

        public void LogIncomingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.LogIncomingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client.Identity, packet, NecLogType.PacketIn, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogIncomingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                LogIncomingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket = new NecLogPacket(connection.Identity, packet, NecLogType.PacketIn, serverType);
            WritePacket(logPacket);
        }

        public void LogUnknownIncomingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.LogUnknownIncomingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client.Identity, packet, NecLogType.PacketUnhandled, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogUnknownIncomingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                LogUnknownIncomingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket = new NecLogPacket(connection.Identity, packet, NecLogType.PacketUnhandled, serverType);
            WritePacket(logPacket);
        }

        public void LogOutgoingPacket(NecClient client, NecPacket packet, ServerType serverType)
        {
            if (_setting.LogOutgoingPackets)
            {
                NecLogPacket logPacket = new NecLogPacket(client.Identity, packet, NecLogType.PacketOut, serverType);
                WritePacket(logPacket);
            }
        }

        public void LogOutgoingPacket(NecConnection connection, NecPacket packet, ServerType serverType)
        {
            NecClient client = connection.Client;
            if (client != null)
            {
                LogOutgoingPacket(client, packet, serverType);
                return;
            }

            if (!_setting.LogIncomingPackets)
            {
                return;
            }

            NecLogPacket logPacket = new NecLogPacket(connection.Identity, packet, NecLogType.PacketOut, serverType);
            WritePacket(logPacket);
        }

        private void WritePacket(NecLogPacket packet)
        {
            Write(LogLevel.Info, packet, packet.ToLogText());
        }
    }
}
