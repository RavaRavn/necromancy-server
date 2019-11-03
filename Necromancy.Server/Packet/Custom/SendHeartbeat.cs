using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Custom
{
    public class SendHeartbeat : ClientHandler
    {
        public SendHeartbeat(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) CustomPacketId.SendHeartbeat;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint time = packet.Data.ReadUInt32();
            Logger.Info(client, $"Time in seconds since Client Executable Start :{(time / 1000)}  ");
        }
    }
}
