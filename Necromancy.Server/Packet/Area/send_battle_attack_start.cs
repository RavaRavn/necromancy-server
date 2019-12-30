using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_attack_start : ClientHandler
    {
        public send_battle_attack_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_attack_start;

        

        public override void Handle(NecClient client, NecPacket packet)
        {

            client.Character.battleAnim = 231; //at the start of every attack, set the battle anim to 231.  231 is the 1st anim for all weapon types 
            client.Character.battleNext = 0;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.battleAttack); //0 means success
            Router.Send(client, (ushort)AreaPacketId.recv_battle_attack_start_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_attack_start, res2, ServerType.Area, client);


        }

    }
}
