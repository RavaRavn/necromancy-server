using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //puts your character ID in chat
    public class SendCharacterId : ServerChatCommand
    {
        public SendCharacterId(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            string chid = client.Character.Id.ToString();
            SendChatNotifyMessage(client, chid, 0);
        }

        private void SendChatNotifyMessage(NecClient client, string Message, int ChatType)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(ChatType);
            res.WriteInt32(client.Character.Id);
            res.WriteFixedString($"{client.Soul.Name}", 49);
            res.WriteFixedString($"{client.Character.Name}", 37);
            res.WriteFixedString($"{Message}", 769);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_chat_notify_message, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "chid";
    }
}
