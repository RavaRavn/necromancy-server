using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_search : ClientHandler
    {
        public send_auction_search(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_auction_search;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
	
            res.WriteInt32(0x64); // cmp to 0x64 = 100
	
            int numEntries4 = 0x64;
            for (int i = 0; i < numEntries4; i++)
            {
                res.WriteInt32(0);
                res.WriteInt64(2);
                res.WriteInt32(3); // Lowest
                res.WriteInt32(4); // Buy Now
                res.WriteFixedString($"{client.Soul.Name}", 49); // Soul Name Of Sellers
                res.WriteByte(0);
                res.WriteFixedString("Viagra for your sword", 385); // Comment section
                res.WriteInt16(5); // Bid
                res.WriteInt32(999999); // Item remaining time
            }
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_search_r, res, ServerType.Area);
        }

    }
}