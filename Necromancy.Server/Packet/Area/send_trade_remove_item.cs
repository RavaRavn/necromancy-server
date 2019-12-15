using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_remove_item : ClientHandler
    {
        public send_trade_remove_item(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_remove_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_remove_item_r, res, ServerType.Area);
        }

    }
}