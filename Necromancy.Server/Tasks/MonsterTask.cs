using Arrowgene.Services.Buffers;
using Arrowgene.Services.Tasks;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;


namespace Necromancy.Server.Tasks
{
    // Usage: create a monster and spawn it then use the following
    //MonsterThread monsterThread = new MonsterThread(Server, client, monsterSpawn);
    //Thread workerThread2 = new Thread(monsterThread.InstanceMethod);
    //workerThread2.Start();

    class MonsterTask : PeriodicTask
    {
        protected NecServer Server { get; }
        protected PacketRouter Router { get; }
        public MonsterSpawn _monster { get; set; }
        public bool monsterFreeze { get; set; }
        public bool monsterActive { get; set; }
        public bool monsterMoving { get; set; }
        public bool hateOn { get; set; }
        private bool casting;
        private bool battlePose;
        private bool monsterWaiting;
        private bool spawnMonster;
        public int gotoDistance { get; set; }
        public int agroRange { get; set; }

        //private int currentDest;
        private int moveTime;
        private int updateTime;
        private int waitTime;
        private int pathingTick;
        private int agroTick;
        //private int monsterVelocity;
        private int respawnTime;
        private int agroCheckTime;
        private int currentWait;
        private float fovAngle;
        private Map Map;

        private Character currentTarget;

        public MonsterCoord monsterHome;
        public MonsterTask(NecServer server, MonsterSpawn monster)
        {
            _monster = monster;
            Server = server;
            Router = Server.Router;
            monsterFreeze = false;
            monsterActive = true;
            monsterMoving = false;
            hateOn = false;
            battlePose = false;
            casting = false;
            spawnMonster = true;
            monsterHome = null;
            _monster.CurrentCoordIndex = 1;
            pathingTick = 100;
            agroTick = 500;
            updateTime = pathingTick;
            waitTime = 2000;
            currentWait = 0;
            moveTime = updateTime;
            monsterWaiting = true;
            agroRange = 1000;
            agroCheckTime = -1;
            fovAngle = (float)Math.Cos(Math.PI / 2);
            RunAtStart = false;
            Name = monster.Name;
            respawnTime = 10000;
            currentTarget = null;
            Map = Server.Maps.Get(_monster.MapId);
        }

        public override string Name { get; }
        public override TimeSpan TimeSpan { get; }
        protected override bool RunAtStart { get; }
        protected override void Execute()
        {
            _monster.TaskActive = true;
            while (monsterActive && _monster.SpawnActive)
            {
                if (spawnMonster)
                {
                    Thread.Sleep(respawnTime/5);
                    updateTime = pathingTick;
                    _monster.CurrentCoordIndex = 1;
                    moveTime = updateTime;
                    MonsterSpawn();
                    //SendBattleReportStartNotify();
                    //MonsterBattlePose(false);
                    //SendBattleReportEndNotify();
                    Thread.Sleep(2000);
                }
                MonsterCoord nextCoord = _monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex);
                Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
                float distance = GetDistance(nextCoord.destination, monster);
                if (_monster.MonsterAgro)
                {
                    if (MonsterAgro())
                        continue;
                }
                else
                { 
                    MonsterPath();
                }
                Thread.Sleep(moveTime);
                if (monsterWaiting)
                {
                    currentWait += updateTime;
                    if (currentWait >= waitTime)
                    {
                        monsterWaiting = false;
                        currentWait = 0;
                    }
                }
            }
            this.Stop();
            _monster.TaskActive = false;
        }

