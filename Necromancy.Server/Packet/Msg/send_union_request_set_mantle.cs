using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_set_mantle : ClientHandler
    {
        public send_union_request_set_mantle(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_set_mantle;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
           
            res.WriteInt32(0);
            
            Router.Send(client, (ushort) MsgPacketId.recv_union_request_set_mantle_r, res, ServerType.Msg);
        }

    }
    }
