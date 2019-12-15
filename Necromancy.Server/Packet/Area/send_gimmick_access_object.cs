using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_gimmick_access_object : ClientHandler
    {
        public send_gimmick_access_object(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_gimmick_access_object;


        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetInstanceId = packet.Data.ReadInt32();
            int unknown = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();  // this is the buffer we create 
            res.WriteInt32(2); //our packets data
            Router.Send(client, (ushort)AreaPacketId.recv_gimmick_access_object_r, res, ServerType.Area); //this sends out our packet to the first operand

            SendGimmickAccessObjectNotify(client, targetInstanceId);
        }

        private void SendGimmickAccessObjectNotify(NecClient client, int targetInstanceId)
        {
            //recv_gimmick_access_object_notify = 0xD804,
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.InstanceId); //this is probably for letting others know who accessed it (Instance Id)
            res.WriteInt32(0);
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_gimmick_access_object_notify, res, ServerType.Area);
        }
    }
}
