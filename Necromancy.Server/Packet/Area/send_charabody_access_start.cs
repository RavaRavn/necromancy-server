using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_access_start : ClientHandler
    {
        public send_charabody_access_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_access_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(2);

            Router.Send(client, (ushort) AreaPacketId.recv_charabody_access_start_r, res, ServerType.Area);            
        }
    }
}