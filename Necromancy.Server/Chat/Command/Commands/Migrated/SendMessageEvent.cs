using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //Event Message at bottom of screen
    public class SendMessageEvent : ServerChatCommand
    {
        public SendMessageEvent(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 1 = cinematic
            res.WriteByte(0);

            Router.Send(client, (ushort) AreaPacketId.recv_event_start, res, ServerType.Area);


            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0);
            res2.WriteCString("Wake up Samurai we have a city to burn"); // find max size   show the text of the message
            Router.Send(client, (ushort) AreaPacketId.recv_event_message, res2, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "mess";
    }
}
