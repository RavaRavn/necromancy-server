using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_guard_start : ClientHandler
    {
        public send_battle_guard_start(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_guard_start;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);//Character ID

            Router.Send(client.Map, (ushort)AreaPacketId.recv_dbg_battle_guard_start_notify, res, ServerType.Area, client);

        }
    }
}
