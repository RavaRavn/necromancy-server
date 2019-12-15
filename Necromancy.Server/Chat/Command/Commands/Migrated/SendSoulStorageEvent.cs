using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens your storage
    public class SendSoulStorageEvent : ServerChatCommand
    {
        public SendSoulStorageEvent(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //1 = cinematic
            res.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_event_start, res, ServerType.Area);


            IBuffer res0 = BufferProvider.Provide();
            res0.WriteInt64(client.Soul.WarehouseGold); // Gold in the storage
            Router.Send(client, (ushort) AreaPacketId.recv_event_soul_storage_open, res0, ServerType.Area);


            /*  IBuffer res1 = BufferProvider.Provide();
              res1.WriteByte(1);
              Router.Send(client, (ushort)AreaPacketId.recv_event_end, res1); */
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "stor";
    }
}
