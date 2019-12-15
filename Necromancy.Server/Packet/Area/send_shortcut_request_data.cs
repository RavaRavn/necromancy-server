using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_shortcut_request_data : ClientHandler
    {
        public send_shortcut_request_data(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_shortcut_request_data;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_shortcut_request_data_r, res, ServerType.Area);

            // Need to find definitions for different action types as Ids do overlap,  0 ?, 1 ?, 2 ? , 3 for skill_tree item, 4 for system , 5 for emote
            // Should we have a shortcutBarItem class?
            ShortcutBar shortcutBar0 = Database.SelectShortcutBarById(client.Character.shortcutBar0Id);
            ShortcutBar shortcutBar1 = Database.SelectShortcutBarById(client.Character.shortcutBar1Id);
            ShortcutBar shortcutBar2 = Database.SelectShortcutBarById(client.Character.shortcutBar2Id);
            ShortcutBar shortcutBar3 = Database.SelectShortcutBarById(client.Character.shortcutBar3Id);
            ShortcutBar shortcutBar4 = Database.SelectShortcutBarById(client.Character.shortcutBar4Id);
            int[] slots0 = shortcutBar0.getSlotArray();
            int[] slots1 = shortcutBar1.getSlotArray();
            int[] slots2 = shortcutBar2.getSlotArray();
            int[] slots3 = shortcutBar3.getSlotArray();
            int[] slots4 = shortcutBar4.getSlotArray();
            int[] action0 = shortcutBar0.getActionArray();
            int[] action1 = shortcutBar1.getActionArray();
            int[] action2 = shortcutBar2.getActionArray();
            int[] action3 = shortcutBar3.getActionArray();
            int[] action4 = shortcutBar4.getActionArray();
            for (int i = 0; i < slots0.Length; i++)
            {
                if (slots0[i] > 0)
                {
                    IBuffer res0 = BufferProvider.Provide();
                    res0.WriteByte(0);
                    res0.WriteByte((byte)i);
                    res0.WriteInt32(action0[i]);
                    res0.WriteInt64(slots0[i]);       // SkillId from skill_tree.csv for class skills
                    res0.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res0, ServerType.Area);
                }
            }
            for (int i = 0; i < slots1.Length; i++)
            {
                if (slots1[i] > 0)
                {
                    IBuffer res1 = BufferProvider.Provide();
                    res1.WriteByte(1);
                    res1.WriteByte((byte)i);
                    res1.WriteInt32(action1[i]);
                    res1.WriteInt64(slots1[i]);
                    res1.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res1, ServerType.Area);
                }
            }
            
            for (int i = 0; i < slots2.Length; i++)
            {
                if (slots2[i] > 0)
                {
                    IBuffer res1 = BufferProvider.Provide();
                    res1.WriteByte(2);
                    res1.WriteByte((byte)i);
                    res1.WriteInt32(action2[i]);
                    res1.WriteInt64(slots2[i]);
                    res1.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res1, ServerType.Area);
                }
            }
            for (int i = 0; i < slots3.Length; i++)
            {
                if (slots3[i] > 0)
                {
                    IBuffer res1 = BufferProvider.Provide();
                    res1.WriteByte(3);
                    res1.WriteByte((byte)i);
                    res1.WriteInt32(action3[i]);
                    res1.WriteInt64(slots3[i]);
                    res1.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res1, ServerType.Area);
                }
            }
            for (int i = 0; i < slots4.Length; i++)
            {
                if (slots4[i] > 0)
                {
                    IBuffer res1 = BufferProvider.Provide();
                    res1.WriteByte(4);
                    res1.WriteByte((byte)i);
                    res1.WriteInt32(action4[i]);
                    res1.WriteInt64(slots4[i]);
                    res1.WriteFixedString("SkillName", 16);//size is 0x10
                    Router.Send(client, (ushort)AreaPacketId.recv_shortcut_notify_regist, res1, ServerType.Area);
                }
            }

        }
    }
}
