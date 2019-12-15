using System;
using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendMapMove : ServerChatCommand
    {
        public SendMapMove(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            string entry = command[0];
            Server.Maps.TryGet(client.Character.MapId, out Map map);
            if (entry.Contains(":"))
            {
                string[] keyValue = entry.Split(":");
                if (keyValue.Length != 4)
                {
                    return;
                }
                int X = Convert.ToInt32(keyValue[0]);
                int Y = Convert.ToInt32(keyValue[1]);
                int Z = Convert.ToInt32(keyValue[2]);
                int Orietation = Convert.ToInt32(keyValue[3]);
                map.X = X;
                map.Y = Y;
                map.Z = Z;
                map.Orientation = Orietation;
                map.EnterForce(client);
            }
        }
        
        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "MapMove";
    }
}