        private void MonsterPath()
        {
            MonsterCoord nextCoord = _monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex);
            Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
            float distance = GetDistance(nextCoord.destination, monster);
            if (distance > gotoDistance && !monsterFreeze && !monsterWaiting && !_monster.MonsterAgro)
            {
                MonsterMove(nextCoord);
            }
            else if (monsterMoving)
            {
                //Thread.Sleep(updateTime/2); //Allow for cases where the remaining distance is less than the gotoDistance
                MonsterStop();
                if (!_monster.MonsterAgro)
                {
                    monsterWaiting = true;
                    currentWait = 0;
                    //                        Thread.Sleep(2000);
                    if (_monster.CurrentCoordIndex < _monster.monsterCoords.Count - (_monster.defaultCoords ? 1 : 2))
                        _monster.CurrentCoordIndex++;
                    else
                        _monster.CurrentCoordIndex = 0;

                    _monster.Heading = (byte)GetHeading(_monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex).destination);
                }
            }
            _monster.MonsterAgro = MonsterAgroCheck();
            if (_monster.MonsterAgro)
            {
                MonsterStop();
                MonsterHate(true, (int)currentTarget.InstanceId);
                updateTime = agroTick;
                gotoDistance = 200;
                //monsterVelocity = 500;
                moveTime = agroTick;
                agroCheckTime = 0;

                orientMonster();
                MonsterAgroMove();

            }
        }
        private bool MonsterAgro()
        {
            Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
            if (currentTarget == null)
            {
                Logger.Error($"No character target set for agroed monster");
                return true;
            }
            if (MonsterCheck())
            {
                Logger.Debug($"MonsterCheck returned true");
                return true;
            }
            float homeDistance = GetDistance(monsterHome.destination, monster);
            if (homeDistance >= (agroRange * 2))
            {
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                Router.Send(Map, objectDisappearData);
                spawnMonster = true;
                Logger.Debug($"Too far from home");
                return true;
            }
            MonsterAgroAdjust();
            Vector3 character = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            float distanceChar = GetDistance(character, monster);
            if (distanceChar <= gotoDistance)
            {
                if (monsterMoving)
                {
                    Thread.Sleep(updateTime/2);
                    MonsterStop();
                    Thread.Sleep(500);
                }
                if (!monsterWaiting)
                {
                    SendBattleReportStartNotify();
                    MonsterBattlePose(true);
                    if (_monster.Id == 5)
                        MonsterAttack();
                    //MonsterCast();
                    SendBattleReportEndNotify();
                    monsterWaiting = true;
                    currentWait = 0;
                }
            }
            else
            {
                if (!casting && CheckHeading() == false)
                    orientMonster();
                MonsterAgroMove();
            }
            return false;
        }
        private void MonsterCast()
        {
            casting = true;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            //res.WriteInt32(1410101);
            //res.WriteFloat(4.0F);
            //Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_skill_start_cast, res, ServerType.Area);

            res = null;
            res = BufferProvider.Provide();
            res.WriteInt32(1410201);
            res.WriteInt32(1);
            res.WriteFloat(4.0F);
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, res, ServerType.Area);


        }

        private void MonsterAttack()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            //res.WriteInt32(10010401); //From monster attack
            res.WriteInt32(1410201); //From monster attack
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_action_attack_exec, res, ServerType.Area);


        }
        public void MonsterHate(bool hateOn, int instanceId)
        {
            if (this.hateOn != hateOn)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(_monster.InstanceId);
                res.WriteInt32(instanceId);
                if (hateOn)
                {
                    Router.Send(Map, (ushort)AreaPacketId.recv_monster_hate_on, res, ServerType.Area);
                }
                else
                {
                    Router.Send(Map, (ushort)AreaPacketId.recv_monster_hate_off, res, ServerType.Area);
                }
                this.hateOn = hateOn;
            }
        }
        public void MonsterSpawn()
        {
            _monster.MonsterAgro = false;
            monsterMoving = false;
            casting = false;
            monsterWaiting = false;
            gotoDistance = 10;
            //monsterVelocity = 200;
            MonsterCoord spawnCoords = _monster.monsterCoords.Find(x => x.CoordIdx == 0);
            _monster.X = spawnCoords.destination.X; 
            _monster.Y = spawnCoords.destination.Y; 
            _monster.Z = spawnCoords.destination.Z; 
            _monster.Heading = (byte)GetHeading(_monster.monsterCoords.Find(x => x.CoordIdx == 1).destination); 
            _monster.CurrentHp = 100;
            respawnTime = _monster.RespawnTime;
            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(_monster);
            Router.Send(Map, monsterData);
            spawnMonster = false;
            _monster.MonsterVisible = true;
            _monster.MonsterPlayerAgro.Clear();
            //MonsterBattlePose(false);
        }
        public bool MonsterCheck()
        {
            //Logger.Debug($"Monster HP [{_monster.CurrentHp}]");
            if (_monster.CurrentHp <= 0)
            {
                SendBattleReportStartNotify();
                //Death Animation
                IBuffer res5 = BufferProvider.Provide();
                res5.WriteInt32(_monster.InstanceId);
                res5.WriteInt32(1); //Death int
                res5.WriteInt32(0);
                res5.WriteInt32(0);
                Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_dead, res5, ServerType.Area);

                SendBattleReportEndNotify();

                //Make the monster a lootable state
                IBuffer res10 = BufferProvider.Provide();
                res10.WriteInt32(_monster.InstanceId);
                res10.WriteInt32(2);//Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing). 
                Router.Send(Map, (ushort)AreaPacketId.recv_monster_state_update_notify, res10, ServerType.Area);

                //  Let a separate loot manager handle the monster body click?
                Thread.Sleep(_monster.RespawnTime);
                //decompose the body
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteInt32(_monster.InstanceId);
                res7.WriteInt32(5);//4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                res7.WriteInt32(1);
                Router.Send(Map, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res7, ServerType.Area);
                Thread.Sleep(2000);
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(_monster.InstanceId);
                Router.Send(Map, objectDisappearData);
                _monster.MonsterVisible = false;

                spawnMonster = true;
                return true;
            }
            return false;
        }
        public void MonsterBattlePose(bool poseOn)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            if (poseOn)
            {
                Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area);
                battlePose = true;
            }
            else
            {
                Router.Send(Map, (ushort)AreaPacketId.recv_battle_attack_pose_end_notify, res, ServerType.Area);
                battlePose = false;
            }
        }
        private void MonsterMove(MonsterCoord monsterCoord)
        {

            //ShowVectorInfo(_monster.X, _monster.Y, monsterCoord.destination.X, monsterCoord.destination.Y);
            //ShowMonsterInfo();
            if (!monsterMoving)
            {
                orientMonster();
                _monster.MonsterMove(Server,_monster.MonsterWalkVelocity);
                monsterMoving = true;
            }
            else
            {
                Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y, monsterCoord.destination.Z);
                Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
                Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
                float distance = Vector3.Distance(monsterPos, destPos);
                float travelTime = distance / _monster.MonsterWalkVelocity;
                float xTick = moveTo.X / travelTime;
                float yTick = moveTo.Y / travelTime;
                float zTick = moveTo.Z / travelTime;
                int tickDivisor = 1000 / updateTime;

                if (distance >= _monster.MonsterWalkVelocity / tickDivisor)
                {
                    _monster.X = _monster.X + (moveTo.X / travelTime) / tickDivisor;
                    _monster.Y = _monster.Y + (moveTo.Y / travelTime) / tickDivisor;
                    //_monster.Z = _monster.Z + (moveTo.Z / travelTime) / tickDivisor;
                }
                else
                {
                    _monster.X = destPos.X;
                    _monster.Y = destPos.Y;
                    _monster.Z = destPos.Z;
                }
                //Logger.Debug($"distance [{distance}] travelTime[{travelTime}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}] moveTo.Z [{moveTo.Z}]");
            }
            //            Logger.Debug($"distance [{distance}]");
            //            ShowMonsterInfo();
        }
        private void MonsterAgroMove()
        {
            Vector3 charPos = new Vector3(currentTarget.X, currentTarget.Y, currentTarget.Z);
            Vector3 monsterPos = new Vector3(_monster.X, _monster.Y, _monster.Z);
            Vector3 moveTo = Vector3.Subtract(charPos, monsterPos);

            float distance = Vector3.Distance(monsterPos, charPos);
            //Logger.Debug($"distance [{distance}]");
            ShowVectorInfo(_monster.X, _monster.Y, _monster.Z, currentTarget.X, currentTarget.Y, currentTarget.Z);
            if (distance <= gotoDistance)
            {
                if (monsterMoving)
                {
                    MonsterStop();
                }
                return;
            }
            //Logger.Debug($"moving [{moving}]");
            //Vector2 moveTo = GetVector(monsterSpawn.X, monsterSpawn.Y, client.Character.X, client.Character.Y);
            //ShowMonsterInfo(monsterSpawn);
            if (!monsterMoving)
                monsterMoving = true;


            int tickDivisor = 1000 / moveTime;
            float travelTime = (float)moveTime / 1000;
            MonsterTick tick = new MonsterTick();
            tick.xTick = (moveTo.X * travelTime);
            tick.yTick = (moveTo.Y * travelTime);
            tick.zTick = (moveTo.Z * travelTime);
            //Logger.Debug($"distance [{distance}] monsterVelocity [{_monster.MonsterRunVelocity}]  travelTime[{travelTime}] xTick [{tick.xTick}] yTick [{tick.yTick}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}] moveTo.Z [{moveTo.Z}]");
            _monster.MonsterMove(Server, _monster.MonsterRunVelocity, tick, travelTime);

            _monster.X = _monster.X + tick.xTick;
            _monster.Y = _monster.Y + tick.yTick;
            _monster.Z = _monster.Z + tick.zTick;
        }

        private bool MonsterAgroCheck()
        {
           List<NecClient> mapsClients = Map.ClientLookup.GetAll();

            Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
            foreach (NecClient client in mapsClients)
            {
                Vector3 character = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
                float distanceChar = GetDistance(character, monster);
                if (distanceChar <= agroRange)
                {
                    if (checkFOV(client))
                    {
                        currentTarget = client.Character;
                        _monster.MonsterAgro = true;
                        _monster.MonsterPlayerAgro.Add((int)client.Character.InstanceId, 0);
                    }
                }
            }

            return _monster.MonsterAgro;
        }

        private void MonsterAgroAdjust()
        {
            if (agroCheckTime != -1 && agroCheckTime < 3000)
            {
                agroCheckTime += updateTime;
                return;
            }
            int currentInstance = _monster.MonsterPlayerAgro.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            if (currentTarget.InstanceId != currentInstance)
            {
                currentTarget = Map.ClientLookup.GetByCharacterInstanceId((uint)currentInstance).Character;
            }
            agroCheckTime = 0;
        }
        private void MonsterStop()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);//Monster ID
            res.WriteFloat(_monster.X);
            res.WriteFloat(_monster.Y);
            res.WriteFloat(_monster.Z);
            res.WriteFloat(0);       //X per tick
            res.WriteFloat(0);       //Y Per tick
            res.WriteFloat(0);              //verticalMovementSpeedMultiplier

            res.WriteFloat((float)1);              //movementMultiplier
            res.WriteFloat((float)1);              //Seconds to move

            res.WriteByte(0); //MOVEMENT ANIM
            res.WriteByte(0);//JUMP & FALLING ANIM
            Router.Send(Map, (ushort)AreaPacketId.recv_0x8D92, res, ServerType.Area);
            monsterMoving = false;
            Thread.Sleep(100);
        }

        private bool checkFOV(NecClient client)
        {
            Vector2 target = new Vector2(client.Character.X, client.Character.Y);
            Vector2 source = new Vector2(_monster.X, _monster.Y);
            Vector2 targetVector = Vector2.Normalize(source - target);
            double sourceRadian = ConvertToRadians(_monster.Heading);
            Vector2 sourceVector = new Vector2((float)Math.Cos(sourceRadian), (float)Math.Sin(sourceRadian));
            sourceVector = Vector2.Normalize(sourceVector);
            float dotProduct = Vector2.Dot(sourceVector, targetVector);
            //Logger.Debug($"sourceVector.X[{sourceVector.X}] sourceVector.Y[{sourceVector.Y}]");
            if (dotProduct > fovAngle)
                Logger.Debug($"Monster {_monster.Name} sees you!!");
            //else
            //Logger.Debug($"Monster is oblivious dotProduct [{dotProduct}] fovAngle [{fovAngle}]");
            return dotProduct > fovAngle;
        }
        private double ConvertToRadians(double angle)
        {
            angle = angle * 2;
            angle = (angle <= 90 ? angle + 270 : angle - 90);
            //direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return (Math.PI / 180) * angle;
        }

        private void orientMonster()
        {
            if (_monster.MonsterAgro)
                AdjustHeading();

            _monster.MonsterOrient(Server);
        }
        private float GetDistance(Vector3 target, Vector3 source)
        {
            return Vector3.Distance(target, source);
        }

        private void ShowVectorInfo(double targetX, double targetY, double targetZ, double objectX, double objectY, double objectZ)
        {
            Vector3 target = new Vector3((float)targetX, (float)targetY, (float)targetZ);
            Vector3 source = new Vector3((float)objectX, (float)objectY, (float)objectZ);
            Vector3 moveTo = Vector3.Subtract(target, source);
            float distance = Vector3.Distance(target, source);
            double dx = objectX - targetX;
            double dy = objectY - targetY;
            double dz = objectZ - targetZ;
            Logger.Debug($"dx [{dx}]   dy[{dy}]  dz[{dz}]   distance [{distance}] moveTo.X [{moveTo.X}]  moveTo.Y [{moveTo.Y}]  moveTo.Z [{moveTo.Z}]");
        }

        private void ShowMonsterInfo()
        {
            Logger.Debug($"monster [{_monster.Name}]    X[{_monster.X}]   Y [{_monster.Y}] monster.Z [{_monster.Z}]  Heading [{_monster.Heading}]");
        }

        private double GetHeading(Vector3 destination) // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = _monster.X - destination.X;
            double dy = _monster.Y - destination.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return direction;
        }
        private void AdjustHeading()
        {
            double dx = _monster.X - currentTarget.X;
            double dy = _monster.Y - currentTarget.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            //Logger.Debug($"direction after [{direction}]");
            _monster.Heading = (byte)direction;
        }
        private bool CheckHeading() // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = _monster.X - currentTarget.X;
            double dy = _monster.Y - currentTarget.Y;
            double direction = (Math.Atan2(dy, dx) / System.Math.PI) * 180f; ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            //Logger.Debug($"direction after [{direction}]");
            return _monster.Heading == (byte)direction;
        }
        private void SendBattleReportStartNotify()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monster.InstanceId);
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res, ServerType.Area);
        }
        private void SendBattleReportEndNotify()
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res, ServerType.Area);
        }
    }
}
