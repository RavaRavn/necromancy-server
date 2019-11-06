using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat
{
    public class ChatResponse
    {
        public static ChatResponse CommandError(NecClient client, string message)
        {
            return new ChatResponse()
            {
                Deliver = true,
                SoulName = "System",
                CharacterName = "",
                ErrorType = ChatErrorType.GenericUnknownStatement,
                Message = message,
                MessageType = ChatMessageType.TextCommandLog,
                Recipients = {client}
            };
        }


        public ChatResponse()
        {
            Recipients = new List<NecClient>();
            Deliver = true;
            ErrorType = ChatErrorType.Success;
            MessageType = ChatMessageType.Area;
        }

        public List<NecClient> Recipients { get; }
        public bool Deliver { get; set; }
        public ChatErrorType ErrorType { get; set; }
        public ChatMessageType MessageType { get; set; }
        public string SoulName { get; set; }
        public string CharacterName { get; set; }
        public string Message { get; set; }
        public int CharacterId { get; set; }
    }
}
