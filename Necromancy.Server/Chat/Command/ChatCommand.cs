using System.Collections.Generic;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command
{
    public abstract class ChatCommand
    {
        protected readonly NecLogger Logger;

        public ChatCommand()
        {
            Logger = LogProvider.Logger<NecLogger>(this);
        }

        public abstract void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses);

        public abstract AccountStateType AccountState { get; }
        public abstract string Key { get; }
        public string KeyToLowerInvariant => Key.ToLowerInvariant();

        public virtual string HelpText
        {
            get { return null; }
        }
    }
}
