using System;
using System.IO;
using System.Runtime.Serialization;
using Necromancy.Server.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Setting
{
    [DataContract]
    public class DatabaseSettings
    {
        public DatabaseSettings()
        {
            Type = DatabaseType.SQLite;
            SqLitePath = Path.Combine(Util.RelativeExecutingDirectory(), "db.sqlite");
            Host = "localhost";
            Port = 3306;
            Database = "necromancy";
            User = string.Empty;
            Password = string.Empty;

            string envDbType = Environment.GetEnvironmentVariable("DB_TYPE");
            switch (envDbType)
            {
                case "sqlite":
                    Type = DatabaseType.SQLite;
                    break;
            }

            string envDbUser = Environment.GetEnvironmentVariable("DB_USER");
            if (!string.IsNullOrEmpty(envDbUser))
            {
                User = envDbUser;
            }

            string envDbPass = Environment.GetEnvironmentVariable("DB_PASS");
            if (!string.IsNullOrEmpty(envDbPass))
            {
                Password = envDbPass;
            }
        }

        public DatabaseSettings(DatabaseSettings databaseSettings)
        {
            Type = databaseSettings.Type;
            SqLitePath = databaseSettings.SqLitePath;
            Host = databaseSettings.Host;
            Port = databaseSettings.Port;
            User = databaseSettings.User;
            Password = databaseSettings.Password;
            Database = databaseSettings.Database;
        }

        [DataMember(Order = 0)] public DatabaseType Type { get; set; }

        [DataMember(Order = 1)] public string SqLitePath { get; set; }

        [DataMember(Order = 2)] public string Host { get; set; }

        [DataMember(Order = 3)] public short Port { get; set; }

        [DataMember(Order = 4)] public string User { get; set; }

        [DataMember(Order = 5)] public string Password { get; set; }

        [DataMember(Order = 6)] public string Database { get; set; }
    }
}