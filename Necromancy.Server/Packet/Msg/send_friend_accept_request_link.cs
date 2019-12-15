using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_accept_request_link : ClientHandler
    {
        public send_friend_accept_request_link(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)MsgPacketId.send_friend_accept_request_link;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  // 0 = sucess, 1 = Request was not approved
            res.WriteInt32(0); // ??
            Router.Send(client, (ushort)MsgPacketId.recv_friend_reply_to_link_r, res, ServerType.Msg);
            NotifyFriendInvite(client);
        }

        private void NotifyFriendInvite(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.Id); // ?
            res.WriteInt32(0);
            res.WriteFixedString($"{client.Soul.Name}", 0x31); //soul name?
            res.WriteFixedString($"{client.Character.Name}", 0x5B); //character name?
            res.WriteInt32(0);  // Class 0 = Fighter, 1 = thief, ect....
            res.WriteByte(0); // Level of the friend
            res.WriteInt32(1001001); // Location of your friend
            res.WriteInt32(0);
            res.WriteFixedString($"dosn't work", 0x61); // When i put the channel it doesn't add the friend, need to fix that.
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            Router.Send(client, (ushort)MsgPacketId.recv_friend_notify_add_member_r, res, ServerType.Msg);
        } 
    }
}