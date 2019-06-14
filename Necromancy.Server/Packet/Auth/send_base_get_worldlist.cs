using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Auth
{
    public class send_base_get_worldlist : Handler
    {
        public send_base_get_worldlist(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AuthPacketId.send_base_get_worldlist;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int numEntries = 4;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(numEntries);
            for (int i = 1; i <= numEntries; i++)
            {
                res.WriteInt32(i);
                res.WriteFixedString($"World {i}", 37);
                res.WriteInt32(0);
                res.WriteInt16(0); //Max Player
                res.WriteInt16(0); //Current Player
            }

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            Router.Send(client, (ushort) AuthPacketId.recv_base_get_worldlist_r, res);
        }
    }
}