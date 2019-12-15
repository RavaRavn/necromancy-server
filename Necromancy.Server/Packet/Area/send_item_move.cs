using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_move : ClientHandler
    {
        public send_item_move(NecServer server) : base(server)
        {
       
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_move;
        int x;
        public override void Handle(NecClient client, NecPacket packet)
        {
       
            //int unknown = packet.Data.ReadInt16();
            int fromSlot = packet.Data.ReadInt32(); // [0 = adventure bag. 1 = character equipment], [then unknown byte], [then slot], [then unknown]
            int toSlot = packet.Data.ReadInt32();
            int itemCount = packet.Data.ReadByte(); //last byte is stack count?

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //error check. 0 to work

            /*
                ITEMUSE	GENERIC	Unable to use this item right now
                ITEMUSE	-201	Store location is incorrect
                ITEMUSE	-204	Item amount is incorrect
                ITEMUSE	-205	The target to use this item is incorrect
                ITEMUSE	-206	Unable to use due to delay time
                ITEMUSE	-207	No space available in inventory
                ITEMUSE	-208	Unable to use this item since it is cursed
                ITEMUSE	-209	Unable to use this item since it is broken
                ITEMUSE	-210	You do not meet the requirements to equip this item
                ITEMUSE	-211	Unable to use this item
                ITEMUSE	-212	You are not in the right status to use this item
                ITEMUSE	-230	Unable to use since it is on cool down.
                ITEMUSE	-2601	You've already received this scrap
                ITEMUSE	-2708	Cannot be used outside of town
                ITEMUSE	-3001	Unable to move items when you have a shop open

            */

            Router.Send(client, (ushort) AreaPacketId.recv_item_move_r, res, ServerType.Area);
            SendItemPlace(client);
            SendItemPlaceChange(client);
        }
        private void SendItemPlace(NecClient client)
        {
            x = -1;
            x++;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(itemIDs[x]); // item id

            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment, 2 = royal bag
            res.WriteByte(0); // position 2	cause crash if you change the 0	]	} im assumming these are x/y row, and page
            res.WriteInt16((short)23); // bag index 0 to 24
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_place, res, ServerType.Area);
        }
        private void SendItemPlaceChange(NecClient client)
        {
            x = -1;
            x++;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(itemIDs[x]); // item id
            res.WriteByte(0);// 0 = adventure bag. 1 = character equipment, 2 = royal bag ??
            res.WriteByte(0); // Position 2 ??
            res.WriteInt16((short)23); // bag index 0 to 24
            res.WriteInt64(0); // item id
            res.WriteByte(0); // 0 = adventure bag. 1 = character equipment, 2 = royal bag ??
            res.WriteByte(0); // Position 2 ??
            res.WriteInt16((short)23); // bag index 0 to 24
            Router.Send(client, (ushort)AreaPacketId.recv_item_update_place_change, res, ServerType.Area);
        }

        int[] itemIDs = new int[]
       {
            10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/, 210701 /*Torso*/,
            360103 /*Pants*/, 401201 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
            30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 160801 /*Avatar Head */,
            260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 560801 /*Avatar Feet*/, 1, 2, 3
       };

        int[] EquipStatus = new int[] { 0, 1, 2, 4, 8, 16 };
    }
}