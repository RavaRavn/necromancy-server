using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_battle_guard_end : ClientHandler
    {
        public send_battle_guard_end(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_battle_guard_end;

        

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);//Character ID

            Router.Send(client.Map, (ushort)AreaPacketId.recv_dbg_battle_guard_end_notify, res, ServerType.Area, client);

        }
    }
}
