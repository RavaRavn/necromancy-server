using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertCharacter =
            "INSERT INTO `nec_character` (`account_id`, `soul_id`, `slot`, `map_id`, `x`, `y`, `z`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `alignment_id`, `strength`, `vitality`, `dexterity`, `agility`, `intelligence`, `piety`, `luck`, `class_id`, `level`, `shortcut_bar0_id`, `shortcut_bar1_id`, `shortcut_bar2_id`, `shortcut_bar3_id`, `shortcut_bar4_id`, `created`) VALUES (@account_id, @soul_id, @slot, @map_id, @x, @y, @z, @name, @race_id, @sex_id, @hair_id, @hair_color_id, @face_id, @alignment_id, @strength, @vitality, @dexterity, @agility, @intelligence, @piety, @luck, @class_id, @level, @shortcut_bar0_id, @shortcut_bar1_id, @shortcut_bar2_id, @shortcut_bar3_id, @shortcut_bar4_id, @created);";

        private const string SqlSelectCharacterById =
            "SELECT `id`, `account_id`, `soul_id`, `slot`, `map_id`, `x`, `y`, `z`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `alignment_id`, `strength`, `vitality`, `dexterity`, `agility`, `intelligence`, `piety`, `luck`, `class_id`, `level`, `shortcut_bar0_id`, `shortcut_bar1_id`, `shortcut_bar2_id`, `shortcut_bar3_id`, `shortcut_bar4_id`, `created` FROM `nec_character` WHERE `id`=@id;";

        private const string SqlSelectCharactersByAccountId =
            "SELECT `id`, `account_id`, `soul_id`, `slot`, `map_id`, `x`, `y`, `z`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `alignment_id`, `strength`, `vitality`, `dexterity`, `agility`, `intelligence`, `piety`, `luck`, `class_id`, `level`, `shortcut_bar0_id`, `shortcut_bar1_id`, `shortcut_bar2_id`, `shortcut_bar3_id`, `shortcut_bar4_id`, `created` FROM `nec_character` WHERE `account_id`=@account_id;";

        private const string SqlSelectCharactersBySoulId =
            "SELECT `id`, `account_id`, `soul_id`, `slot`, `map_id`, `x`, `y`, `z`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `alignment_id`, `strength`, `vitality`, `dexterity`, `agility`, `intelligence`, `piety`, `luck`, `class_id`, `level`, `shortcut_bar0_id`, `shortcut_bar1_id`, `shortcut_bar2_id`, `shortcut_bar3_id`, `shortcut_bar4_id`, `created` FROM `nec_character` WHERE `soul_id`=@soul_id;";

        private const string SqlSelectCharacterBySlot =
            "SELECT `id`, `account_id`, `soul_id`, `slot`, `map_id`, `x`, `y`, `z`, `name`, `race_id`, `sex_id`, `hair_id`, `hair_color_id`, `face_id`, `alignment_id`, `strength`, `vitality`, `dexterity`, `agility`, `intelligence`, `piety`, `luck`, `class_id`, `level`, `shortcut_bar0_id`, `shortcut_bar1_id`, `shortcut_bar2_id`, `shortcut_bar3_id`, `shortcut_bar4_id`, `created` FROM `nec_character` WHERE `soul_id`=@soul_id AND `slot`=@slot;";

        private const string SqlUpdateCharacter =
            "UPDATE `nec_character` SET `account_id`=@account_id, `soul_id`=@soul_id, `slot`=@slot, `map_id`=@map_id, `x`=@x, `y`=@y, `z`=@z, `name`=@name, `race_id`=@race_id, `sex_id`=@sex_id, `hair_id`=@hair_id, `hair_color_id`=@hair_color_id, `face_id`=@face_id, `alignment_id`=@alignment_id, `strength`=@strength, `vitality`=@vitality, `dexterity`=@dexterity, `agility`=@agility, `intelligence`=@intelligence, `piety`=@piety, `luck`=@luck, `class_id`=@class_id, `level`=@level, `shortcut_bar0_id`=@shortcut_bar0_id, `shortcut_bar1_id`=@shortcut_bar1_id, `shortcut_bar2_id`=@shortcut_bar2_id, `shortcut_bar3_id`=@shortcut_bar3_id, `shortcut_bar4_id`=@shortcut_bar4_id, `created`=@created WHERE `id`=@id;";

        private const string SqlDeleteCharacter =
            "DELETE FROM `nec_character` WHERE `id`=@id;";

        public bool InsertCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@soul_id", character.SoulId);
                AddParameter(command, "@slot", character.Slot);
                AddParameter(command, "@map_id", character.MapId);
                AddParameter(command, "@x", character.X);
                AddParameter(command, "@y", character.Y);
                AddParameter(command, "@z", character.Z);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@race_id", character.Raceid);
                AddParameter(command, "@sex_id", character.Sexid);
                AddParameter(command, "@hair_id", character.HairId);
                AddParameter(command, "@hair_color_id", character.HairColorId);
                AddParameter(command, "@face_id", character.FaceId);
                AddParameter(command, "@alignment_id", character.Alignmentid);
                AddParameter(command, "@strength", character.Strength);
                AddParameter(command, "@vitality", character.vitality);
                AddParameter(command, "@dexterity", character.dexterity);
                AddParameter(command, "@agility", character.agility);
                AddParameter(command, "@intelligence", character.intelligence);
                AddParameter(command, "@piety", character.piety);
                AddParameter(command, "@luck", character.luck);
                AddParameter(command, "@class_id", character.ClassId);
                AddParameter(command, "@level", character.Level);
                AddParameter(command, "@created", character.Created);
                AddParameter(command, "@shortcut_bar0_id", character.shortcutBar0Id);
                AddParameter(command, "@shortcut_bar1_id", character.shortcutBar1Id);
                AddParameter(command, "@shortcut_bar2_id", character.shortcutBar2Id);
                AddParameter(command, "@shortcut_bar3_id", character.shortcutBar3Id);
                AddParameter(command, "@shortcut_bar4_id", character.shortcutBar4Id);
                AddParameter(command, "@created", character.Created);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            character.Id = (int) autoIncrement;
            return true;
        }

        public Character SelectCharacterById(int characterId)
        {
            Character character = null;
            ExecuteReader(SqlSelectCharacterById,
                command => { AddParameter(command, "@id", characterId); }, reader =>
                {
                    if (reader.Read())
                    {
                        character = ReadCharacter(reader);
                    }
                });
            return character;
        }

        public List<Character> SelectCharactersByAccountId(int accountId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharactersByAccountId,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        public List<Character> SelectCharactersBySoulId(int soulId)
        {
            List<Character> characters = new List<Character>();
            ExecuteReader(SqlSelectCharactersBySoulId,
                command => { AddParameter(command, "@soul_id", soulId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Character character = ReadCharacter(reader);
                        characters.Add(character);
                    }
                });
            return characters;
        }

        public Character SelectCharacterBySlot(int soulId, int slot)
        {
            Character characters = null;
            ExecuteReader(SqlSelectCharacterBySlot,
                command =>
                {
                    AddParameter(command, "@soul_id", soulId);
                    AddParameter(command, "@slot", slot);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        characters = ReadCharacter(reader);
                    }
                });
            return characters;
        }

        public bool UpdateCharacter(Character character)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateCharacter, command =>
            {
                AddParameter(command, "@account_id", character.AccountId);
                AddParameter(command, "@soul_id", character.SoulId);
                AddParameter(command, "@slot", character.Slot);
                AddParameter(command, "@map_id", character.MapId);
                AddParameter(command, "@x", character.X);
                AddParameter(command, "@y", character.Y);
                AddParameter(command, "@z", character.Z);
                AddParameter(command, "@name", character.Name);
                AddParameter(command, "@race_id", character.Raceid);
                AddParameter(command, "@sex_id", character.Sexid);
                AddParameter(command, "@hair_id", character.HairId);
                AddParameter(command, "@hair_color_id", character.HairColorId);
                AddParameter(command, "@face_id", character.FaceId);
                AddParameter(command, "@alignment_id", character.Alignmentid);
                AddParameter(command, "@strength", character.Strength);
                AddParameter(command, "@vitality", character.vitality);
                AddParameter(command, "@dexterity", character.dexterity);
                AddParameter(command, "@agility", character.agility);
                AddParameter(command, "@intelligence", character.intelligence);
                AddParameter(command, "@piety", character.piety);
                AddParameter(command, "@luck", character.luck);
                AddParameter(command, "@class_id", character.ClassId);
                AddParameter(command, "@level", character.Level);
                AddParameter(command, "@shortcut_bar0_id", character.shortcutBar0Id);
                AddParameter(command, "@shortcut_bar1_id", character.shortcutBar1Id);
                AddParameter(command, "@shortcut_bar2_id", character.shortcutBar2Id);
                AddParameter(command, "@shortcut_bar3_id", character.shortcutBar3Id);
                AddParameter(command, "@shortcut_bar4_id", character.shortcutBar4Id);
                AddParameter(command, "@created", character.Created);
                AddParameter(command, "@id", character.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteCharacter(int characterId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteCharacter,
                command => { AddParameter(command, "@id", characterId); });
            return rowsAffected > NoRowsAffected;
        }

        private Character ReadCharacter(DbDataReader reader)
        {
            Character character = new Character();
            character.Id = GetInt32(reader, "id");
            character.AccountId = GetInt32(reader, "account_id");
            character.SoulId = GetInt32(reader, "soul_id");
            character.Created = GetDateTime(reader, "created");
            character.Slot = GetByte(reader, "slot");
            character.MapId = GetInt32(reader, "map_id");
            character.X = GetFloat(reader, "x");
            character.Y = GetFloat(reader, "y");
            character.Z = GetFloat(reader, "z");
            character.Name = GetString(reader, "name");
            character.Raceid = GetByte(reader, "race_id");
            character.Sexid = GetByte(reader, "sex_id");
            character.HairId = GetByte(reader, "hair_id");
            character.HairColorId = GetByte(reader, "hair_color_id");
            character.FaceId = GetByte(reader, "face_id");
            character.Alignmentid = GetByte(reader, "alignment_id");
            character.Strength = GetByte(reader, "strength");
            character.vitality = GetByte(reader, "vitality");
            character.dexterity = GetByte(reader, "dexterity");
            character.agility = GetByte(reader, "agility");
            character.intelligence = GetByte(reader, "intelligence");
            character.piety = GetByte(reader, "piety");
            character.luck = GetByte(reader, "luck");
            character.ClassId = GetByte(reader, "class_id");
            character.Level = GetByte(reader, "level");
            character.shortcutBar0Id = GetInt32(reader, "shortcut_bar0_id");
            character.shortcutBar1Id = GetInt32(reader, "shortcut_bar1_id");
            character.shortcutBar2Id = GetInt32(reader, "shortcut_bar2_id");
            character.shortcutBar3Id = GetInt32(reader, "shortcut_bar3_id");
            character.shortcutBar4Id = GetInt32(reader, "shortcut_bar4_id");
            return character;
        }
    }
}
