using System.Collections.Generic;
using System.Threading;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //Equips your whole character with gear based on settings in LoadEquipment.cs
    public class AdminConsoleRecvItemInstanceUnidentified : ServerChatCommand
    {
        public AdminConsoleRecvItemInstanceUnidentified(NecServer server) : base(server)
        {
        }

        private int x;

        int[] EquipBitMask = new int[]
        {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
            1048576, 2097152
        };

        int[] itemIDs = new int[]
        {
            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/, 210701 /*Torso*/,
            360103 /*Pants*/, 401201 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 160801 /*Avatar Head */,
            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 560801 /*Avatar Feet*/, 1, 2, 3
        };

        int[] EquipItemType = new int[]
            {9, 20, 23, 28, 31, 32, 36, 40, 41, 44, 43, 45, 42, 54, 61, 61, 61, 61, 61, 61, 0, 0};

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            int i = 0;
            x = -1;
            for (i = 0; i < 19; i++)
            {
                x++;
                Thread.Sleep(100);
                //recv_item_instance_unidentified = 0xD57A,
                IBuffer res = BufferProvider.Provide();

                res.WriteInt64(client.Character.EquipId[x]);

                res.WriteCString($"ID:{itemIDs[x]} MSK:{EquipBitMask[x]} Type:{EquipItemType[x]} lvl"); // Item Name

                res.WriteInt32(EquipItemType[x] -
                               1); // Item Type. Refer To ItemType.csv   // This controls Item Type.  61 ( minus 1) makes everything Type "Avatar"
                res.WriteInt32(EquipBitMask[x]); //Slot Limiting Bitmask.  Limits  Slot Item can be Equiped.

                res.WriteByte(1); // Numbers of items

                res.WriteInt32(
                    EquipBitMask[
                        Util.GetRandomNumber(4,
                            4)]); /* 10001003 Put The Item Unidentified. 0 put the item Identified 1-2-4-8-16 follow this patterns (8 cursed, 16 blessed)*/


                res.WriteInt32(client.Character.EquipId[x]); //Item ID for Icon
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt32(1);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(1); // bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
                res.WriteByte(0); // 0~2
                res.WriteInt16((short) x); // bag index 0 to 24

                res.WriteInt32(EquipBitMask[x]); //bit mask. This indicates where to put items.  

                res.WriteInt64(client.Character.EquipId[x]);

                res.WriteInt32(1);


                Router.Send(client, (ushort) AreaPacketId.recv_item_instance_unidentified, res, ServerType.Area);


                IBuffer res0 = BufferProvider.Provide();
                res0.WriteInt64(client.Character.EquipId[x]);
                res0.WriteInt32(Util.GetRandomNumber(199, 200)); // MaxDura points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_maxdur, res0, ServerType.Area);


                IBuffer res2 = BufferProvider.Provide(); // Maybe not the good one ?
                res2.WriteInt64(client.Character.EquipId[x]);
                res2.WriteInt32(Util.GetRandomNumber(1, 200)); // Durability points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_durability, res2, ServerType.Area);


                IBuffer res4 = BufferProvider.Provide();
                res4.WriteInt64(client.Character.EquipId[x]);
                res4.WriteInt32(Util.GetRandomNumber(800, 10000)); // Weight points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_weight, res4, ServerType.Area);


                IBuffer res5 = BufferProvider.Provide();
                res5.WriteInt64(client.Character.EquipId[x]);
                res5.WriteInt16((short) Util.GetRandomNumber(5, 500)); // Defense and attack points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_physics, res5, ServerType.Area);


                IBuffer res6 = BufferProvider.Provide();
                res6.WriteInt64(client.Character.EquipId[x]);
                res6.WriteInt16((short) Util.GetRandomNumber(5, 500)); // Magic def and attack Points
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_magic, res6, ServerType.Area);


                IBuffer res7 = BufferProvider.Provide();
                res7.WriteInt64(client.Character.EquipId[x]);
                res7.WriteInt32(Util.GetRandomNumber(1, 10)); // for the moment i don't know what it change
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_enchantid, res7, ServerType.Area);


                IBuffer res8 = BufferProvider.Provide();
                res8.WriteInt64(client.Character.EquipId[x]);
                res8.WriteInt16((short) Util.GetRandomNumber(0, 100000)); // Shwo GP on certain items
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_ac, res8, ServerType.Area);

                IBuffer res9 = BufferProvider.Provide();
                res9.WriteInt64(client.Character.EquipId[x]);
                res9.WriteInt32(Util.GetRandomNumber(1, 50)); // for the moment i don't know what it change
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_date_end_protect, res9, ServerType.Area);


                IBuffer res11 = BufferProvider.Provide();
                res11.WriteInt64(client.Character.EquipId[x]);
                res11.WriteByte((byte) Util.GetRandomNumber(0, 100)); // Hardness
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_hardness, res11, ServerType.Area);


                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt64(client.Character
                    .EquipId[x]); //client.Character.EquipId[x]   put stuff unidentified and get the status equipped  , 0 put stuff identified
                res1.WriteInt32(0);
                Router.Send(client, (ushort) AreaPacketId.recv_item_update_state, res1, ServerType.Area);


                IBuffer res13 = BufferProvider.Provide();
                //95 torso ?
                //55 full armor too ?
                //93 full armor ?
                // 27 full armor ?
                //11 under ?
                // 38 = boots and cape
                //byte y = unchecked((byte)110111);
                //byte y = unchecked ((byte)Util.GetRandomNumber(0, 100)); // for the moment i only get the armor on this way :/

                res13.WriteInt64(client.Character.EquipId[x]);
                res13.WriteInt32(EquipBitMask[x]); // Permit to get the armor on the chara

                res13.WriteInt32(client.Character.EquipId[x]); // List of items that gonna be equip on the chara
                res13.WriteByte(0); // ?? when you change this the armor dissapear, apparently
                res13.WriteByte(0);
                res13.WriteByte(0); //need to find the right number, permit to get the armor on the chara

                res13.WriteInt32(1);
                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0);

                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0); //bool
                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0);
                res13.WriteByte(0); // 1 = body pink texture
                res13.WriteByte(0);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_item_update_eqmask, res13, ServerType.Area);


                IBuffer res17 = BufferProvider.Provide();
                res17.WriteInt64(itemIDs[x]);
                res17.WriteInt32(EquipBitMask[x]);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_item_update_spirit_eqmask, res17, ServerType.Area,
                    client);


                /*IBuffer res19 = BufferProvider.Provide();
                res19.WriteInt32(itemIDs[x]);
                res19.WriteInt32(EquipBitMask[x]);

                int numEntries = 0x2;
                for (i = 0; i < numEntries; i++)
                {
                    res19.WriteInt32(0);
                    res19.WriteByte(0);
                    res19.WriteByte(0);
                    res19.WriteByte(0);

                }

                res19.WriteByte(0);

                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);
                res19.WriteByte(0);

                res19.WriteByte(0);

                res19.WriteInt32(EquipBitMask[x]);
                Router.Send(client.Map, (ushort)AreaPacketId.recv_dbg_chara_equipped, res19, ServerType.Area);*/
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "GetUItem";
    }
}
