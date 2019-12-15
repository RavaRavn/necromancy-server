using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_request_int_r : ClientHandler
    {
        public send_event_request_int_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_request_int_r;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteCString("hello you :)");//find max size
            res.WriteInt32(1);
            res.WriteInt32(2);
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_request_int, res, ServerType.Area);
            SendEventEnd(client);
        }
        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area, client);

        }

    }
}