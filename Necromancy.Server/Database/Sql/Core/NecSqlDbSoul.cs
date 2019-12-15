using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertSoul =
            "INSERT INTO `nec_soul` (`account_id`, `name`, `level`, `created`, `password`) VALUES (@account_id, @name, @level, @created, @password);";

        private const string SqlSelectSoulById =
            "SELECT `id`, `account_id`, `name`, `level`, `created`, `password` FROM `nec_soul` WHERE `id`=@id;";

        private const string SqlSelectSoulsByAccountId =
            "SELECT `id`, `account_id`, `name`, `level`, `created`, `password` FROM `nec_soul` WHERE `account_id`=@account_id;";

        private const string SqlUpdateSoul =
            "UPDATE `nec_soul` SET `account_id`=@account_id, `name`=@name, `level`=@level, `created`=@created, `password`=@password WHERE `id`=@id;";

        private const string SqlDeleteSoul =
            "DELETE FROM `nec_soul` WHERE `id`=@id;";

        public bool InsertSoul(Soul soul)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertSoul, command =>
            {
                AddParameter(command, "@account_id", soul.AccountId);
                AddParameter(command, "@name", soul.Name);
                AddParameter(command, "@level", soul.Level);
                AddParameter(command, "@created", soul.Created);
                AddParameter(command, "@password", soul.Password);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            soul.Id = (int) autoIncrement;
            return true;
        }
        
        public Soul SelectSoulById(int soulId)
        {
            Soul soul = null;
            ExecuteReader(SqlSelectSoulById,
                command => { AddParameter(command, "@id", soulId); }, reader =>
                {
                    if (reader.Read())
                    {
                        soul = ReadSoul(reader);
                    }
                });
            return soul;
        }

        public List<Soul> SelectSoulsByAccountId(int accountId)
        {
            List<Soul> souls = new List<Soul>();
            ExecuteReader(SqlSelectSoulsByAccountId,
                command => { AddParameter(command, "@account_id", accountId); }, reader =>
                {
                    while (reader.Read())
                    {
                        Soul soul = ReadSoul(reader);
                        souls.Add(soul);
                    }
                });
            return souls;
        }

        public bool UpdateSoul(Soul soul)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateSoul, command =>
            {
                AddParameter(command, "@account_id", soul.AccountId);
                AddParameter(command, "@name", soul.Name);
                AddParameter(command, "@level", soul.Level);
                AddParameter(command, "@created", soul.Created);
                AddParameter(command, "@id", soul.Id);
                AddParameter(command, "@password", soul.Password);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteSoul(int soulId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteSoul, command => { AddParameter(command, "@id", soulId); });
            return rowsAffected > NoRowsAffected;
        }

        private Soul ReadSoul(DbDataReader reader)
        {
            {
                Soul soul = new Soul();
                soul.Id = GetInt32(reader, "id");
                soul.AccountId = GetInt32(reader, "account_id");
                soul.Name = GetString(reader, "name");
                soul.Level = GetByte(reader, "level");
                soul.Created = GetDateTime(reader, "created");
                soul.Password = GetStringNullable(reader, "password");
                return soul;
            }
        }
    }
}
