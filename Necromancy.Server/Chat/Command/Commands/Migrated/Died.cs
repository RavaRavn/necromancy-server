using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //displays message that you died
    public class Died : ServerChatCommand
    {
        public Died(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort) AreaPacketId.recv_self_lost_notify, res4, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "Died";
    }
}
