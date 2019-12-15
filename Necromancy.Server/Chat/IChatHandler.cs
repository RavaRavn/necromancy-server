using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat
{
    public interface IChatHandler
    {
        void Handle(NecClient client, ChatMessage message, ChatResponse response, List<ChatResponse> responses);
    }
}
