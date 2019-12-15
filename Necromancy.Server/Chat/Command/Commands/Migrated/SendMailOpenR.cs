using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens the mail window
    public class SendMailOpenR : ServerChatCommand
    {
        public SendMailOpenR(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);

            Router.Send(client, (ushort) AreaPacketId.recv_mail_open_r, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "mail";
    }
}
