using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Response
{
    public class RecvDataNotifyMonsterData : PacketResponse
    {
        private MonsterSpawn _monsterSpawn;

        public RecvDataNotifyMonsterData(MonsterSpawn monsterSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_monster_data, ServerType.Area)
        {
            _monsterSpawn = monsterSpawn;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_monsterSpawn.InstanceId);
            res.WriteCString(_monsterSpawn.Name);
            res.WriteCString(_monsterSpawn.Title);
            res.WriteFloat(_monsterSpawn.X);
            res.WriteFloat(_monsterSpawn.Y);
            res.WriteFloat(_monsterSpawn.Z);
            res.WriteByte(_monsterSpawn.Heading);
            res.WriteInt32(_monsterSpawn.MonsterId); //Monster Serial ID
            res.WriteInt32(_monsterSpawn.ModelId); //Monster Model ID
            res.WriteInt16(_monsterSpawn.Size);
            res.WriteInt32(0x10); // cmp to 0x10 = 16
            int numEntries = 0x10;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(1);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16
            int numEntries2 = 0x10;
            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteInt32(1); // this was an x2 loop (i broke it down)
            }

            for (int i = 0; i < numEntries2; i++)
            {
                res.WriteByte(1);
                res.WriteByte(2);
                res.WriteByte(3);
                res.WriteInt32(0);
                res.WriteByte(4);
                res.WriteByte(5);
                res.WriteByte(6);
                res.WriteByte(1);
                res.WriteByte(2);
                res.WriteByte(1); // bool
                res.WriteByte(3);
                res.WriteByte(4);
                res.WriteByte(5);
                res.WriteByte(6);
                res.WriteByte(7);
            }

            res.WriteInt32(0x10); // cmp to 0x10 = 16
            int numEntries3 = 0x10; //equipment slots to display?
            for (int i = 0; i < numEntries3; i++)
            {
                res.WriteInt64(100);
            }
            
            if (_monsterSpawn.CurrentHp <=0)
            {
                res.WriteInt32(1); //1000 0000 here makes it stand up and not be dead.   or 0 = alive, 1 = dead
            }
            else
            {
                res.WriteInt32(11110000); //1000 0000 here makes it stand up and not be dead.   or 0 = alive, 1 = dead
            }
            res.WriteInt64(111111); // item Id ?
            res.WriteInt64(111112); // item Id ?
            res.WriteInt64(111113); // item Id ?
            res.WriteByte(90);
            res.WriteByte(100);
            res.WriteInt32(_monsterSpawn.CurrentHp); //Current HP
            res.WriteInt32(_monsterSpawn.MaxHp); //Max HP
            res.WriteInt32(0x80); // cmp to 0x80 = 128
            int numEntries4 = 0x80; //Statuses?
            for (int i = 0; i < numEntries4; i++)
            {
                res.WriteInt32(900102); // ID ?
                res.WriteInt32(1);
                res.WriteInt32(0);
            }

            return res;
        }
    }
}
