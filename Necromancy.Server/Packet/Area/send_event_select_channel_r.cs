using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_select_channel_r : ClientHandler
    {
        public send_event_select_channel_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_select_channel_r;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteByte(0);

            int numEntries = 0x80;
            for (int i = 0; i < numEntries; i++)

            {
                res.WriteInt32(4);
                res.WriteFixedString("BoobsChannel", 0x61); // Channel names
                res.WriteByte(0); // Bool
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }
            res.WriteByte(4); // Numbers of channel, 0 = no channel
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_select_channel, res, ServerType.Area);
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