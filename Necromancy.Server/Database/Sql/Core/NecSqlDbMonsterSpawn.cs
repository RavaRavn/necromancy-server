using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertMonsterSpawn =
            "INSERT INTO `nec_monster_spawn` (`monster_id`, `model_id`, `level`,  `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `created`, `updated`) VALUES (@monster_id, @model_id, @level, @name, @title, @map_id, @x, @y, @z, @active, @heading, @size, @created, @updated);";

        private const string SqlSelectMonsterSpawns =
            "SELECT `id`, `monster_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `created`, `updated` FROM `nec_monster_spawn`;";

        private const string SqlSelectMonsterSpawnsByMapId =
            "SELECT `id`, `monster_id`, `model_id`, `level`, `name`, `title`, `map_id`, `x`, `y`, `z`, `active`, `heading`, `size`, `created`, `updated` FROM `nec_monster_spawn` WHERE `map_id`=@map_id;";

        private const string SqlUpdateMonsterSpawn =
            "UPDATE `nec_monster_spawn` SET `monster_id`=@monster_id, `model_id`=@model_id, `level`=@level,  `name`=@name, `title`=@title, `map_id`=@map_id, `x`=@x, `y`=@y, `z`=@z, `active`=@active, `heading`=@heading, `size`=@size, `created`=@created, `updated`=@updated WHERE `id`=@id;";

        private const string SqlDeleteMonsterSpawn =
            "DELETE FROM `nec_monster_spawn` WHERE `id`=@id;";

        public bool InsertMonsterSpawn(MonsterSpawn monsterSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertMonsterSpawn, command =>
            {
                AddParameter(command, "@monster_id", monsterSpawn.MonsterId);
                AddParameter(command, "@model_id", monsterSpawn.ModelId);
                AddParameter(command, "@level", monsterSpawn.Level);
                AddParameter(command, "@name", monsterSpawn.Name);
                AddParameter(command, "@title", monsterSpawn.Title);
                AddParameter(command, "@map_id", monsterSpawn.MapId);
                AddParameter(command, "@x", monsterSpawn.X);
                AddParameter(command, "@y", monsterSpawn.Y);
                AddParameter(command, "@z", monsterSpawn.Z);
                AddParameter(command, "@active", monsterSpawn.Active);
                AddParameter(command, "@heading", monsterSpawn.Heading);
                AddParameter(command, "@size", monsterSpawn.Size);
                AddParameter(command, "@created", monsterSpawn.Created);
                AddParameter(command, "@updated", monsterSpawn.Updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            monsterSpawn.Id = (int) autoIncrement;
            return true;
        }

        public List<MonsterSpawn> SelectMonsterSpawns()
        {
            List<MonsterSpawn> monsterSpawns = new List<MonsterSpawn>();
            ExecuteReader(SqlSelectMonsterSpawns, reader =>
            {
                while (reader.Read())
                {
                    MonsterSpawn monsterSpawn = ReadMonsterSpawn(reader);
                    monsterSpawns.Add(monsterSpawn);
                }
            });
            return monsterSpawns;
        }

        public List<MonsterSpawn> SelectMonsterSpawnsByMapId(int mapId)
        {
            List<MonsterSpawn> monsterSpawns = new List<MonsterSpawn>();
            ExecuteReader(SqlSelectMonsterSpawnsByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterSpawn monsterSpawn = ReadMonsterSpawn(reader);
                        monsterSpawns.Add(monsterSpawn);
                    }
                });
            return monsterSpawns;
        }

        public bool UpdateMonsterSpawn(MonsterSpawn monsterSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateMonsterSpawn, command =>
            {
                AddParameter(command, "@monster_id", monsterSpawn.MonsterId);
                AddParameter(command, "@model_id", monsterSpawn.ModelId);
                AddParameter(command, "@level", monsterSpawn.Level);
                AddParameter(command, "@name", monsterSpawn.Name);
                AddParameter(command, "@title", monsterSpawn.Title);
                AddParameter(command, "@map_id", monsterSpawn.MapId);
                AddParameter(command, "@x", monsterSpawn.X);
                AddParameter(command, "@y", monsterSpawn.Y);
                AddParameter(command, "@z", monsterSpawn.Z);
                AddParameter(command, "@active", monsterSpawn.Active);
                AddParameter(command, "@heading", monsterSpawn.Heading);
                AddParameter(command, "@size", monsterSpawn.Size);
                AddParameter(command, "@created", monsterSpawn.Created);
                AddParameter(command, "@updated", monsterSpawn.Updated);
                AddParameter(command, "@id", monsterSpawn.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteMonsterSpawn(int monsterSpawnId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteMonsterSpawn,
                command => { AddParameter(command, "@id", monsterSpawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private MonsterSpawn ReadMonsterSpawn(DbDataReader reader)
        {
            MonsterSpawn monsterSpawn = new MonsterSpawn();
            monsterSpawn.Id = GetInt32(reader, "id");
            monsterSpawn.ModelId = GetInt32(reader, "model_id");
            monsterSpawn.MonsterId = GetInt32(reader, "monster_id");
            monsterSpawn.Level = GetByte(reader, "level");
            monsterSpawn.Name = GetString(reader, "name");
            monsterSpawn.Title = GetString(reader, "title");
            monsterSpawn.MapId = GetInt32(reader, "map_id");
            monsterSpawn.X = GetFloat(reader, "x");
            monsterSpawn.Y = GetFloat(reader, "y");
            monsterSpawn.Z = GetFloat(reader, "z");
            monsterSpawn.Active = GetBoolean(reader, "active");
            monsterSpawn.Heading = GetByte(reader, "heading");
            monsterSpawn.Size = GetInt16(reader, "size");
            monsterSpawn.Created = GetDateTime(reader, "created");
            monsterSpawn.Updated = GetDateTime(reader, "updated");
            return monsterSpawn;
        }
    }
}
