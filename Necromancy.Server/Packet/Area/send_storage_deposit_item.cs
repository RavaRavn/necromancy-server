using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_storage_deposit_item : ClientHandler
    {
        public send_storage_deposit_item(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_storage_deposit_item;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_storage_deposit_item2_r, res, ServerType.Area);
        }

    }
}