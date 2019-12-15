using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //union Cape design window
    public class SendUnionMantleOpen : ServerChatCommand
    {
        public SendUnionMantleOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            for (int i = 0; i < 0x10; i++)
            {
                res.WriteByte(0);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_union_mantle_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "mant";
    }
}
