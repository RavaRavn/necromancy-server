using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyCharaData : PacketResponse
    {
        private readonly Character _character;
        private readonly string _soulName;

        public RecvDataNotifyCharaData(Character character, string soulName)
            : base((ushort) AreaPacketId.recv_data_notify_chara_data, ServerType.Area)
        {
            _character = character;
            _soulName = soulName;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_character.InstanceId);
            res.WriteCString(_soulName);
            res.WriteCString(_character.Name);
            res.WriteFloat(_character.X);
            res.WriteFloat(_character.Y);
            res.WriteFloat(_character.Z);
            res.WriteByte(_character.Heading);
            res.WriteInt32(6);
            res.WriteInt32(0); //Character pose? 6 = guard, 8 = invisible,
            res.WriteInt16(0);

            int numEntries = 19;
            res.WriteInt32(numEntries); //has to be less than 19(defines how many int32s to read?)
            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res, _character);
            //sub_483420
            numEntries = 19;
            res.WriteInt32(numEntries); //has to be less than 19
            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res, _character);
            //sub_483420
            numEntries = 19; //influences a loop that needs to be under 19
            res.WriteInt32(numEntries);
            //Consolidated Frequently Used Code
            LoadEquip.EquipSlotBitMask(res, _character);
            //sub_4835C0
            res.WriteInt32(0); //1 here means crouching?
            //sub_484660
            LoadEquip.BasicTraits(res, _character);
            //sub_483420
            res.WriteInt32(0); // party id?
            //sub_4837C0
            res.WriteInt32(1); // party id? // i don't think sooo'
            //sub_read_byte
            res.WriteByte(0); //Criminal name icon
            //sub_494890
            res.WriteByte(0); //Bool Beginner Protection
            //sub_4835E0
            res.WriteInt32(0); //pose, 1 = sitting, 0 = standing
            //sub_483920
            res.WriteInt32(0);
            //sub_483440
            res.WriteInt16(65);
            //sub_read_byte
            res.WriteByte(255); //no change?
            //sub_read_byte
            res.WriteByte(255); //no change?
            //sub_read_int_32
            res.WriteInt32(1); //title; 0 - display title, 1 - no title
            //sub_483580
            res.WriteInt32(244);
            //sub_483420
            numEntries = 1;
            res.WriteInt32(numEntries); //influences a loop that needs to be under 128
            //sub_485A70
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            //sub_481AA0
            res.WriteCString(""); //Comment string
            return res;
        }
    }
}
