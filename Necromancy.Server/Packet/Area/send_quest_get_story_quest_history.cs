using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_quest_get_story_quest_history : Handler
    {
        public send_quest_get_story_quest_history(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_quest_get_story_quest_history;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);//Bool

            //Router.Send(client, (ushort) AreaPacketId.recv_quest_get_story_quest_history, res);            
        }
    }
}