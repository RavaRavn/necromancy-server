using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_reply_to_link2 : ClientHandler
    {
        public send_friend_reply_to_link2(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_friend_reply_to_link2;



        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(1); // 0  = msg friend added

            Router.Send(client, (ushort)MsgPacketId.recv_friend_result_reply_link2, res, ServerType.Msg);
        }
    }
}