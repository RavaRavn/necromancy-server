using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Auth
{
    public class send_base_check_version_auth : ConnectionHandler
    {
        public send_base_check_version_auth(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AuthPacketId.send_base_check_version;

        public override void Handle(NecConnection connection, NecPacket packet)
        {
            uint major = packet.Data.ReadUInt32();
            uint minor = packet.Data.ReadUInt32();
            Logger.Info($"{major} - {minor}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(major);
            res.WriteInt32(minor);

            Router.Send(connection, (ushort) AuthPacketId.recv_base_check_version_r, res);
        }
    }
}