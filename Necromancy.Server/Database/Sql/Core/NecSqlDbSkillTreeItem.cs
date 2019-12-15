using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertSkillTreeItem =
            "INSERT INTO `nec_skilltree_item` (`skill_id`, `char_id`, `level`) VALUES (@skill_id, @char_id, @level);";

        private const string SqlSelectSkillTreeItemById =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `id`=@id;";

        private const string SqlSelectSkillTreeItemsByCharId =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `char_id`=@char_id;";

        private const string SqlSelectSkillTreeItemByCharSkillId =
            "SELECT `id`, `skill_id`, `char_id`, `level` FROM `nec_skilltree_item` WHERE `char_id`=@char_id AND `skill_id`=@skill_id;";

        private const string SqlUpdateSkillTreeItem =
            "UPDATE `nec_skilltree_item` SET `skill_id`=@skill_id, `char_id`=@char_id, `level`=@level WHERE `id`=@id;";

        private const string SqlDeleteSkillTreeItem =
            "DELETE FROM `nec_skilltree_item` WHERE `id`=@id;";

        public bool InsertSkillTreeItem(SkillTreeItem skillTreeItem)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertSkillTreeItem, command =>
            {
                AddParameter(command, "@id", skillTreeItem.Id);
                AddParameter(command, "@skill_id", skillTreeItem.SkillId);
                AddParameter(command, "@char_id", skillTreeItem.CharId);
                AddParameter(command, "@level", skillTreeItem.Level);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }
            skillTreeItem.Id = (int)autoIncrement;
            return true;
        }
        
        public SkillTreeItem SelectSkillTreeItemById(int id)
        {
            SkillTreeItem skillTreeItem = null;
            ExecuteReader(SqlSelectSkillTreeItemById,
                command => { AddParameter(command, "@id", id); }, reader =>
                {
                    if (reader.Read())
                    {
                        skillTreeItem = ReadSkillTreeItem(reader);
                    }
                });
            return skillTreeItem;
        }

        public List<SkillTreeItem> SelectSkillTreeItemsByCharId(int charId)
        {
            List<SkillTreeItem> skillTreeItems = new List<SkillTreeItem>();
            ExecuteReader(SqlSelectSkillTreeItemsByCharId,
                command => { AddParameter(command, "@char_id", charId); }, reader =>
                {
                    while (reader.Read())
                    {
                        SkillTreeItem skillTreeItem = ReadSkillTreeItem(reader);
                        skillTreeItems.Add(skillTreeItem);
                    }
                });
            return skillTreeItems;
        }

        public SkillTreeItem SelectSkillTreeItemByCharSkillId(int charId, int skillId)
        {
            SkillTreeItem skillTreeItem = null;
            ExecuteReader(SqlSelectSkillTreeItemByCharSkillId,
                command => { 
                    AddParameter(command, "@char_id", charId);
                    AddParameter(command, "@skill_id", skillId);
                }, reader =>
                {
                    if (reader.Read())
                    {
                        skillTreeItem = ReadSkillTreeItem(reader);
                    }
                });
            return skillTreeItem;
        }
        public bool UpdateSkillTreeItem(SkillTreeItem skillTreeItem)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateSkillTreeItem, command =>
            {
                AddParameter(command, "@skill_id", skillTreeItem.SkillId);
                AddParameter(command, "@char_id", skillTreeItem.CharId);
                AddParameter(command, "@level", skillTreeItem.Level);
                AddParameter(command, "@id", skillTreeItem.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteSkillTreeItem(int id)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteSkillTreeItem, command => { AddParameter(command, "@id", id); });
            return rowsAffected > NoRowsAffected;
        }

        private SkillTreeItem ReadSkillTreeItem(DbDataReader reader)
        {
            SkillTreeItem skillTreeItem = new SkillTreeItem();
            skillTreeItem.Id = GetInt32(reader, "id");
            skillTreeItem.SkillId = GetInt32(reader, "skill_id");
            skillTreeItem.CharId = GetInt32(reader, "char_id");
            skillTreeItem.Level = GetInt32(reader, "level");
            return skillTreeItem;
        }
    }
}
