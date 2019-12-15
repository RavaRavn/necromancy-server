using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Response;
using Necromancy.Server.Tasks;
using System;
using System.Numerics;
using System.Threading;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Spawns a monster
    /// </summary>
    public class MonsterCommand : ServerChatCommand
    {
        //protected NecServer server { get; }
        public MonsterCommand(NecServer server) : base(server)
        {
            //this.server = server;
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            MonsterSpawn monsterSpawn = Server.Instances.CreateInstance<MonsterSpawn>();
            if (!int.TryParse(command[0], out int monsterId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!Server.SettingRepository.Monster.TryGetValue(monsterId, out MonsterSetting monsterSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid MonsterId: {monsterId}"));
                return;
            }

            if (!int.TryParse(command[1], out int modelId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                return;
            }

            if (!int.TryParse(command[2], out int monsterX))
            {
                monsterSpawn.X = 0;
                //                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                //                return;
            }
            else
            {
                monsterSpawn.X = client.Character.X + monsterX;
            }

            if (!int.TryParse(command[3], out int monsterY))
            {
                monsterSpawn.Y = 0;
                //                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                //                return;
            }
            else
            {
                monsterSpawn.Y = client.Character.Y + monsterY;
                monsterSpawn.Z = client.Character.Z;
            }

            if (!Server.SettingRepository.ModelCommon.TryGetValue(modelId, out ModelCommonSetting modelSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid ModelId: {modelId}"));
                return;
            }
            Logger.Debug($"modelSetting.Radius [{modelSetting.Radius}]");
            monsterSpawn.MonsterId = monsterSetting.Id;
            monsterSpawn.Name = monsterSetting.Name;
            monsterSpawn.Title = monsterSetting.Title;
            monsterSpawn.Level = (byte)monsterSetting.Level;

            monsterSpawn.ModelId = modelSetting.Id;
            monsterSpawn.Size = (short)modelSetting.Height;
            monsterSpawn.Radius = (short)modelSetting.Radius;

            monsterSpawn.MapId = client.Character.MapId;
            //double heading1 = GetHeading(client.Character.X, client.Character.Y, monsterSpawn.X, monsterSpawn.Y);
            if (monsterSpawn.X == 0)
            {
                monsterSpawn.X = monsterXCoords[0];
                monsterSpawn.Z = monsterZCoords[0];
            }
            if (monsterSpawn.Y == 0)
            {
                monsterSpawn.Y = monsterYCoords[0];
            }

            monsterSpawn.MaxHp = 1000;
            monsterSpawn.CurrentHp = 100;
            /*if (!Server.Database.InsertMonsterSpawn(monsterSpawn))
            {
                responses.Add(ChatResponse.CommandError(client, "MonsterSpawn could not be saved to database"));
                return;
            }   */
            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
            Router.Send(client.Map, monsterData);
            SendMonsterPose(client, monsterSpawn);
            MonsterTask monsterTask = new MonsterTask(Server, monsterSpawn);
            //Thread workerThread = new Thread(monsterThread.InstanceMethod);
            monsterTask.gotoDistance = 10;
            MonsterCoord monsterHome = new MonsterCoord();
            monsterHome.destination = new Vector3(-923, 3599, -371);
            monsterTask.monsterHome = monsterHome;
            monsterTask.Start();

        }
        private void SendBattleReportStartNotify(NecClient client, MonsterSpawn monsterSpawn)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(monsterSpawn.InstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }
        private void SendBattleReportEndNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
        }

        private void SendMonsterPose(NecClient client, MonsterSpawn monsterSpawn)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(monsterSpawn.InstanceId); // character id
            res.WriteInt32(1); // pose id
            Router.Send(client, (ushort)AreaPacketId.recv_chara_pose_notify, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "mon";
        public override string HelpText => "usage: `/mon [monsterId] [modelId]` - Spawns a Monster";

        int[] monsterXCoords = { -312, -1461, -1489, -313 };
        int[] monsterYCoords = { 4116, 4156, 3118, 3096 };
        int[] monsterZCoords = { -356, -357, -350, -358 };
        byte[] monsterHeading = { 0, 45, 90, 135};
    }
}
