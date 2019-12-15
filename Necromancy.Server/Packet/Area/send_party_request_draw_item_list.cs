using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_request_draw_item_list : ClientHandler
    {
        public send_party_request_draw_item_list(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_request_draw_item_list;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            
            res.WriteInt32(0);//Error?
            res.WriteInt32(0);//Map ID?
            res.WriteFixedString("127.0.0.1", 65);
            res.WriteInt16(60002);

            //sub_484420
            res.WriteFloat(1);
            res.WriteFloat(2);
            res.WriteFloat(3);
            res.WriteByte(1);
            //

            //Router.Send(client, (ushort)AreaPacketId.recv_map_change_force, res, ServerType.Area);
        }
    }
}