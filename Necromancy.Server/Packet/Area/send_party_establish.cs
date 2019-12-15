using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_establish : ClientHandler
    {
        public send_party_establish(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_party_establish;

        public override void Handle(NecClient client, NecPacket packet)
        {
            /*int32, party type: 1 = open, 0 = closed
              int32, normal item distribution: 1 = random, 0 = do not distribute
              int32, rare item distribution: 1 = draw, 0 = do not distribute
              int32, target client(character) id*/

            int partyType = packet.Data.ReadInt32();
            int normItemDist = packet.Data.ReadInt32();
            int rareItemDist = packet.Data.ReadInt32();
            int targetClient = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_party_establish_r, res, ServerType.Area);

            SendPartyNotifyEstablish(client, partyType, normItemDist, rareItemDist, targetClient);
        }

        private void SendPartyNotifyEstablish(NecClient client, int partyType, int normItemDist, int rareItemDist, int targetClient)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);//Party maker client id
            res.WriteInt32(partyType);//Party type
            res.WriteInt32(normItemDist);//Normal item distribution
            res.WriteInt32(rareItemDist);//Rare item distribution
            res.WriteInt32(69);
            res.WriteInt32(client.Character.Id);
            //for (int i = 0; i < 4; i++)
            {
                res.WriteInt32(client.Character.Id);
                res.WriteInt32(0);
                res.WriteFixedString("asdf1", 0x31); //size is 0x31
                res.WriteFixedString("asdf2", 0x5B); //size is 0x5B
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("asdf3", 0x31); //size is 0x31
                res.WriteFixedString("asdf4", 0x5B); //size is 0x5B
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("asdf5", 0x31); //size is 0x31
                res.WriteFixedString("asdf6", 0x5B); //size is 0x5B
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("asdf7", 0x31); //size is 0x31
                res.WriteFixedString("asdf8", 0x5B); //size is 0x5B
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
            }
            res.WriteByte(0);

            Router.Send(client, (ushort)MsgPacketId.recv_party_notify_establish, res, ServerType.Msg);
        }

        private void SendPartyRegistMemberRecruit(NecClient client)
        {
            //recv_party_regist_member_recruit_r = 0xA7BF,

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_party_establish_r, res, ServerType.Area);
        }

        private void SendPartyNotifyRecruitRequest(NecClient client, int partyType, int normItemDist, int rareItemDist, int targetClient)
        {
            //recv_party_notify_recruit_request = 0x9F8F, // Parent = 0x9F70 // Range ID = 02 // after -> send_party_regist_party_recruit

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(partyType);
            res.WriteInt32(normItemDist);
            res.WriteInt32(rareItemDist);
            res.WriteInt32(targetClient);
            Router.Send(client, (ushort)AreaPacketId.recv_party_notify_recruit_request, res, ServerType.Area);
        }

        private void SendPartyInvite(NecClient client, int targetClient)
        {
            //recv_party_invite_r = 0x300A, 
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort)AreaPacketId.recv_party_invite_r, res, ServerType.Area);
        }

        private void SendPartyApply(NecClient client, int targetClient)
        {
            //recv_party_apply_r = 0x5F1A,

            IBuffer res = BufferProvider.Provide();

        	res.WriteInt32(targetClient);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_party_apply_r, res, ServerType.Area);
        }

        private void SendCharaNotifyPartyJoin(NecClient client)
        {
            //recv_chara_notify_party_join = 0x58A8,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_chara_notify_party_join, res, ServerType.Area);
        }

        private void SendPartyChangeLeader(NecClient client)
        {
            //recv_party_change_leader_r = 0x7BB3,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);

            Router.Send(client.Map, (ushort)AreaPacketId.recv_party_change_leader_r, res, ServerType.Area);
        }
    }
}