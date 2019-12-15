using System;
using Arrowgene.Services.Logging;
using Necromancy.Server.Chat;

namespace Necromancy.Server.Packet.Area.SendChatPostMessage
{
    public class SendChatPostMessageDeserializer : IPacketDeserializer<ChatMessage>
    {
        private ILogger _logger;

        public SendChatPostMessageDeserializer()
        {
            _logger = LogProvider.Logger(this);
        }

        public ChatMessage Deserialize(NecPacket packet)
        {
            int messageTypeValue = packet.Data.ReadInt32();
            if (!Enum.IsDefined(typeof(ChatMessageType), messageTypeValue))
            {
                _logger.Error($"ChatMessageType: {messageTypeValue} not defined");
                return null;
            }

            ChatMessageType messageType = (ChatMessageType) messageTypeValue;
            string recipient = packet.Data.ReadCString();
            string message = packet.Data.ReadCString();

            return new ChatMessage(messageType, recipient, message);
        }
    }
}
