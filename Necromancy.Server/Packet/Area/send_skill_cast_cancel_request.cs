using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Threading;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_cast_cancel_request : ClientHandler
    {
        public send_skill_cast_cancel_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_cast_cancel_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_skill_cast_cancel, res, ServerType.Area);
        }
    }
}