using Arrowgene.Services.Logging;

namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecDeserializer : IPacketDeserializer<SendCmdExecRequest>
    {
        private ILogger _logger;

        public SendCmdExecDeserializer()
        {
            _logger = LogProvider.Logger(this);
        }

        public SendCmdExecRequest Deserialize(NecPacket packet)
        {
            string command = packet.Data.ReadCString();

            SendCmdExecRequest sendCmdExecRequest = new SendCmdExecRequest(command);
            
            int startPosition = 49;
            int blockSize = 769;
            while (startPosition + blockSize < packet.Data.Size)
            {
                packet.Data.Position = startPosition;
                string parameter = packet.Data.ReadCString();
                sendCmdExecRequest.Parameter.Add(parameter);
                packet.Data.Position = startPosition;
                startPosition += blockSize;
                // Skip 769 Unknown bytes
            }

            packet.Data.Position = 7739;
            int u = packet.Data.ReadInt32();
            int u1 = packet.Data.ReadInt32();

            return sendCmdExecRequest;
        }
    }
}
