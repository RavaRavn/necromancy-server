using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_set_money : ClientHandler
    {
        public send_trade_set_money(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_set_money;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int myGoldOffer = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check.  must be 0 to succeed
            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_set_money_r, res, ServerType.Area);
        }

    }
}