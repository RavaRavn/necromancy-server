using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Auth
{
    public class send_base_select_world : ClientHandler
    {
        public send_base_select_world(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AuthPacketId.send_base_select_world;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint selectedWorld = packet.Data.ReadUInt32();

            Logger.Info($"Selected World: {selectedWorld}");


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("127.0.0.1"); //Message Server IP
            res.WriteInt32(60001); //Message Server Port
            Router.Send(client, (ushort) AuthPacketId.recv_base_select_world_r, res, ServerType.Auth);
        }
    }
}