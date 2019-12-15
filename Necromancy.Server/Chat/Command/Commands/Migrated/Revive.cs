using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class Revive : ServerChatCommand
    {
        public Revive(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (client.Character.soulFormState == 1)
            {
                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(0);
                res1.WriteInt32(0);
                res1.WriteInt32(0);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_revive_init_r, res1, ServerType.Area);

                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(0); // 0 = sucess to revive, 1 = failed to revive
                client.Character.soulFormState -= 1;
                Router.Send(client.Map, (ushort) AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area);

                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(0);
                Router.Send(client.Map, (ushort) AreaPacketId.recv_revive_execute_r, res2, ServerType.Area);
            }

            else if (client.Character.soulFormState == 0)
            {
                IBuffer res1 = BufferProvider.Provide();
                res1.WriteInt32(client.Character.Id); // ID
                res1.WriteInt32(100101); //100101, its the id to get the tombstone
                Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_notify_stateflag, res1, ServerType.Area);

                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(1); // 0 = sucess to revive, 1 = failed to revive
                Router.Send(client.Map, (ushort) AreaPacketId.recv_raisescale_request_revive_r, res, ServerType.Area);

                IBuffer res5 = BufferProvider.Provide();
                Router.Send(client.Map, (ushort) AreaPacketId.recv_self_lost_notify, res5, ServerType.Area);
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "revi";
    }
}
