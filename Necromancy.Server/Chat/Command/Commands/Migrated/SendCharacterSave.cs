//using System;
using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendCharacterSave : ServerChatCommand
    {
        public SendCharacterSave(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            Server.Database.UpdateCharacter(client.Character);
        }
        
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "charsave";
    }
}
