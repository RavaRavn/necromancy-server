using System;
using System.IO;
using Arrowgene.Services.Logging;
using Necromancy.Server.Database.Sql;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Database
{
    public class NecDatabaseBuilder
    {
        private readonly ILogger _logger;

        public NecDatabaseBuilder()
        {
            _logger = LogProvider.Logger(this);
        }

        public IDatabase Build(DatabaseSettings settings)
        {
            IDatabase database = null;
            switch (settings.Type)
            {
                case DatabaseType.SQLite:
                    database = PrepareSqlLiteDb(settings.SqLiteFolder);
                    break;
            }

            if (database == null)
            {
                _logger.Error("Database could not be created, exiting...");
                Environment.Exit(1);
            }

            return database;
        }

        private NecSqLiteDb PrepareSqlLiteDb(string sqLiteFolder)
        {
            string sqLitePath = Path.Combine(sqLiteFolder, $"db.v{NecSqLiteDb.Version}.sqlite");
            NecSqLiteDb db = new NecSqLiteDb(sqLitePath);
            if (db.CreateDatabase())
            {
                ScriptRunner scriptRunner = new ScriptRunner(db);
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/schema_sqlite.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_item.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_npc.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_monster.sql"));
                scriptRunner.Run(Path.Combine(sqLiteFolder, "Script/data_account.sql"));
            }

            return db;
        }
    }
}
