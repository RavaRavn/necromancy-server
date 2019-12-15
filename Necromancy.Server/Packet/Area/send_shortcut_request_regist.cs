using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_shortcut_request_regist : ClientHandler
    {
        public send_shortcut_request_regist(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_shortcut_request_regist;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            byte shortcutBarIdx = packet.Data.ReadByte(),
                 slot = packet.Data.ReadByte();
            int actionType = packet.Data.ReadInt32();
            long skillID = packet.Data.ReadInt64();

            int shortcutBarId = -1;
            switch (shortcutBarIdx)
            {
                case 0:
                    shortcutBarId = client.Character.shortcutBar0Id;
                    break;
                case 1:
                    shortcutBarId = client.Character.shortcutBar1Id;
                    break;
                case 2:
                    shortcutBarId = client.Character.shortcutBar2Id;
                    break;
                case 3:
                    shortcutBarId = client.Character.shortcutBar3Id;
                    break;
                case 4:
                    shortcutBarId = client.Character.shortcutBar4Id;
                    break;
                default:
                    return;
            }
            ShortcutBar shortcutBar = Database.SelectShortcutBarById(shortcutBarId);
            int [] slots = shortcutBar.getSlotArray();
            slots[slot] = (int)skillID;
            shortcutBar.setSlotArray(slots);
            int[] actions = shortcutBar.getActionArray();
            actions[slot] = actionType;
            shortcutBar.setActionArray(actions);
            Database.UpdateShortcutBar(shortcutBar);

            res.WriteByte(shortcutBarIdx);
            res.WriteByte(slot);
            res.WriteInt32(actionType);
            res.WriteInt64(skillID);
            res.WriteFixedString("SkillName", 16);//size is 0x10

            Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res, ServerType.Area);
        }
    }
}
