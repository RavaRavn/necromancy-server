using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //updates and items state
    public class SendItemUpdateState : ServerChatCommand
    {
        public SendItemUpdateState(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //recv_item_update_state = 0x3247, 
            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(300000); //ItemID
            res.WriteInt32(200000); //Icon type, [x]00000 = certain armors, 1 = orb? 2 = helmet, up to 6

            Router.Send(client, (ushort) AreaPacketId.recv_item_update_state, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "upit";
    }
}
