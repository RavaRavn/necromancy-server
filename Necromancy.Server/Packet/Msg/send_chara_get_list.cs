using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_list : ClientHandler
    {
        public send_chara_get_list(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_get_list;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0xFFFFFFFF);
            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_list_r, res, ServerType.Msg);
            SendNotifyData(client);
            SendNotifyDataComplete(client);
        }

        private void SendNotifyDataComplete(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteByte(0xFF);
            res2.WriteInt32(0xFFFFFFFF);
            res2.WriteInt32(0xFFF);
            res2.WriteInt32(0xFFFFFFFF);
            Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data_complete, res2, ServerType.Msg);
        }
        
        private void SendNotifyData(NecClient client)
        {
            List<Character> characters = Database.SelectCharactersBySoulId(client.Soul.Id);
            if (characters == null || characters.Count <= 0)
            {
                Logger.Debug(client, "No characters found");
                return;
            }

            foreach (Character character in characters)
            {
                IBuffer res = BufferProvider.Provide();

                res.WriteByte(character.Slot); //character slot, 0 for left, 1 for middle, 2 for right
                res.WriteInt32(character.Id); //  Character ID
                res.WriteFixedString(character.Name, 91); // 0x5B | 91x 1 byte

                res.WriteInt32(0); // 0 = Alive | 1 = Dead
                res.WriteInt32(character.Level); //character level stat
                res.WriteInt32(1); //todo (unknown)
                res.WriteInt32(character.ClassId); //class stat 

                //Consolidated Frequently Used Code
                LoadEquip.BasicTraits(res, character);
                LoadEquip.SlotSetup(res, character);
                LoadEquip.EquipItems(res, character);
                LoadEquip.EquipSlotBitMask(res, character);
                
                //19x 4 byte //item quality(+#) or aura? 10 = +7, 19 = +6,(maybe just wep aura)
                res.WriteInt32(10); //Right Hand    //1 for weapon
                res.WriteInt32(10); //Left Hand     //2 for Shield
                res.WriteInt32(10); //Torso         //16 for torso
                res.WriteInt32(10); //Head          //08 for head
                res.WriteInt32(10); //Legs          //32 for legs
                res.WriteInt32(10); //Arms          //64 for Arms
                res.WriteInt32(10); //Feet          //128 for feet
                res.WriteInt32(004); //???Cape
                res.WriteInt32(0); //???Ring
                res.WriteInt32(0); //???Earring
                res.WriteInt32(0); //???Necklace
                res.WriteInt32(0); //???Belt
                res.WriteInt32(10); //Avatar Torso
                res.WriteInt32(10); //Avatar Feet
                res.WriteInt32(10); //Avatar Arms
                res.WriteInt32(10); //Avatar Legs
                res.WriteInt32(10); //Avatar Head  
                res.WriteInt32(10); //???Talk Ring
                res.WriteInt32(00); //???Quiver    
                res.WriteByte(19); //Number of equipment to display
                res.WriteInt32(character.MapId); //map location ID
                Router.Send(client, (ushort) MsgPacketId.recv_chara_notify_data, res, ServerType.Msg);
            }
        }
    }
}
