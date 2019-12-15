using Necromancy.Server.Model;

namespace Necromancy.Server.Packet
{
    public abstract class ClientHandlerDeserializer<T> : ClientHandler
    {
        private readonly IPacketDeserializer<T> _deserializer;

        protected ClientHandlerDeserializer(NecServer server, IPacketDeserializer<T> deserializer) : base(server)
        {
            _deserializer = deserializer;
        }

        public override void Handle(NecClient client, NecPacket requestPacket)
        {
            T request = _deserializer.Deserialize(requestPacket);
            HandleRequest(client, request);
        }

        public abstract void HandleRequest(NecClient client, T request);
    }
}
