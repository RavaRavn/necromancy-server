using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : ClientHandler
    {
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
            res.WriteInt32(client.Character.InstanceId); // InstanceId
            res.WriteInt32(client.Character.ClassId); // class
            res.WriteInt16(client.Character.Level); // current level
            res.WriteInt64(555555550); // current exp
            res.WriteInt64(777777712); // soul exp
            res.WriteInt64(33); // exp needed to level
            res.WriteInt64(44); // soul exp needed to level
            res.WriteInt32(client.Character.currentHp); // current hp
            res.WriteInt32(client.Character.currentMp); // current mp
            res.WriteInt32(client.Character.currentOd); // current od
            res.WriteInt32(client.Character.maxHp); // max hp
            res.WriteInt32(client.Character.maxMp); // maxmp
            res.WriteInt32(client.Character.maxOd); // max od
            res.WriteInt32(client.Character.AdventureBagGold); // current gp
            res.WriteInt32(2000000); // map gp
            res.WriteInt32(1238); // value/100 = current weight
            res.WriteInt32(1895); // value/100 = max weight
            res.WriteByte(200); // condition

            // total stat level includes bonus'?
            res.WriteInt16(24); // str
            res.WriteInt16(28); // vit
            res.WriteInt16(35); // dex
            res.WriteInt16(89); // agi
            res.WriteInt16(42); // int
            res.WriteInt16(52); // pie
            res.WriteInt16(90); // luk

            // mag atk atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            // mag def atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            //status change resistance
            res.WriteInt16(215); // fire
            res.WriteInt16(99); // water
            res.WriteInt16(88); // wind
            res.WriteInt16(455); // earth
            res.WriteInt16(333); // light
            res.WriteInt16(1222); // dark
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            // gold and alignment?
            res.WriteInt64(client.Character.AdventureBagGold); // gold
            res.WriteInt32(187); // changed nothing visably
            res.WriteInt32(600000); // lawful
            res.WriteInt32(5000); // neutral
            res.WriteInt32(4000); // chaos
            res.WriteInt32(Util.GetRandomNumber(90400101, 90400130)); // title from honor.csv

            //sub_484980
            res.WriteInt32(0); // changed nothing visably
            res.WriteInt32(0); // changed nothing visably
            res.WriteInt32(0); // changed nothing visably

            // characters stats
            res.WriteInt16(client.Character.Strength); // str
            res.WriteInt16(client.Character.vitality); // vit
            res.WriteInt16(client.Character.dexterity); // dex
            res.WriteInt16(client.Character.agility); // agi
            res.WriteInt16(client.Character.intelligence); // int
            res.WriteInt16(client.Character.piety); // pie
            res.WriteInt16(client.Character.luck); // luk

            // nothing
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably


            // nothing
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            // nothing
            res.WriteInt16(51); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(10); // changed nothing visably
            res.WriteInt16(25); // changed nothing visably
            res.WriteInt16(87); // changed nothing visably
            res.WriteInt16(122); // changed nothing visably
            res.WriteInt16(14); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably
            res.WriteInt16(73); // changed nothing visably
            res.WriteInt16(69); // changed nothing visably


            //sub_484B00 map ip and connection
            res.WriteInt32(client.Character.MapId); //MapSerialID
            res.WriteInt32(client.Character.MapId); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            //sub_484420 // Map Spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(15); //view offset

            //sub_read_int32 skill point
            res.WriteInt32(101); // skill point

            //sub_483420 character state like alive/dead/invis
            res.WriteInt32(0); //-254 GM

            //sub_494AC0
            res.WriteByte(78); // soul level
            res.WriteInt32(22); // current soul points
            res.WriteInt32(29); // changed nothing visably
            res.WriteInt32(12); // max soul points
            res.WriteByte(0); // 0 is white,1 yellow 2 red 3+ skull
            res.WriteByte(0); //Bool
            res.WriteByte(0); // changed nothing visably
            res.WriteByte(0); // changed nothing visably
            res.WriteByte(0); // changed nothing visably
            res.WriteByte(0); // changed nothing visably

            //sub_read_3-int16 unknown
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably
            res.WriteInt16(0); // changed nothing visably

            //sub_4833D0
            res.WriteInt64(0); // changed nothing visably

            //sub_4833D0
            res.WriteInt64(0); // changed nothing visably

            //sub_4834A0
            res.WriteFixedString("", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString("", 385); //Comment

            //sub_494890
            res.WriteByte(1); //Bool

            //sub_4834A0
            res.WriteFixedString("aaaa", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            //sub_483420
            int numEntries = 19;
            res.WriteInt32(numEntries); //has to be less than 19(defines how many int32s to read?)

            //Consolidated Frequently Used Code
            LoadEquip.SlotSetup(res, client.Character);


            //sub_483420
            res.WriteInt32(numEntries); //has to be less than 19

            //Consolidated Frequently Used Code
            LoadEquip.EquipItems(res, client.Character);

            //sub_483420
            res.WriteInt32(numEntries);

            LoadEquip.EquipSlotBitMask(res, client.Character);

            //sub_483420
            numEntries = 128;
            res.WriteInt32(numEntries); //has to be less than 128

            //sub_485A70
            for (int imac = 0; imac < numEntries; imac++) //status buffs / debuffs
            {
                res.WriteInt32(15100901); //[eax]:&L"i.dllext-ms-mf-pal-l2-1-0"
                res.WriteInt32(15100901);
                res.WriteInt32(15100901);
            }

            Router.Send(client, (ushort) AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);
        }
    }
}
