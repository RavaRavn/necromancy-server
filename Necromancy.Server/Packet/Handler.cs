using Necromancy.Server.Database;
using Necromancy.Server.Model;
using Necromancy.Server.Setting;

namespace Necromancy.Server.Packet
{
    public abstract class Handler : IHandler
    {
        protected Handler(NecServer server)
        {
            Server = server;
            Router = server.Router;
            Database = server.Database;
            Settings = server.Setting;
            Maps = server.Maps;
            Clients = server.Clients;
        }

        public abstract ushort Id { get; }
        public virtual int ExpectedSize => NecQueueConsumer.NoExpectedSize;
        protected NecServer Server { get; }
        protected NecSetting Settings { get; }
        protected PacketRouter Router { get; }
        protected MapLookup Maps { get; }
        protected ClientLookup Clients { get; }
        protected IDatabase Database { get; }
    }
}
