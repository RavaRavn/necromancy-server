using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //Adds an Identified item to inventory
    public class SendItemInstance : ServerChatCommand
    {
        public SendItemInstance(NecServer server) : base(server)
        {
        }

        int x = 0;

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //recv_item_instance = 0x86EA,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt64(69); //ItemID
            res.WriteInt32((int) x); //Icon type, [x]00000 = certain armors, 1 = orb? 2 = helmet, up to 6
            res.WriteByte(0); //Number of "items"
            res.WriteInt32(0); //Item status, in multiples of numbers, 8 = blessed/cursed/both 
            res.WriteFixedString("fixed", 0x10);
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment
            res.WriteByte(0); // 0~2 // maybe.. more bag index?
            res.WriteInt16(1); // bag index
            res.WriteInt32(0); //Slot spots? 10200101 here caused certain spots to have an item, -1 for all slots(avatar included)
            res.WriteInt32(1); //Percentage stat, 9 max i think
            res.WriteByte(1);
            res.WriteByte(3);
            res.WriteCString("cstring"); // find max size 
            res.WriteInt16(2);
            res.WriteInt16(1);
            res.WriteInt32(1); //Divides max % by this number
            res.WriteByte(1);
            res.WriteInt32(0);
            int numEntries = 2;
            res.WriteInt32(numEntries); // less than or equal to 2

            //for (int i = 0; i < numEntries; i++)
            res.WriteInt32(0);
            res.WriteInt32(0);

            numEntries = 3;
            res.WriteInt32(numEntries); // less than or equal to 3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0); //bool
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(0); //Guard protection toggle, 1 = on, everything else is off
            res.WriteInt16(0);

            Router.Send(client, (ushort) AreaPacketId.recv_item_instance, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "itis";
    }
}
