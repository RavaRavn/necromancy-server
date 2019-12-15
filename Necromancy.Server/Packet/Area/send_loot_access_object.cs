using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_loot_access_object : ClientHandler
    {
        public send_loot_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_loot_access_object;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectID = packet.Data.ReadInt32();
            //int first = packet.Data.ReadInt16();
            //int second = packet.Data.ReadInt16();
            //Console.WriteLine($"First Int 16 is : {first}");
            //Console.WriteLine($"second Int 16 is : {second}");
            Console.WriteLine($"first Int 32 is : {objectID}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(objectID);

            Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
        }
    }
}