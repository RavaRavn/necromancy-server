using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //puts items in your inventory
    public class AdminConsoleRecvItemInstance : ServerChatCommand
    {
        public AdminConsoleRecvItemInstance(NecServer server) : base(server)
        {
        }

        int[] EquipBitMask = new int[]
        {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
            1048576, 2097152
        };

        int[] EquipItemType = new int[]
            {9, 20, 23, 28, 31, 32, 36, 40, 41, 44, 43, 45, 42, 54, 61, 61, 61, 61, 61, 61, 0, 0};

        private int x;

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            x = 0;
            for (int j = 0; j < 19; j++)
            {
                IBuffer res = BufferProvider.Provide();
                //recv_item_instance = 0x86EA,
                x++;
                res.WriteInt64(client.Character
                    .EquipId[x]); //  Assume Unique ID instance identifier. 1 here makes item green icon
                res.WriteInt32(EquipItemType[x] - 1);
                res.WriteByte(1); //number of items in stack
                res.WriteInt32(client.Character.EquipId[x]); //
                res.WriteFixedString("WhatIsThis", 0x10);
                res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
                res.WriteByte(0); // 0~2
                res.WriteInt16((short) x); // bag index
                res.WriteInt32(EquipBitMask[x]); //bit mask. This indicates where to put items.   ??
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteCString("ThisIsThis"); // find max size 
                res.WriteInt16(0);
                res.WriteInt16(0);
                res.WriteInt32(client.Character.EquipId[x]);
                res.WriteByte(0);
                res.WriteInt32(client.Character.EquipId[x]);
                int numEntries = 2;
                res.WriteInt32(numEntries); // less than or equal to 2
                for (int i = 0; i < numEntries; i++)
                {
                    res.WriteInt32(client.Character.EquipId[x]);
                }

                numEntries = 3;
                res.WriteInt32(numEntries); // less than or equal to 3
                for (int i = 0; i < numEntries; i++)
                {
                    res.WriteByte(0); //bool
                    res.WriteInt32(client.Character.EquipId[x]);
                    res.WriteInt32(client.Character.EquipId[x]);
                    res.WriteInt32(client.Character.EquipId[x]);
                }

                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt16(0);
                res.WriteInt32(1); //1 here lables the item "Gaurd".   no effect from higher numbers
                res.WriteInt16(0);

                Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "GetItem";
    }
}
