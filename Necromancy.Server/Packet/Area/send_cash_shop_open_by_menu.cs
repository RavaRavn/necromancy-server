using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_cash_shop_open_by_menu : ClientHandler
    {
        public send_cash_shop_open_by_menu(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_cash_shop_open_by_menu;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);//Bool

            //Router.Send(client, (ushort) AreaPacketId.recv_cash_shop_open_by_menu, res, ServerType.Area);            
        }
    }
}