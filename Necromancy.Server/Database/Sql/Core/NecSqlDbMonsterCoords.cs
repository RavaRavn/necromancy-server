using System.Collections.Generic;
using System.Data.Common;
using System.Numerics;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertMonsterCoord =
            "INSERT INTO `nec_monster_coords` (`monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z`) VALUES (@monster_id, @map_id, @coord_idx, @x, @y, @z);";

        private const string SqlSelectMonsterCoords =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords`;";

        private const string SqlSelectMonsterCoordByMapId =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords` WHERE `map_id`=@map_id;";

        private const string SqlSelectMonsterCoordById =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords` WHERE `id`=@id;";

        private const string SqlSelectMonsterCoordByMonsterId =
            "SELECT `id`, `monster_id`, `map_id`, `coord_idx`, `x`, `y`, `z` FROM `nec_monster_coords` WHERE `monster_id`=@monster_id;";

        private const string SqlUpdateMonsterCoord =
            "UPDATE `nec_monster_coords` SET `monster_id`=@monster_id, `map_id`=@map_id, `coord_idx`=@coord_idx, `x`=@x, `y`=@y, `z`=@z WHERE `id`=@id;";

        private const string SqlDeleteMonsterCoord =
            "DELETE FROM `nec_monster_coords` WHERE `id`=@id;";

        public bool InsertMonsterCoords(MonsterCoord monsterCoord)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertMonsterCoord, command =>
            {
                AddParameter(command, "@monster_id", monsterCoord.MonsterId);
                AddParameter(command, "@map_id", monsterCoord.MapId);
                AddParameter(command, "@coord_idx", monsterCoord.CoordIdx);
                AddParameter(command, "@x", monsterCoord.destination.X);
                AddParameter(command, "@y", monsterCoord.destination.Y);
                AddParameter(command, "@z", monsterCoord.destination.Z);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            monsterCoord.Id = (int) autoIncrement;
            return true;
        }

        public List<MonsterCoord> SelectMonsterCoords()
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SqlSelectMonsterCoords, reader =>
            {
                while (reader.Read())
                {
                    MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                    monsterCoords.Add(monsterCoord);
                }
            });
            return monsterCoords;
        }

        public List<MonsterCoord> SelectMonsterCoordsById(int Id)
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SqlSelectMonsterCoordById,
                command => { AddParameter(command, "@id", Id); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                        monsterCoords.Add(monsterCoord);
                    }
                });
            return monsterCoords;
        }
        public List<MonsterCoord> SelectMonsterCoordsByMonsterId(int mapId)
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SqlSelectMonsterCoordByMonsterId,
                command => { AddParameter(command, "@monster_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                        monsterCoords.Add(monsterCoord);
                    }
                });
            return monsterCoords;
        }
        public List<MonsterCoord> SelectMonsterCoordsByMapId(int mapId)
        {
            List<MonsterCoord> monsterCoords = new List<MonsterCoord>();
            ExecuteReader(SqlSelectMonsterCoordByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        MonsterCoord monsterCoord = ReadMonsterCoord(reader);
                        monsterCoords.Add(monsterCoord);
                    }
                });
            return monsterCoords;
        }

        public bool UpdateMonsterCoord(MonsterCoord monsterCoord)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateMonsterCoord, command =>
            {
                AddParameter(command, "@monster_id", monsterCoord.MonsterId);
                AddParameter(command, "@map_id", monsterCoord.MapId);
                AddParameter(command, "@coord_idx", monsterCoord.CoordIdx);
                AddParameter(command, "@x", monsterCoord.destination.X);
                AddParameter(command, "@y", monsterCoord.destination.Y);
                AddParameter(command, "@z", monsterCoord.destination.Z);
                AddParameter(command, "@id", monsterCoord.Id);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteMonsterCoord(int coordId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteMonsterCoord,
                command => { AddParameter(command, "@id", coordId); });
            return rowsAffected > NoRowsAffected;
        }

        private MonsterCoord ReadMonsterCoord(DbDataReader reader)
        {
            MonsterCoord monsterCoord = new MonsterCoord();
            Vector3 coords = new Vector3();
            monsterCoord.Id = GetInt32(reader, "id");
            monsterCoord.MonsterId = (uint)GetInt32(reader, "monster_id");
            monsterCoord.MapId = (uint)GetInt32(reader, "map_id");
            monsterCoord.CoordIdx = GetInt32(reader, "coord_idx");
            coords.X = GetFloat(reader, "x");
            coords.Y = GetFloat(reader, "y");
            coords.Z = GetFloat(reader, "z");
            monsterCoord.destination = coords;
           return monsterCoord;
        }
    }
}
