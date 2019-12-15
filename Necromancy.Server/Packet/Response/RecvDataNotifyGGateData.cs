using Arrowgene.Services.Buffers;
using Arrowgene.Services.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;


namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyGGateData : PacketResponse
    {
        private NpcSpawn _gGateSpawn;

        public RecvDataNotifyGGateData(NpcSpawn gGateSpawn)
            : base((ushort)AreaPacketId.recv_data_notify_ggate_stone_data, ServerType.Area)
        {
            _gGateSpawn = gGateSpawn;
        }

        protected override IBuffer ToBuffer()
        {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(_gGateSpawn.InstanceId);// Unique Object ID.  Crash if already in use (dont use your character ID)
                res.WriteInt32(_gGateSpawn.Id);// Serial ID for Interaction? from npc.csv????
                res.WriteByte(1);// 0 = Text, 1 = F to examine  , 2 or above nothing
                res.WriteCString($"");//"0x5B" //Name
                res.WriteCString($"");//"0x5B" //Title
                res.WriteFloat((float)_gGateSpawn.X);//X
                res.WriteFloat((float)_gGateSpawn.Y);//Y
                res.WriteFloat((float)_gGateSpawn.Z);//Z
                res.WriteByte(0);//
                res.WriteInt32(_gGateSpawn.ModelId);// Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv
                res.WriteInt16(_gGateSpawn.Size);//  size of the object
                res.WriteInt32(_gGateSpawn.Active ? 1 : 0);// 0 = collision, 1 = no collision  (active/Inactive?)
                res.WriteInt32(01);//0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx

                return res;

        }
    }
}
