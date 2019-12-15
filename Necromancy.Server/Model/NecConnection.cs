using System;
using System.Collections.Generic;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Networking.Tcp;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    public class NecConnection
    {
        private readonly NecLogger _logger;

        public NecConnection(ITcpSocket clientSocket, PacketFactory packetFactory, ServerType serverType)
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            Socket = clientSocket;
            PacketFactory = packetFactory;
            ServerType = serverType;
            Client = null;
        }

        public string Identity => Socket.Identity;
        public ServerType ServerType { get; }
        public ITcpSocket Socket { get; }
        public PacketFactory PacketFactory { get; }
        public NecClient Client { get; set; }

        public List<NecPacket> Receive(byte[] data)
        {
            List<NecPacket> packets;
            try
            {
                packets = PacketFactory.Read(data, ServerType);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                packets = new List<NecPacket>();
            }

            return packets;
        }

        public void Send(NecPacket packet)
        {
            byte[] data;
            try
            {
                data = PacketFactory.Write(packet);
            }
            catch (Exception ex)
            {
                _logger.Exception(this, ex);
                return;
            }

            _logger.LogOutgoingPacket(this, packet, ServerType);
            Socket.Send(data);
        }
    }
}
