using System;
using System.Diagnostics;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Model
{
    [DebuggerDisplay("{Identity,nq}")]
    public class NecClient
    {
        private readonly NecLogger _logger;

        public NecClient()
        {
            _logger = LogProvider.Logger<NecLogger>(this);
            Creation = DateTime.Now;
        }

        public DateTime Creation { get; }
        public string Identity { get; private set; }
        public Account Account { get; set; }
        public Soul Soul { get; set; }
        public Character Character { get; set; }
        public Channel Channel { get; set; }
        public Map Map { get; set; }
        public NecConnection AuthConnection { get; set; }
        public NecConnection MsgConnection { get; set; }
        public NecConnection AreaConnection { get; set; }

        public void Send(NecPacket packet, ServerType serverType)
        {
            switch (serverType)
            {
                case ServerType.Area:
                    AreaConnection.Send(packet);
                    break;
                case ServerType.Msg:
                    MsgConnection.Send(packet);
                    break;
                case ServerType.Auth:
                    AuthConnection.Send(packet);
                    break;
                default:
                    _logger.Error(this, "Invalid ServerType");
                    break;
            }
        }

        public void Close()
        {
            AuthConnection?.Socket.Close();
            MsgConnection?.Socket.Close();
            AreaConnection?.Socket.Close();
        }
    }
}