using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_refusallist_add_user : ClientHandler
    {
        public send_refusallist_add_user(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_refusallist_add_user;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            int myRefusalListAddIdPart1 = packet.Data.ReadInt32();
            int myRefusalListAddIdPart2 = packet.Data.ReadInt32();
            
            res.WriteInt32(myRefusalListAddIdPart1);
            res.WriteInt32(999);

            Router.Send(client, (ushort) AreaPacketId.recv_refusallist_add_user_r, res, ServerType.Area);
        }
    }
}