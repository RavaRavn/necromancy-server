using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : ClientHandler
    {
        private readonly NecServer _server;
        public send_map_get_info(NecServer server) : base(server)
        {
        _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Map.Id);
            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res, ServerType.Area);

            foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
            {
                // This requires database changes to add the GGates to the Npc database!!!!!
                if (npcSpawn.Name == "GGate")
                {
                    RecvDataNotifyGGateData gGateData = new RecvDataNotifyGGateData(npcSpawn);
                    Router.Send(gGateData, client);
                }
                else
                {
                    RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                    Router.Send(npcData, client);
                }
            }

            foreach (MonsterSpawn monsterSpawn in client.Map.MonsterSpawns.Values)
            {
                monsterSpawn.SpawnActive = true;
                if (!monsterSpawn.TaskActive)
                {
                    MonsterTask monsterTask = new MonsterTask(Server, monsterSpawn);
                    if (monsterSpawn.defaultCoords)
                        monsterTask.monsterHome = monsterSpawn.monsterCoords[0];
                    else
                        monsterTask.monsterHome = monsterSpawn.monsterCoords.Find(x => x.CoordIdx == 64);
                    monsterTask.Start();
                }
                else
                {
                    if (monsterSpawn.MonsterVisible)
                    {
                        if (monsterSpawn.MonsterAgro)
                        {
                            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                            Server.Router.Send(monsterData, client);
                            monsterSpawn.MonsterStop(_server, client);      // Without some kind of movement the monster doesn't show  why?????
                        }
                        else
                        {
                            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                            Server.Router.Send(monsterData, client);
                            monsterSpawn.MonsterMove(_server, client, monsterSpawn.MonsterWalkVelocity);
                        }
                    }
                }
            }

            foreach (NecClient otherClient in client.Map.ClientLookup.GetAll())
            {
                if (otherClient == client)
                {
                    // skip myself
                    continue;
                }

                RecvDataNotifyCharaData otherCharacterData =
                    new RecvDataNotifyCharaData(otherClient.Character, otherClient.Soul.Name);
                Router.Send(otherCharacterData, client);
            }
        }

      
    }
}
