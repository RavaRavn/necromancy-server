using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : ClientHandler
    {
        private static readonly NecLogger Logger =
            LogProvider.Logger<NecLogger>(typeof(send_data_get_self_chara_data_request));

        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            SendDataGetSelfCharaData(client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_request_r, res2, ServerType.Area);
        }

        private void SendDataGetSelfCharaData(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();

            //sub_4953B0 - characteristics
            //Consolidated Frequently Used Code
            LoadEquip.BasicTraits(res, client.Character);

            //sub_484720 - combat/leveling info
            Logger.Debug($"Character ID Loading : {client.Character.Id}");
            res.WriteUInt32(client.Character.InstanceId); // InstanceId
            res.WriteUInt32(client.Character.ClassId); // class
            res.WriteInt16(client.Character.Level); // current level //+50 Temporary client.Character.Level
            res.WriteInt64(91978348); // current exp
            res.WriteInt64(50); // soul exp
            res.WriteInt64(96978348); // exp needed to level
            res.WriteInt64(1100); // soul exp needed to level
            res.WriteInt32(client.Character.Hp.current); // current hp
            res.WriteInt32(client.Character.Mp.current); // current mp
            res.WriteInt32(client.Character.Od.current); // current od
            res.WriteInt32(client.Character.Hp.max); // max hp
            res.WriteInt32(client.Character.Mp.max); // maxmp
            res.WriteInt32(client.Character.Od.max); // max od
            res.WriteInt32(500); // current guard points
            res.WriteInt32(600); // max guard points
            res.WriteInt32(1238); // value/100 = current weight
            res.WriteInt32(1895); // value/100 = max weight
            res.WriteByte(200); // condition

            // total stat level includes bonus'?
            res.WriteUInt16(client.Character.Strength); // str
            res.WriteUInt16(client.Character.vitality); // vit
            res.WriteInt16((short) (client.Character.dexterity + 3)); // dex
            res.WriteUInt16(client.Character.agility); // agi
            res.WriteUInt16(client.Character.intelligence); // int
            res.WriteUInt16(client.Character.piety); // pie
            res.WriteInt16((short) (client.Character.luck + 4)); // luk

            // mag atk atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);

            // mag def atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);

            //status change resistance
            res.WriteInt16(11); // Poison
            res.WriteInt16(12); // Paralyze
            res.WriteInt16(13); // Stone
            res.WriteInt16(14); // Faint
            res.WriteInt16(15); // Blind
            res.WriteInt16(16); // Sleep
            res.WriteInt16(17); // Silent
            res.WriteInt16(18); // Charm
            res.WriteInt16(19); // confus
            res.WriteInt16(20); // fear
            res.WriteInt16(21); //possibly EXP Boost Gauge. trying to find it

            // gold and alignment?
            res.WriteInt64(client.Character.AdventureBagGold); // gold
            res.WriteUInt32(client.Character.Alignmentid); // AlignmentId
            res.WriteInt32(6000); // lawful
            res.WriteInt32(5000); // neutral
            res.WriteInt32(6100); // chaos
            res.WriteInt32(Util.GetRandomNumber(90400101, 90400130)); // title from honor.csv

            //sub_484980
            res.WriteInt32(1); // ac eval calculation?
            res.WriteInt32(1); // ac eval calculation?
            res.WriteInt32(1); // ac eval calculation?

            // characters stats
            res.WriteUInt16(client.Character.Strength); // str
            res.WriteUInt16(client.Character.vitality); // vit
            res.WriteInt16((short) (client.Character.dexterity)); // dex
            res.WriteUInt16(client.Character.agility); // agi
            res.WriteUInt16(client.Character.intelligence); // int
            res.WriteUInt16(client.Character.piety); // pie
            res.WriteInt16((short) (client.Character.luck)); // luk

            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);


            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);

            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);
            res.WriteInt16(10);
            res.WriteInt16(11);


            //sub_484B00 map ip and connection
            res.WriteInt32(client.Character.MapId); //MapSerialID
            res.WriteInt32(client.Character.MapId); //MapID
            res.WriteFixedString(Settings.DataAreaIpAddress, 65); //IP
            res.WriteUInt16(Settings.AreaPort); //Port

            //sub_484420 // Map Spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.Heading); //view offset

            //sub_read_int32 skill point
            res.WriteInt32(101); // skill point

            //sub_483420 character state like alive/dead/invis
            res.WriteInt32(0); //-254 GM

            //sub_494AC0
            res.WriteByte(20); // soul level
            res.WriteInt32(22); // current soul points
            res.WriteInt32(790); // soul point bar value (percenage of current/max)
            res.WriteInt32(120); // max soul points
            res.WriteByte(0); // 0 is white,1 yellow 2 red 3+ skull
            res.WriteByte(0); //Beginner protection (bool)
            res.WriteByte(50); //Level cap
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            //sub_read_3-int16 unknown
            res.WriteInt16(50); // HP Consumption Rate?
            res.WriteInt16(50); // MP Consumption Rate?
            res.WriteInt16(50); // OD Consumption Rate (if greater than currentOD, Can not sprint)

            //sub_4833D0
            res.WriteInt64(0);

            //sub_4833D0
            res.WriteInt64(0);

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Shop", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Comment", 385); //Comment

            //sub_494890
            res.WriteByte(0); //Bool 0 off 1 on

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} chatbox?", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            //sub_483420
            int numEntries = 19;
            res.WriteInt32(numEntries); //has to be less than 19(defines how many int32s to read?)

            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res, client.Character, numEntries);


            //sub_483420
            res.WriteInt32(numEntries); //has to be less than 19

            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res, client.Character, numEntries);

            //sub_483420
            res.WriteInt32(numEntries);

            LoadEquip.EquipSlotBitMask(res, client.Character, numEntries);

            //sub_483420
            numEntries = 128;
            res.WriteInt32(numEntries); //has to be less than 128

            //sub_485A70
            for (int k = 0; k < numEntries; k++) //status buffs / debuffs
            {
                res.WriteInt32(0); //set to k
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);
        }
    }
}
