using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public abstract class PacketResponse
    {
        private readonly List<NecClient> _clients;
        private NecPacket _packet;

        public PacketResponse(ushort id, ServerType serverType)
        {
            _clients = new List<NecClient>();
            Id = id;
            ServerType = serverType;
        }

        public List<NecClient> Clients => new List<NecClient>(_clients);
        public ServerType ServerType { get; }
        public ushort Id { get; }

        protected abstract IBuffer ToBuffer();

        public NecPacket ToPacket()
        {
            if (_packet == null)
            {
                _packet = new NecPacket(Id, ToBuffer(), ServerType);
            }

            return _packet;
        }

        public void AddClients(params NecClient[] clients)
        {
            _clients.AddRange(clients);
        }

        public void AddClients(IEnumerable<NecClient> clients)
        {
            _clients.AddRange(clients);
        }

        public void CleatClients()
        {
            _clients.Clear();
        }
    }
}
