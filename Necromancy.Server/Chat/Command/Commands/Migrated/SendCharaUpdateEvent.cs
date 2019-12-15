using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    // updates your HP and MP to 1
    public class SendCharaUpdateEvent : ServerChatCommand
    {
        public SendCharaUpdateEvent(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            IBuffer res1 = BufferProvider.Provide();
            res1.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_weight, res1, ServerType.Area);


            IBuffer res6 = BufferProvider.Provide();
            res6.WriteInt32(44);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_mp, res6, ServerType.Area);


            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(100);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_maxmp, res2, ServerType.Area);


            IBuffer res7 = BufferProvider.Provide();
            res7.WriteInt32(55);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_hp, res7, ServerType.Area);


            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(100);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_maxhp, res3, ServerType.Area);

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(200);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_maxap, res4, ServerType.Area);


            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(200);
            Router.Send(client, (ushort) AreaPacketId.recv_chara_update_maxac, res5, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "char";
    }
}
