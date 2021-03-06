using System.Collections.Generic;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Receive;

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
            Character character = Server.Database.SelectCharacterById((int) client.Character.InstanceId);
            Server.Maps.TryGet(character.MapId, out Map map);
            MapPosition mapPos = new MapPosition(character.X, character.Y, character.Z, character.Heading);
            map.EnterForce(client, mapPos);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "CharHome";
    }
}
