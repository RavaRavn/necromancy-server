using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_request_link_target : ClientHandler
    {
        public send_friend_request_link_target(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_friend_request_link_target;

        public override void Handle(NecClient client, NecPacket packet)
        {


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = sucess
            res.WriteInt32(0);
            Router.Send(client, (ushort)MsgPacketId.recv_friend_request_link_target_r, res, ServerType.Msg);
            NotifyFriendInvite(client);
        }
        private void NotifyFriendInvite(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.Id); // Change nothing visibaly ?
            res2.WriteInt32(0);
            res2.WriteFixedString($"{client.Soul.Name}", 0x31); //size is 0x31
            res2.WriteFixedString($"{client.Character.Name}", 0x5B); //size is 0x5B
            res2.WriteInt32(0);
            res2.WriteByte(0);
            Router.Send(client, (ushort)MsgPacketId.recv_friend_notify_link_invite, res2, ServerType.Msg);
        }
    }
}