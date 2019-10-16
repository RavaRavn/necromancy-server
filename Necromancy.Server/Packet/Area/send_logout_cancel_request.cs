using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_logout_cancel_request : ClientHandler
    {
        public send_logout_cancel_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_logout_cancel_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            client.Character.logoutCanceled = 1;

            res.WriteInt32(0);//Ready to discover

            Router.Send(client, (ushort) AreaPacketId.recv_logout_cancel_request_r, res);
        }
    }
}