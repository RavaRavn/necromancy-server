using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_exec : ClientHandler
    {
        public send_battle_attack_exec(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_attack_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int Unknown1 = packet.Data.ReadInt32();
            int instanceId = packet.Data.ReadInt32();
            int Unknown2 = packet.Data.ReadInt32();

            SendBattleAttackExecR(client);
            if (instanceId == 0)
            {
                return;
            }
            IInstance instance = Server.Instances.GetInstance((uint)instanceId);
            //Router.Send(client, (ushort) AreaPacketId.recv_battle_attack_exec_direct_r, res, ServerType.Area);
            SendBattleReportStartNotify(client, instance);
            SendReportNotifyHitEffect(client, instance);
            SendReportDamageHP(client, instance);
            //SendDataNotifyEoData(client, instance);
            SendBattleReportEndNotify(client, instance);
        }

        private void SendBattleReportStartNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }
        private void SendBattleReportEndNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
        }


        private void SendReportAcctionAtackExec(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_report_action_attack_exec, res4, ServerType.Area);


        }
        private void SendReportNotifyHitEffect(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(100020);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_hit_effect, res4, ServerType.Area);


        }

        private void SendReportDamageHP(NecClient client, IInstance instance)
        {
            int damage = Util.GetRandomNumber(1, 20);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(instance.InstanceId);
            res.WriteInt32(damage);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_phy_damage_hp, res, ServerType.Area);

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            res4.WriteByte((byte)damage); // % hp remaining of target.  need to store current NPC HP and OD as variables to "attack" them
            Router.Send(client.Map, (ushort)AreaPacketId.recv_object_hp_per_update_notify, res4, ServerType.Area);

        }

        private void SendBattleAteckExecDirect(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(instance.InstanceId);
            Router.Send(client, (ushort)AreaPacketId.recv_battle_attack_exec_direct_r, res4, ServerType.Area);


        }
        private void SendBattleAttackExecR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);  // If not zero next attack is allowed before first complete

            Router.Send(client, (ushort)AreaPacketId.recv_battle_attack_exec_r, res, ServerType.Area);
        }


    }
}
