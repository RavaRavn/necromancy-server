using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_invite : ClientHandler
    {
        public send_party_invite(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            /*int32, unknown
              int32, target client(character) id[i think this is the sends structure]*/
            int unknown = packet.Data.ReadInt32();
            uint targetInstanceId = packet.Data.ReadUInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_party_invite_r, res, ServerType.Area);

            SendPartyNotifyInvite(client, targetInstanceId);
        }

        private void SendPartyNotifyInvite(NecClient client, uint targetInstanceId)
        {
            //recv_party_notify_invite

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.InstanceId);//Party maker client id
            res.WriteInt32(0);//Party type
            res.WriteInt32(0);//Normal item distribution
            res.WriteInt32(0);//Rare item distribution
            res.WriteInt32(0);
            res.WriteInt32(client.Character.Id);
            for (int i = 0; i < 4; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("fixed1", 0x31); //size is 0x31
                res.WriteFixedString("fixed2", 0x5B); //size is 0x5B
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); //bool
                res.WriteByte(0);
                res.WriteByte(0);
            }
            res.WriteByte(0);
            res.WriteFixedString("fixed3", 0xB5); //size is 0xB5

            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId), (ushort)MsgPacketId.recv_party_notify_invite, res, ServerType.Msg);
        }
    }
}
