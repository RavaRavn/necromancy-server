using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_enter : ClientHandler
    {
        public send_map_enter(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_enter;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0); //Bool
            Router.Send(client, (ushort) AreaPacketId.recv_map_enter_r, res, ServerType.Area);
            
           
            
        }

        private void SendDataNotifyCharaData(NecClient client, NecClient thisNecClient)
        {
            SendMapBGM(client);
            client.Character.weaponEquipped = false;
        }

        private void SendMapBGM(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(100401);


            Router.Send(client.Map, (ushort) AreaPacketId.recv_map_update_bgm, res, ServerType.Area, client);
        }
    }
}
