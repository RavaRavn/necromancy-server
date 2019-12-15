using System;
using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Services.Logging;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Model
{
    public class Map
    {
        public const int NewCharacterMapId = 1001902;

        private readonly NecLogger _logger;
        private readonly NecServer _server;

        public Map(MapSetting setting, NecServer server)
        {
            _server = server;
            _logger = LogProvider.Logger<NecLogger>(this);
            ClientLookup = new ClientLookup();
            NpcSpawns = new Dictionary<int, NpcSpawn>();
            MonsterSpawns = new Dictionary<int, MonsterSpawn>();
            Id = setting.Id;
            X = setting.X;
            Y = setting.Y;
            Z = setting.Z;
            Country = setting.Country;
            Area = setting.Area;
            Place = setting.Place;
            Orientation = setting.Orientation;
            MonsterTasks = new TaskManager();

            //Assign Unique Instance ID to each NPC per map. Add to dictionary stored with the Map object
            List<NpcSpawn> npcSpawns = server.Database.SelectNpcSpawnsByMapId(setting.Id);
            foreach (NpcSpawn npcSpawn in npcSpawns)
            {
                server.Instances.AssignInstance(npcSpawn);
                NpcSpawns.Add((int)npcSpawn.InstanceId, npcSpawn);
            }

            List<MonsterSpawn> monsterSpawns = server.Database.SelectMonsterSpawnsByMapId(setting.Id);
            foreach (MonsterSpawn monsterSpawn in monsterSpawns)
            {
                server.Instances.AssignInstance(monsterSpawn);
                if (!_server.SettingRepository.ModelCommon.TryGetValue(monsterSpawn.ModelId, out ModelCommonSetting modelSetting))
                {
                    return;
                }
                monsterSpawn.ModelId = modelSetting.Id;
                monsterSpawn.Size = (short)(modelSetting.Height / 2);
                monsterSpawn.Radius = (short)modelSetting.Radius;
                monsterSpawn.MaxHp = 1000;
                monsterSpawn.CurrentHp = 100;
                monsterSpawn.Map = this;
                MonsterSpawns.Add((int)monsterSpawn.InstanceId, monsterSpawn);

                List<MonsterCoord> coords = server.Database.SelectMonsterCoordsByMonsterId(monsterSpawn.Id);
                if (coords.Count > 0)
                {
                    monsterSpawn.defaultCoords = false;
                    monsterSpawn.monsterCoords.Clear();
                    foreach (MonsterCoord monsterCoord in coords)
                    {
                        //Console.WriteLine($"added coord {monsterCoord} to monster {monsterSpawn.InstanceId}");
                        monsterSpawn.monsterCoords.Add(monsterCoord);
                    }
                }
                else
                {

                    //home coordinate set to monster X,Y,Z from database
                    Vector3 homeVector3 = new Vector3(monsterSpawn.X, monsterSpawn.Y, monsterSpawn.Z);
                    MonsterCoord homeCoord = new MonsterCoord();
                    homeCoord.Id = monsterSpawn.Id;
                    homeCoord.MonsterId = (uint)monsterSpawn.MonsterId;
                    homeCoord.MapId = (uint)monsterSpawn.MapId;
                    homeCoord.CoordIdx = 0;
                    homeCoord.destination = homeVector3;
                    monsterSpawn.monsterCoords.Add(homeCoord);

                    //default path part 2
                    Vector3 defaultVector3 = new Vector3(monsterSpawn.X, monsterSpawn.Y + 100, monsterSpawn.Z);
                    MonsterCoord defaultCoord = new MonsterCoord();
                    defaultCoord.Id = monsterSpawn.Id;
                    defaultCoord.MonsterId = (uint)monsterSpawn.MonsterId;
                    defaultCoord.MapId = (uint)monsterSpawn.MapId;
                    defaultCoord.CoordIdx = 1; 
                    defaultCoord.destination = defaultVector3;

                    monsterSpawn.monsterCoords.Add(defaultCoord);

                    //default path part 3
                    Vector3 defaultVector32 = new Vector3(monsterSpawn.X + 100, monsterSpawn.Y + 100, monsterSpawn.Z);
                    MonsterCoord defaultCoord2 = new MonsterCoord();
                    defaultCoord2.Id = monsterSpawn.Id;
                    defaultCoord2.MonsterId = (uint)monsterSpawn.MonsterId;
                    defaultCoord2.MapId = (uint)monsterSpawn.MapId;
                    defaultCoord2.CoordIdx = 2; //64 is currently the Idx of monsterHome on send_map_get_info.cs
                    defaultCoord2.destination = defaultVector32;

                    monsterSpawn.monsterCoords.Add(defaultCoord2);

                }
                
            }
        }

        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public int Orientation { get; set; }
        public string FullName => $"{Country}/{Area}/{Place}";
        public ClientLookup ClientLookup { get; }
        public Dictionary<int, NpcSpawn> NpcSpawns { get; }
        public Dictionary<int, MonsterSpawn> MonsterSpawns { get; }

        public TaskManager MonsterTasks;

        public void EnterForce(NecClient client)
        {
            Enter(client);
            _server.Router.Send(new RecvMapChangeForce(this), client);
        }

        public void Enter(NecClient client)
        {
            if (client.Map != null)
            {
                client.Map.Leave(client);
            }

            _logger.Info(client, $"Entering Map: {Id}:{FullName}", client);
            ClientLookup.Add(client);
            client.Map = this;
            client.Character.MapId = Id;
            client.Character.X = X;
            client.Character.Y = Y;
            client.Character.Z = Z;

            RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
            _server.Router.Send(this, myCharacterData, client);

        }

        public void Leave(NecClient client)
        {
            _logger.Info(client, $"Leaving Map: {Id}:{FullName}", client);
            ClientLookup.Remove(client);
            client.Map = null;

            RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(client.Character.InstanceId);
            _server.Router.Send(this, objectDisappearData, client);
            if (ClientLookup.GetAll().Count == 0)
            {
                foreach (MonsterSpawn monsterSpawn in this.MonsterSpawns.Values)
                {
                    monsterSpawn.SpawnActive = false;
                }
            }
        }
    }
}
