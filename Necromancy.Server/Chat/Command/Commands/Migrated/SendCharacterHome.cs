using System;
using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Response;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendCharacterHome : ServerChatCommand
    {
        public SendCharacterHome(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            Character character = Server.Database.SelectCharacterById(client.Character.Id);
            Server.Maps.TryGet(character.MapId, out Map map);
            map.X = (int)character.X;
            map.Y = (int)character.Y;
            map.Z = (int)character.Z;
            map.Enter(client);
            Server.Router.Send(new RecvMapChangeForce(map), client);
        }
        
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "CharHome";
    }
}
