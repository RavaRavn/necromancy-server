using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_blacklist_open : ClientHandler
    {
        public send_blacklist_open(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_blacklist_open;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

          
            res.WriteInt32(0);

            res.WriteInt32(10);

            for (int i = 0; i < 10; i++)
            {

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(1); // bool

                res.WriteInt32(0);
                res.WriteFixedString("soul name", 49);

                res.WriteInt32(0);
                res.WriteFixedString("chara name", 91);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("channel name", 97);

            }
            


            Router.Send(client, (ushort) AreaPacketId.recv_blacklist_open_r, res, ServerType.Area);
        }
    }
}