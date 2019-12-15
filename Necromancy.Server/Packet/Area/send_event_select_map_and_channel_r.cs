using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_select_map_and_channel_r : ClientHandler
    {
        public send_event_select_map_and_channel_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_event_select_map_and_channel_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int mapId = packet.Data.ReadInt32();
            int channelId = packet.Data.ReadInt32();

            if (mapId == -2147483648)
            {
                // TODO remove this when done testing
                Logger.Debug("Escape button was selected to close dungeun select. MapID code  == -2147483648 => SendEventEnd");
                SendEventEnd(client);
                return;
            }

            if (!Server.Maps.TryGet(mapId, out Map map))
            {
                Logger.Error($"MapId: {mapId} does not exist");
                return;
            }
            // Default orietation from map.csv is not correct for some maps??
            if (map.Id == 2002104)
            {
                map.Orientation = 90;
            } else if (map.Id == 2002105)
            {
                map.Orientation = 133;
            }
            map.EnterForce(client);
            SendEventEnd(client);
        }

        private void SendEventEnd(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
        }
    }
}
