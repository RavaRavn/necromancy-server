using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_map_get_info));

        private readonly NecServer _server;

        public send_map_get_info(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
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

                if (otherClient.Union != null)
                {
                    RecvDataNotifyUnionData otherUnionData =
                        new RecvDataNotifyUnionData(otherClient.Character, otherClient.Union.Name);
                    Router.Send(otherUnionData, client);
                }
            }

            foreach (MonsterSpawn monsterSpawn in client.Map.MonsterSpawns.Values)
            {
                if (monsterSpawn.Active == false)
                {
                    RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                    Logger.Debug($"Monster Id {monsterSpawn.Id} with model {monsterSpawn.ModelId} is loading");
                    Router.Send(monsterData, client);
                }
            }

            foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
            {
                // This requires database changes to add the GGates to the Npc database!!!!!
                if (npcSpawn.Name == "GGate")
                {
                    GGateSpawn gGate = new GGateSpawn();
                    gGate.X = npcSpawn.X;
                    gGate.Y = npcSpawn.Y;
                    gGate.Z = npcSpawn.Z;
                    gGate.Heading = npcSpawn.Heading;
                    gGate.MapId = npcSpawn.MapId;
                    gGate.Name = npcSpawn.Name;
                    gGate.Title = npcSpawn.Title;

                    RecvDataNotifyGGateData gGateData = new RecvDataNotifyGGateData(gGate);
                    Router.Send(gGateData, client);
                }
                else
                {
                    RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                    Router.Send(npcData, client);
                }
            }

            foreach (Gimmick gimmickSpawn in client.Map.GimmickSpawns.Values)
            {
                RecvDataNotifyGimmickData gimmickData = new RecvDataNotifyGimmickData(gimmickSpawn);
                Router.Send(gimmickData, client);
                GGateSpawn gGateSpawn = new GGateSpawn();
                Server.Instances.AssignInstance(gGateSpawn);
                gGateSpawn.X = gimmickSpawn.X;
                gGateSpawn.Y = gimmickSpawn.Y;
                gGateSpawn.Z = gimmickSpawn.Z + 300;
                gGateSpawn.Heading = gimmickSpawn.Heading;
                gGateSpawn.Name = $"gGateSpawn to your current position. ID {gimmickSpawn.ModelId}";
                gGateSpawn.Title = $"type '/gimmick move {gimmickSpawn.InstanceId} to move this ";
                gGateSpawn.MapId = gimmickSpawn.MapId;
                gGateSpawn.ModelId = 1900001;
                gGateSpawn.Active = 0;
                gGateSpawn.SerialId = 1900001;

                RecvDataNotifyGGateData gGateData = new RecvDataNotifyGGateData(gGateSpawn);
                Router.Send(gGateData, client);
            }

            foreach (GGateSpawn gGateSpawn in client.Map.GGateSpawns.Values)
            {
                RecvDataNotifyGGateData gGateSpawnData = new RecvDataNotifyGGateData(gGateSpawn);
                Router.Send(gGateSpawnData, client);
            }

            foreach (DeadBody deadBody in client.Map.DeadBodies.Values)
            {
                RecvDataNotifyCharaBodyData deadBodyData = new RecvDataNotifyCharaBodyData(deadBody, client);
                Router.Send(deadBodyData, client);
            }

            foreach (MapTransition mapTran in client.Map.MapTransitions.Values)
            {
                MapPosition mapPos = new MapPosition(mapTran.ReferencePos.X, mapTran.ReferencePos.Y,
                    mapTran.ReferencePos.Z, mapTran.MaplinkHeading);
                RecvDataNotifyMapLink mapLink = new RecvDataNotifyMapLink(client, this.Id, mapPos,
                    mapTran.MaplinkOffset, mapTran.MaplinkWidth, mapTran.MaplinkColor);
                Router.Send(mapLink, client);
            }

            // ToDo this should be a database lookup
            RecvMapFragmentFlag mapFragments = new RecvMapFragmentFlag(client.Map.Id, 0xff);
            Router.Send(mapFragments, client);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Map.Id);
            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res, ServerType.Area);
        }
    }
}
