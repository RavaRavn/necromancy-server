using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_abort : ClientHandler
    {
        public send_trade_abort(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_abort_r, res, ServerType.Area);
            recvTradeNotifyAbort(client);
        }

        private void recvTradeNotifyAbort(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            
            res.WriteInt32(0);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_notify_aborted, res, ServerType.Area, client);

        }

    }
}