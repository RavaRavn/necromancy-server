using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using System;
using System.Collections.Generic;
using System.Numerics;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Model
{
    public class MonsterSpawn : IInstance
    {
        private readonly NecLogger _logger;
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int ModelId { get; set; }
        public byte Level { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public Map Map { get; set; }
        public bool Active { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public short Size { get; set; }
        public short Radius { get; set; }
        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }
        public int RespawnTime { get; set; }
        public int CurrentCoordIndex { get; set; }

        public int MonsterWalkVelocity { get; }
        public int MonsterRunVelocity { get; }
        public bool MonsterAgro { get; set; }
        public bool SpawnActive { get; set; }
        public bool TaskActive { get; set; }
        public bool MonsterVisible { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public List<MonsterCoord> monsterCoords;
        public bool defaultCoords { get; set; }
        public Dictionary<int, int> MonsterPlayerAgro { get; set; }
        public MonsterSpawn()
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            CurrentHp = 80085;
            MaxHp = 88887355;
            RespawnTime = 60000;
            SpawnActive = false;
            TaskActive = false;
            defaultCoords = true;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            monsterCoords = new List<MonsterCoord>();
            MonsterPlayerAgro = new Dictionary<int, int>();
            MonsterWalkVelocity = 250;
            MonsterRunVelocity = 500;
            MonsterVisible = false;
            MonsterAgro = false;
#if false
            //To-Do   add at least 1 default monster coord for /mon spawns
            Vector3 defaultVector3 = new Vector3(X,Y,Z); 
            MonsterCoord defaultCoord = new MonsterCoord();
            defaultCoord.Id = Id;
            defaultCoord.MonsterId = (uint)MonsterId;
            defaultCoord.MapId = (uint)MapId;
            defaultCoord.destination = defaultVector3;

            monsterCoords.Add(defaultCoord);

            //To-Do Next.  make a default heading for _monster.Heading = (byte)GetHeading(_monster.monsterCoords[1].destination);
#endif
        }

        public void MonsterStop(NecServer server, NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(this.InstanceId);//Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(0);       //X per tick
            res.WriteFloat(0);       //Y Per tick
            res.WriteFloat(0);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1);              //movementMultiplier
            res.WriteFloat((float)1);              //Seconds to move

            res.WriteByte(0); //MOVEMENT ANIM
            res.WriteByte(0);//JUMP & FALLING ANIM
            server.Router.Send(Map, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }
        public void MonsterMove(NecServer server, NecClient client, int monsterVelocity, MonsterCoord monsterCoord = null)
        {
            if (monsterCoord == null)
                monsterCoord = monsterCoords[CurrentCoordIndex];
            Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y, monsterCoord.destination.Z);
            Vector3 monsterPos = new Vector3(this.X, this.Y, this.Z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(this.InstanceId);//Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(moveTo.X);       //X per tick
            res.WriteFloat(moveTo.Y);       //Y Per tick
            res.WriteFloat((float)1);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1 / travelTime);              //movementMultiplier
            res.WriteFloat((float)travelTime);              //Seconds to move

            res.WriteByte(2); //MOVEMENT ANIM
            res.WriteByte(0);//JUMP & FALLING ANIM
            server.Router.Send(client, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);

        }

        public void MonsterMove(NecServer server, int monsterVelocity, MonsterCoord monsterCoord = null)
        {
            if (monsterCoord == null)
                monsterCoord = monsterCoords[CurrentCoordIndex];
            Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y, monsterCoord.destination.Z);
            Vector3 monsterPos = new Vector3(this.X, this.Y, this.Z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;
            float xTick = moveTo.X / travelTime;
            float yTick = moveTo.Y / travelTime;
            float zTick = moveTo.Z / travelTime;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(this.InstanceId);//Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(moveTo.X);       //X per tick
            res.WriteFloat(moveTo.Y);       //Y Per tick
            res.WriteFloat((float)1);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1 / travelTime);              //movementMultiplier
            res.WriteFloat((float)travelTime);              //Seconds to move

            res.WriteByte(2); //MOVEMENT ANIM
            res.WriteByte(0);//JUMP & FALLING ANIM
            server.Router.Send(Map, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }
        public void MonsterMove(NecServer server, int monsterVelocity, MonsterTick moveTo, float travelTime)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(this.InstanceId);//Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(moveTo.xTick);       //X per tick
            res.WriteFloat(moveTo.yTick);       //Y Per tick
            res.WriteFloat((float)1);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1 / travelTime);              //movementMultiplier
            res.WriteFloat((float)travelTime);              //Seconds to move

            res.WriteByte(3); //MOVEMENT ANIM
            res.WriteByte(0);//JUMP & FALLING ANIM
            server.Router.Send(Map, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void MonsterOrient(NecServer server)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(this.InstanceId);

            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteByte(this.Heading);
            res.WriteByte(1);
            server.Router.Send(Map, (ushort)AreaPacketId.recv_0x6B6A, res, ServerType.Area);
        }
        public void MonsterOrient(NecServer server, NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(this.InstanceId);

            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteByte(this.Heading);
            res.WriteByte(1);
            server.Router.Send(client, (ushort)AreaPacketId.recv_0x6B6A, res, ServerType.Area);
        }
    }
    public class MonsterTick
    {
        public float xTick;
        public float yTick;
        public float zTick;
    }
    public class MonsterCoord
    {
        public int Id;
        public uint MonsterId { get; set; }
        public uint MapId { get; set; }
        public int CoordIdx { get; set; }
        public Vector3 destination { get; set; }

    }
}
