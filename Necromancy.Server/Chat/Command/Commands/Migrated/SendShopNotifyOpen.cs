using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens your shop
    public class SendShopNotifyOpen : ServerChatCommand
    {
        public SendShopNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res0 = BufferProvider.Provide();
            res0.WriteInt16(2);
            /* Shop ID, 0 it's forge, 1 it's cursed, 2 Purchase shop, 3 purchase and curse, 4 it's sell, 
                        5 sell and curse. 6 purchase and sell. 7 Purchase, Sell, Curse.
                        
                        8 Identify. 9 identify & curse. 10 Purchase & Identify.
                        
                        11 Purchase, Identify & Curse. 12 Sell And Identify
                        
                        13 Sell, Identify & Curse, 14 Purchase, Sell & Identify
                        
                        15 All of what i say before except the forge.
                        
                        16 Repair ! 17 repair and curse. 18 Repair and purchase;
                        
                        19 repair, purchase, cursed. 20 Repair and sell
           */
            res0.WriteInt32(0); // don't know
            res0.WriteInt32(0); // don't know too
            res0.WriteByte(0); // 0 = shop open, 1 = shop not open ?
            Router.Send(client, (ushort) AreaPacketId.recv_shop_notify_open, res0, ServerType.Area);


/*
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            res.WriteInt32(100101); // item id

            res.WriteInt64(1); // item price

            int numEntries = 0x10;
            for (int i = 0; i < numEntries; i++)  // loops 0x10 times assuming this is stats for weapon / armor
            {
                res.WriteByte(1); 
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);
                res.WriteByte(1);

            } 
           
            Router.Send(client, (ushort)AreaPacketId.recv_shop_notify_item, res, ServerType.Area); */


            IBuffer res1 = BufferProvider.Provide();
            res1.WriteCString("GnomeBoobs");
            Router.Send(client, (ushort) AreaPacketId.recv_shop_title_push, res1, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "shop";
    }
}
