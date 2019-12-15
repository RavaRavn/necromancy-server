using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_quest_get_mission_quest_works : ClientHandler
    {
        public send_quest_get_mission_quest_works(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_quest_get_mission_quest_works;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1);

            int numEntries = 2;
            res.WriteInt32(numEntries);//less than 0x1E

            for (int j = 0; j < numEntries; j++)
            {
                res.WriteFixedString("idk", 385);
                res.WriteInt64(1);
                res.WriteByte(1);
                res.WriteFixedString("idk_also", 385);

                for (int i = 0; i < 5; i++)
                {
                    res.WriteByte(1);
                    res.WriteInt32(1);
                    res.WriteInt32(1);
                    res.WriteInt32(1);

                    res.WriteInt32(1);

                    res.WriteInt32(1);

                }
            }
            res.WriteByte(1);
            res.WriteInt32(1);
            res.WriteFloat(1);

            Router.Send(client, (ushort) AreaPacketId.recv_quest_get_mission_quest_works_r, res, ServerType.Area);            
        }
    }
}