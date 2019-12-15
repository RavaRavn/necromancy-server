using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_reply : ClientHandler
    {
        public send_trade_reply(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_reply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); 
            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_reply_r, res, ServerType.Area);
            SendTradeRepliedNotify(client);
        }
        
        private void SendTradeRepliedNotify(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            
            res.WriteInt32(client.Character.Id);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_notify_replied, res, ServerType.Area, client);

        }

       
        
    }
}