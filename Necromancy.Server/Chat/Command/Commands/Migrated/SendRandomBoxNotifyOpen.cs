using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendRandomBoxNotifyOpen : ServerChatCommand
    {
        public SendRandomBoxNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //recv_random_box_notify_open = 0xC374,
            IBuffer res = BufferProvider.Provide();

            int numEntries = 10; // Slots
            res.WriteInt32(numEntries); //less than or equal to 10

            // Weapon
            int itemId = 10800405;

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt64(itemId); // ?
            }

            res.WriteInt32(itemId); // Show item name                                                   


            Router.Send(client, (ushort) AreaPacketId.recv_random_box_notify_open, res,
                ServerType.Area); // Trying to spawn item in this boxe, maybe i need the item instance ?

            IBuffer res1 = BufferProvider.Provide();
            res1.WriteInt64(0);
            res1.WriteInt32(itemId);
            Router.Send(client, (ushort) AreaPacketId.recv_item_update_state, res1, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "rbox";
    }
}
