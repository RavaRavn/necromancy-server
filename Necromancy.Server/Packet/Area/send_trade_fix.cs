﻿using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_fix : Handler
    {
        public send_trade_fix(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_fix;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check?  to be tested
            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_fix_r, res);
            SendTradeNotifyFixed(client);
        }
        
        private void SendTradeNotifyFixed(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            
            res.WriteInt32(0);//error check? to be tested    Chat message "%d's trade completed

            Router.Send(client.Map, (ushort) AreaPacketId.recv_trade_notify_fixed, res, client);

        }

       
        
    }
}