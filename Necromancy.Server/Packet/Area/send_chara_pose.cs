using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_chara_pose : Handler
    {
        public send_chara_pose(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_chara_pose;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            client.Character.charaPose = packet.Data.ReadInt32();
            
            res.WriteInt32(0);  

            Router.Send(client, (ushort) AreaPacketId.recv_chara_pose_r, res);

            SendCharaPoseNotify(client);
        }

        private void SendCharaPoseNotify(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);//Character ID
            res.WriteInt32(client.Character.charaPose); //Character pose

            Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_pose_notify, res, client);
        }
    }
}