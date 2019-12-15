using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_accept_to_invite : ClientHandler
    {
        public send_party_accept_to_invite(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_accept_to_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int instanceId = packet.Data.ReadInt32(); //Could be a Party ID value hidden as character-who-made-it's value

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_party_accept_to_invite_r, res, ServerType.Area);

            SendPartyNotifyAddMember(client, instanceId);
        }

        private void SendPartyNotifyAddMember(NecClient client, int instanceId)
        {

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(instanceId); //Most likely insanceId
            res.WriteInt32(instanceId);
            res.WriteInt32(instanceId);
            res.WriteFixedString("soulname", 0x31); //Soul name
            res.WriteFixedString("charaname", 0x5B); //Character name
            res.WriteInt32(3); //Class
            res.WriteByte(2); //Soul rank
            res.WriteByte(4); //Character level
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteInt32(1001902); //Might make the character selectable?
            res.WriteInt32(1001902); //One half of location? 1001902 = Illfalo Port but is actually Deep Sea Port
            res.WriteInt32(1);
            res.WriteInt32(1);
            res.WriteFixedString("", 0x61); //Location of player if not in same zone
            res.WriteFloat(1);
            res.WriteFloat(1);
            res.WriteFloat(1);
            res.WriteByte(3);
            res.WriteByte(4);
            res.WriteByte(5);
            res.WriteByte(6);

            Router.Send(client.Map, (ushort)MsgPacketId.recv_party_notify_add_member, res, ServerType.Msg);
        }
    }
}
