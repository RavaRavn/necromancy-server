using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_wanted_entry : ClientHandler
    {
        public send_wanted_entry(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_wanted_entry;

       public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);




            Router.Send(client, (ushort)AreaPacketId.recv_wanted_entry_r, res, ServerType.Area);
        }
    }
}