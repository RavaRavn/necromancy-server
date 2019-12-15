using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_equip : ClientHandler
    {
        public send_item_equip(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_equip;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int unknown = packet.Data.ReadInt16();//Equip slot maybe?
            int backpackSlot = packet.Data.ReadInt16();//Slot from backpack the item is in
            int itemSerialId = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_equip_r, res, ServerType.Area);
        }
    }
}