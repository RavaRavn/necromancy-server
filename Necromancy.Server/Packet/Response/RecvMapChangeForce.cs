using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvMapChangeForce : PacketResponse
    {
        private readonly Map _map;

        public RecvMapChangeForce(Map map)
            : base((ushort) AreaPacketId.recv_map_change_force, ServerType.Area)
        {
            _map = map;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_map.Id); //MapSerialID
            res.WriteInt32(_map.Id); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port
            res.WriteFloat(_map.X); //X Pos
            res.WriteFloat(_map.Y); //Y Pos
            res.WriteFloat(_map.Z); //Z Pos
            res.WriteByte((byte)_map.Orientation); //View offset
            return res;
        }
    }
}
