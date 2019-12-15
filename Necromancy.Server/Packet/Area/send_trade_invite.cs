using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_invite : ClientHandler
    {
        public send_trade_invite(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int myTargetID = packet.Data.ReadInt32();
            
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  // error check.  1 auto cancels the trade,  0 "the trade has been presented to %d. Awaiting response"
            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_invite_r, res, ServerType.Area);
            SendTradeInviteNotify(client);
        }
        
        private void SendTradeInviteNotify(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            
            res.WriteInt32(client.Character.Id);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_notify_invited, res, ServerType.Area, client);

        }

       
        
    }
}