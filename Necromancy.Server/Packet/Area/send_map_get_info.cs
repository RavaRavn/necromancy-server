using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : ClientHandler
    {
        public send_map_get_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Map.Id);
            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res, ServerType.Area);

            List<NpcSpawn> npcSpawns = Database.SelectNpcSpawnsByMapId(client.Map.Id);
            if (npcSpawns.Count > 0)
            {
                foreach (NpcSpawn npcSpawn in npcSpawns)
                {
                    Server.Instances.AssignInstance(npcSpawn);
                }

                RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawns);
                Router.Send(npcData, client);
            }
        }
    }
}
