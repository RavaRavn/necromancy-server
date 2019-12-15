using System;
using System.Data.Common;
using Arrowgene.Services.Logging;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Database.Sql.Core
{
    /// <summary>
    /// Implementation of Necromancy database operations.
    /// </summary>
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        protected readonly NecLogger Logger;


        public NecSqlDb()
        {
            Logger = LogProvider.Logger<NecLogger>(this);
        }

        protected override void Exception(Exception ex)
        {
            Logger.Exception(ex);
        }

    }
}
