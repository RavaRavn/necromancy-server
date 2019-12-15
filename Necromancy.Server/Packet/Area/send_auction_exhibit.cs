using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_exhibit : ClientHandler
    {
        public send_auction_exhibit(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_auction_exhibit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt64(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_exhibit_r, res, ServerType.Area);
        }

    }
}