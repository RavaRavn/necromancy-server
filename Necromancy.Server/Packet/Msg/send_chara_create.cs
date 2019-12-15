using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_create : ClientHandler
    {
        public send_chara_create(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_create;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte character_slot_id = packet.Data.ReadByte();
            string character_name = packet.Data.ReadCString();
            uint race_id = packet.Data.ReadUInt32();
            uint sex_id = packet.Data.ReadUInt32();

            byte hair_id = packet.Data.ReadByte();
            byte hair_color_id = packet.Data.ReadByte();
            byte face_id = packet.Data.ReadByte();

            uint alignment_id = packet.Data.ReadUInt32();

            ushort strength = packet.Data.ReadUInt16();
            ushort vitality = packet.Data.ReadUInt16();
            ushort dexterity = packet.Data.ReadUInt16();
            ushort agility = packet.Data.ReadUInt16();
            ushort intelligence = packet.Data.ReadUInt16();
            ushort piety = packet.Data.ReadUInt16();
            ushort luck = packet.Data.ReadUInt16();

            uint class_id = packet.Data.ReadUInt32();
            uint unknown_a = packet.Data.ReadUInt32();


            //-------------------------------------
            // Send Character Creation packets to Database for laster use.

            if (!Maps.TryGet(Map.NewCharacterMapId, out Map map))
            {
                Logger.Error($"New character map not found MapId: {Map.NewCharacterMapId}");
                client.Close();
            }

            Character character = new Character();
            character.MapId = map.Id;
            character.X = map.X;
            character.Y = map.Y;
            character.Z = map.Z;
            character.Heading = (byte) map.Orientation;

            character.AccountId = client.Account.Id;
            character.SoulId = client.Soul.Id;
            character.Slot = character_slot_id;
            character.Name = character_name;
            
            character.Raceid = race_id;
            character.Sexid = sex_id;
            character.HairId = hair_id;
            character.HairColorId = hair_color_id;
            character.FaceId = face_id;
            character.Alignmentid = alignment_id;
            character.Strength = strength;
            character.vitality = vitality;
            character.dexterity = dexterity;
            character.agility = agility;
            character.intelligence = intelligence;
            character.piety = piety;
            character.luck = luck;
            character.ClassId = class_id;
            CreateShortcutBars(client, character, class_id);

            //----------------------------------------------------------
            // Character Slot ID

            if (!Database.InsertCharacter(character))
            {
                Logger.Error(client, $"Failed to create CharacterSlot: {character_slot_id}");
                client.Close();
                return;
            }
            CreateSkillTreeItems(client, character, class_id);

            client.Character = character;
            Logger.Info($"Created CharacterSlot: {character_slot_id}");
            Logger.Info($"Created CharacterName: {character_name}");
            Logger.Info($"Created race: {race_id}");
            Logger.Info($"Created sex: {sex_id}");
            Logger.Info($"Created hair: {hair_id}");
            Logger.Info($"Created hair color: {hair_color_id}");
            Logger.Info($"Created faceid: {face_id}");
            Logger.Info($"Created alignment_id: {alignment_id}");
            Logger.Info($"Created strength: {strength}");
            Logger.Info($"Created vitality: {vitality}");
            Logger.Info($"Created dexterity: {dexterity}");
            Logger.Info($"Created agility: {agility}");
            Logger.Info($"Created intelligence: {intelligence}");
            Logger.Info($"Created piety: {piety}");
            Logger.Info($"Created luck: {luck}");
            Logger.Info($"Created class_id: {class_id}");

            //-------------------------------------------------------------------------------

            //-------------------------------------------------------------------------------


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(character.Id); //CharacterId

            Router.Send(client, (ushort) MsgPacketId.recv_chara_create_r, res, ServerType.Msg);
        }

        private void CreateSkillTreeItems(NecClient client, Character character, uint class_id)
        {
            if (class_id == 0)      // Fighter
            {
                for (int i = 0; i < fighterSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = fighterSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }
            else if (class_id == 1)     // Thief
            {
                for (int i = 0; i < thiefSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = thiefSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }
            else if (class_id == 2)       // Mage
            {
                for (int i = 0; i < mageSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = mageSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }
            else if (class_id == 3)         // Priest
            {
                for (int i = 0; i < priestSkills.Length; i++)
                {
                    SkillTreeItem skillTreeItem = new SkillTreeItem();
                    skillTreeItem.Level = 1;
                    skillTreeItem.SkillId = priestSkills[i];
                    skillTreeItem.CharId = character.Id;
                    if (!Database.InsertSkillTreeItem(skillTreeItem))
                    {
                        Logger.Error(client, $"Failed to create SkillTreeItem");
                        client.Close();
                        return;
                    }
                }
            }

        }
        // ToDo should we have separate claases for each class?  Fighter, Mage, Priest and Thief
        int[] thiefSkills = new int[] { 14101, 14302, 14803 };
        int[] fighterSkills = new int[] { 11101, 11201 };
        int[] mageSkills = new int[] { 13101, 13404 };
        int[] priestSkills = new int[] { 12501, 12601 };

        private void CreateShortcutBars(NecClient client, Character character, uint class_id)
        {
            ShortcutBar shortcutBar0 = new ShortcutBar();
            if (class_id == 0)      // Fighter
            {
                shortcutBar0.Slot0 = 11101;
                shortcutBar0.Action0 = 3;
                shortcutBar0.Slot1 = 11201;
                shortcutBar0.Action1 = 3;
            }
            else if (class_id == 1)     // Thief
            {
                shortcutBar0.Slot0 = 14101;
                shortcutBar0.Action0 = 3;
                shortcutBar0.Slot1 = 14302;
                shortcutBar0.Action1 = 3;
                shortcutBar0.Slot2 = 14803;
                shortcutBar0.Action2 = 3;
            }
            else if (class_id == 2)       // Mage
            {
                shortcutBar0.Slot0 = 13101;
                shortcutBar0.Action0 = 3;
                shortcutBar0.Slot1 = 13404;
                shortcutBar0.Action1 = 3;
            }
            else if (class_id == 3)         // Priest
            {
                shortcutBar0.Slot0 = 12501;
                shortcutBar0.Action0 = 3;
                shortcutBar0.Slot1 = 12601;
                shortcutBar0.Action1 = 3;
            }
            shortcutBar0.Slot4 = 11;
            shortcutBar0.Action4 = 4;
            shortcutBar0.Slot6 = 18;
            shortcutBar0.Action6 = 4;
            shortcutBar0.Slot7 = 22;
            shortcutBar0.Action7 = 4;
            shortcutBar0.Slot9 = 2;
            shortcutBar0.Action9 = 4;
            if (!Database.InsertShortcutBar(shortcutBar0))
            {
                Logger.Error(client, $"Failed to create ShortcutBar0");
                client.Close();
                character.shortcutBar0Id = -1;
                return;
            }
            character.shortcutBar0Id = shortcutBar0.Id;

            ShortcutBar shortcutBar1 = new ShortcutBar();
            shortcutBar1.Slot0 = 1;
            shortcutBar1.Action0 = 5;
            shortcutBar1.Slot1 = 2;
            shortcutBar1.Action1 = 5;
            shortcutBar1.Slot2 = 4;
            shortcutBar1.Action2 = 5;
            shortcutBar1.Slot3 = 5;
            shortcutBar1.Action3 = 5;
            shortcutBar1.Slot4 = 6;
            shortcutBar1.Action4 = 5;
            shortcutBar1.Slot5 = 7;
            shortcutBar1.Action5 = 5;
            shortcutBar1.Slot6 = 11;
            shortcutBar1.Action6 = 5;
            shortcutBar1.Slot7 = 14;
            shortcutBar1.Action7 = 5;
            shortcutBar1.Slot8 = 15;
            shortcutBar1.Action8 = 5;
            shortcutBar1.Slot9 = 16;
            shortcutBar1.Action9 = 5;
            if (!Database.InsertShortcutBar(shortcutBar1))
            {
                Logger.Error(client, $"Failed to create ShortcutBar1");
                client.Close();
                character.shortcutBar1Id = -1;
                return;
            }
            character.shortcutBar1Id = shortcutBar1.Id;

            ShortcutBar shortcutBar2 = new ShortcutBar();
            if (!Database.InsertShortcutBar(shortcutBar2))
            {
                Logger.Error(client, $"Failed to create ShortcutBar2");
                client.Close();
                character.shortcutBar2Id = -1;
                return;
            }
            character.shortcutBar2Id = shortcutBar2.Id;

            ShortcutBar shortcutBar3 = new ShortcutBar();
            if (!Database.InsertShortcutBar(shortcutBar3))
            {
                Logger.Error(client, $"Failed to create ShortcutBar3");
                client.Close();
                character.shortcutBar3Id = -1;
                return;
            }
            character.shortcutBar3Id = shortcutBar3.Id;

            ShortcutBar shortcutBar4 = new ShortcutBar();
            if (!Database.InsertShortcutBar(shortcutBar4))
            {
                Logger.Error(client, $"Failed to create ShortcutBar4");
                client.Close();
                character.shortcutBar4Id = -1;
                return;
            }
            character.shortcutBar4Id = shortcutBar4.Id;
        }
    }
}
