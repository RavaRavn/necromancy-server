using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertShortcutBar =
            "INSERT INTO `nec_shortcut_bar` (`slot0`, `slot1`, `slot2`, `slot3`, `slot4`, `slot5`, `slot6`, `slot7`, `slot8`, `slot9`, `action0`, `action1`, `action2`, `action3`, `action4`, `action5`, `action6`, `action7`, `action8`, `action9`) VALUES (@slot0, @slot1, @slot2, @slot3, @slot4, @slot5, @slot6, @slot7, @slot8, @slot9, @action0, @action1, @action2, @action3, @action4, @action5, @action6, @action7, @action8, @action9);";

        private const string SqlSelectShortcutBarById =
            "SELECT `id`, `slot0`, `slot1`, `slot2`, `slot3`, `slot4`, `slot5`, `slot6`, `slot7`, `slot8`, `slot9`, `action0`, `action1`, `action2`, `action3`, `action4`, `action5`, `action6`, `action7`, `action8`, `action9` FROM `nec_shortcut_bar` WHERE `id`=@id;";
        private const string SqlUpdateShortcutBar =
            "UPDATE `nec_shortcut_bar` SET `slot0`=@slot0, `slot1`=@slot1, `slot2`=@slot2, `slot3`=@slot3, `slot4`=@slot4, `slot5`=@slot5, `slot6`=@slot6, `slot7`=@slot7, `slot8`=@slot8, `slot9`=@slot9, `action0`=@action0, `action1`=@action1, `action2`=@action2, `action3`=@action3, `action4`=@action4, `action5`=@action5, `action6`=@action6, `action7`=@action7, `action8`=@action8, `action9`=@action9 WHERE `id`=@id;";

        private const string SqlDeleteShortcutBar =
            "DELETE FROM `nec_shortcut_bar` WHERE `id`=@id;";

        public bool InsertShortcutBar(ShortcutBar shortcutBar)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertShortcutBar, command =>
            {
                AddParameter(command, "@slot0", shortcutBar.Slot0);
                AddParameter(command, "@slot1", shortcutBar.Slot1);
                AddParameter(command, "@slot2", shortcutBar.Slot2);
                AddParameter(command, "@slot3", shortcutBar.Slot3);
                AddParameter(command, "@slot4", shortcutBar.Slot4);
                AddParameter(command, "@slot5", shortcutBar.Slot5);
                AddParameter(command, "@slot6", shortcutBar.Slot6);
                AddParameter(command, "@slot7", shortcutBar.Slot7);
                AddParameter(command, "@slot8", shortcutBar.Slot8);
                AddParameter(command, "@slot9", shortcutBar.Slot9);
                AddParameter(command, "@action0", shortcutBar.Action0);
                AddParameter(command, "@action1", shortcutBar.Action1);
                AddParameter(command, "@action2", shortcutBar.Action2);
                AddParameter(command, "@action3", shortcutBar.Action3);
                AddParameter(command, "@action4", shortcutBar.Action4);
                AddParameter(command, "@action5", shortcutBar.Action5);
                AddParameter(command, "@action6", shortcutBar.Action6);
                AddParameter(command, "@action7", shortcutBar.Action7);
                AddParameter(command, "@action8", shortcutBar.Action8);
                AddParameter(command, "@action9", shortcutBar.Action9);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            shortcutBar.Id = (int) autoIncrement;
            return true;
        }
        
        public ShortcutBar SelectShortcutBarById(int shortcutBarId)
        {
            ShortcutBar shortcutBar = null;
            ExecuteReader(SqlSelectShortcutBarById,
                command => { AddParameter(command, "@id", shortcutBarId); }, reader =>
                {
                    if (reader.Read())
                    {
                        shortcutBar = ReadShortcutBar(reader);
                    }
                });
            return shortcutBar;
        }
        public bool UpdateShortcutBar(ShortcutBar shortcutBar)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateShortcutBar, command =>
            {
                AddParameter(command, "@id", shortcutBar.Id);
                AddParameter(command, "@slot0", shortcutBar.Slot0);
                AddParameter(command, "@slot1", shortcutBar.Slot1);
                AddParameter(command, "@slot2", shortcutBar.Slot2);
                AddParameter(command, "@slot3", shortcutBar.Slot3);
                AddParameter(command, "@slot4", shortcutBar.Slot4);
                AddParameter(command, "@slot5", shortcutBar.Slot5);
                AddParameter(command, "@slot6", shortcutBar.Slot6);
                AddParameter(command, "@slot7", shortcutBar.Slot7);
                AddParameter(command, "@slot8", shortcutBar.Slot8);
                AddParameter(command, "@slot9", shortcutBar.Slot9);
                AddParameter(command, "@action0", shortcutBar.Action0);
                AddParameter(command, "@action1", shortcutBar.Action1);
                AddParameter(command, "@action2", shortcutBar.Action2);
                AddParameter(command, "@action3", shortcutBar.Action3);
                AddParameter(command, "@action4", shortcutBar.Action4);
                AddParameter(command, "@action5", shortcutBar.Action5);
                AddParameter(command, "@action6", shortcutBar.Action6);
                AddParameter(command, "@action7", shortcutBar.Action7);
                AddParameter(command, "@action8", shortcutBar.Action8);
                AddParameter(command, "@action9", shortcutBar.Action9);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteShortcutBar(int shortcutBarId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteShortcutBar, command => { AddParameter(command, "@id", shortcutBarId); });
            return rowsAffected > NoRowsAffected;
        }

        private ShortcutBar ReadShortcutBar(DbDataReader reader)
        {
            {
                ShortcutBar shortcutBar = new ShortcutBar();
                shortcutBar.Id = GetInt32(reader, "id");
                shortcutBar.Slot0 = GetInt32(reader, "slot0");
                shortcutBar.Slot1 = GetInt32(reader, "slot1");
                shortcutBar.Slot2 = GetInt32(reader, "slot2");
                shortcutBar.Slot3 = GetInt32(reader, "slot3");
                shortcutBar.Slot4 = GetInt32(reader, "slot4");
                shortcutBar.Slot5 = GetInt32(reader, "slot5");
                shortcutBar.Slot6 = GetInt32(reader, "slot6");
                shortcutBar.Slot7 = GetInt32(reader, "slot7");
                shortcutBar.Slot8 = GetInt32(reader, "slot8");
                shortcutBar.Slot9 = GetInt32(reader, "slot9");
                shortcutBar.Action0 = GetInt32(reader, "action0");
                shortcutBar.Action1 = GetInt32(reader, "action1");
                shortcutBar.Action2 = GetInt32(reader, "action2");
                shortcutBar.Action3 = GetInt32(reader, "action3");
                shortcutBar.Action4 = GetInt32(reader, "action4");
                shortcutBar.Action5 = GetInt32(reader, "action5");
                shortcutBar.Action6 = GetInt32(reader, "action6");
                shortcutBar.Action7 = GetInt32(reader, "action7");
                shortcutBar.Action8 = GetInt32(reader, "action8");
                shortcutBar.Action9 = GetInt32(reader, "action9");
                return shortcutBar;
            }
        }
    }
}
