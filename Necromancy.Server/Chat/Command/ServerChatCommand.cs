using Necromancy.Server.Packet;

namespace Necromancy.Server.Chat.Command
{
    public abstract class ServerChatCommand : ChatCommand
    {
        protected ServerChatCommand(NecServer server)
        {
            Server = server;
            Router = Server.Router;
        }

        protected NecServer Server { get; }
        protected PacketRouter Router { get; }
    }
}
