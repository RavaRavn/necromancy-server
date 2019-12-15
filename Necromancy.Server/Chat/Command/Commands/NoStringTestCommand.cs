using System.Collections.Generic;
using Necromancy.Server.Model;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Moves character x units upward.
    /// </summary>
    public class NoStringTestCommand : ServerChatCommand
    {
        public NoStringTestCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!int.TryParse(command[0], out int x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            IBuffer res = BufferProvider.Provide();

	        res.WriteInt32(client.Character.Id);
            res.WriteInt32(x);

            Router.Send(client.Map, 0x1489, res, ServerType.Area);

        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "nstest";
        public override string HelpText => "usage: `/nstest` - Quickly test a non string protocol.";
    }
}
