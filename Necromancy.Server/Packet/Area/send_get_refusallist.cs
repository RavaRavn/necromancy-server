using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_get_refusallist : ClientHandler
    {
        public send_get_refusallist(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_get_refusallist;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);

            res.WriteInt32(2);

            res.WriteInt32(3);
            res.WriteFixedString("soul name", 49);

            res.WriteInt32(5);

            res.WriteInt32(3);
            res.WriteFixedString("soul name", 49);

            Router.Send(client, (ushort)AreaPacketId.recv_get_refusallist_r, res, ServerType.Area);
        }
    }
}