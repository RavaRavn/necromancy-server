using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_script_play_r : ClientHandler
    {
        public send_event_script_play_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_script_play_r;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound"); // find max size 
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_script_play, res, ServerType.Area);
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