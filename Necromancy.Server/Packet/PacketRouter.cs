using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Chat;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Packet
{
    public class PacketRouter
    {
        private readonly NecLogger _logger;

        public PacketRouter()
        {
            _logger = LogProvider.Logger<NecLogger>(this);
        }

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(NecClient client, NecPacket packet)
        {
            client.Send(packet);
        }

        /// <summary>
        /// Send a packet to a connection.
        /// </summary>
        public void Send(NecConnection connection, NecPacket packet)
        {
            connection.Send(packet);
        }

        /// <summary>
        /// Send a packet to a client.
        /// </summary>
        public void Send(NecClient client, ushort id, IBuffer data, ServerType serverType)
        {
            NecPacket packet = new NecPacket(id, data, serverType);
            Send(client, packet);
        }

        /// <summary>
        /// Send a packet to a connection.
        /// </summary>
        public void Send(NecConnection connection, ushort id, IBuffer data)
        {
            NecPacket packet = new NecPacket(id, data, connection.ServerType);
            Send(connection, packet);
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<NecClient> clients, NecPacket packet, params NecClient[] excepts)
        {
            clients = GetClients(clients, excepts);
            foreach (NecClient client in clients)
            {
                Send(client, packet);
            }
        }

        /// <summary>
        /// Send a packet to multiple clients.
        /// </summary>
        /// <param name="excepts">clients to exclude</param>
        public void Send(List<NecClient> clients, ushort id, IBuffer data, ServerType serverType,
            params NecClient[] excepts)
        {
            Send(clients, new NecPacket(id, data, serverType), excepts);
        }


        /// <summary>
        /// Send a packet to everyone in the map.
        /// </summary>
        public void Send(Map map, ushort id, IBuffer data, ServerType serverType, params NecClient[] excepts)
        {
            Send(map, new NecPacket(id, data, serverType), excepts);
        }

        /// <summary>
        /// Send a packet to everyone in the map.
        /// </summary>
        public void Send(Map map, NecPacket packet, params NecClient[] excepts)
        {
            List<NecClient> clients = GetClients(map.ClientLookup.GetAll(), excepts);
            foreach (NecClient client in clients)
            {
                Send(client, packet);
            }
        }

        /// <summary>
        /// Send a specific packet response.
        /// </summary>
        public void Send(PacketResponse response)
        {
            foreach (NecClient client in response.Clients)
            {
                Send(client, response.ToPacket());
            }
        }

        /// <summary>
        /// Send a specific packet response to a map.
        /// </summary>
        public void Send(Map map, PacketResponse response, params NecClient[] excepts)
        {
            List<NecClient> mapClients = map.ClientLookup.GetAll();
            mapClients.AddRange(response.Clients);
            response.CleatClients();
            List<NecClient> clients = GetClients(mapClients, excepts);
            response.AddClients(clients);
            Send(response);
        }

        /// <summary>
        /// Send a specific packet response.
        /// </summary>
        public void Send(PacketResponse response, params NecClient[] clients)
        {
            response.AddClients(clients);
            Send(response);
        }

        /// <summary>
        /// Send a chat message
        /// </summary>
        public void Send(ChatResponse response)
        {
            RecvChatNotifyMessage notifyMessage = new RecvChatNotifyMessage(response);
            notifyMessage.AddClients(response.Recipients);
            Send(notifyMessage);
        }

        private List<NecClient> GetClients(List<NecClient> clients, params NecClient[] excepts)
        {
            if (excepts.Length == 0)
            {
                return clients;
            }

            foreach (NecClient except in excepts)
            {
                clients.Remove(except);
            }

            return clients;
        }
    }
}
