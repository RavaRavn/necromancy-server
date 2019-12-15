using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_search_recruited_party : ClientHandler
    {
        public send_party_search_recruited_party(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_search_recruited_party;

       public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            res.WriteInt32(0x14); // cmp to 0x14

            int numEntries = 0x14;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);

                res.WriteInt32(0);

                res.WriteInt32(0);

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("soulname", 49);
                res.WriteFixedString("charaname", 91);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0); // bool
                res.WriteByte(0);
                res.WriteByte(0);

            }

            Router.Send(client, (ushort)AreaPacketId.recv_party_search_recruited_party_r, res, ServerType.Area);
        }
    }
}