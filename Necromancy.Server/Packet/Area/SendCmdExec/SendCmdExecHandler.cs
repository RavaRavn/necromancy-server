using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecHandler : ClientHandlerDeserializer<SendCmdExecRequest>
    {
        public SendCmdExecHandler(NecServer server) : base(server, new SendCmdExecDeserializer())
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_cmd_exec;

        public override void HandleRequest(NecClient client, SendCmdExecRequest request)
        {
            Server.Chat.CommandHandler.HandleCommand(client, request.CommandString());
            // IBuffer res = BufferProvider.Provide();
            // res.WriteInt32(0);
            // Router.Send(client, (ushort) AreaPacketId.recv_cmd_exec_r, res, ServerType.Area);
        }
    }
}
