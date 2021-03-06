using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Changes the map
    /// </summary>
    public class SendMapChangeForce : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SendMapChangeForce));

        public SendMapChangeForce(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0].Length == 0)
            {
                Logger.Debug("Re-entering current map");
                command[0] = $"{client.Character.MapId}";
            }

            if (!int.TryParse(command[0], out int mapId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!Server.Maps.TryGet(mapId, out Map map))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid MapId: {mapId}"));
                return;
            }

            map.EnterForce(client);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "map";
        public override string HelpText => "usage: `/map [mapId]` - Changes the map";
    }
}
