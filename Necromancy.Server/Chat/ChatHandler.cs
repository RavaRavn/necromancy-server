using System.Collections.Generic;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat
{
    public abstract class ChatHandler : IChatHandler
    {
        protected ChatHandler()
        {
            Logger = LogProvider.Logger<NecLogger>(this);
        }

        protected NecLogger Logger { get; }

        public abstract void Handle(NecClient client, ChatMessage message, ChatResponse response,
            List<ChatResponse> responses);
    }
}
