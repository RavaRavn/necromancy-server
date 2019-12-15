using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_reply_to_invite2 : ClientHandler
    {
        public send_union_reply_to_invite2(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_reply_to_invite2;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
           

            Router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res, ServerType.Msg);
        }
    }
}