namespace Necromancy.Server.Chat
{
    public class ChatMessage
    {
        public ChatMessage(ChatMessageType messageType, string recipientSoulName, string message)
        {
            MessageType = messageType;
            RecipientSoulName = recipientSoulName;
            Message = message;
        }

        public ChatMessageType MessageType { get; }
        public string RecipientSoulName { get; }
        public string Message { get; }
    }
}
