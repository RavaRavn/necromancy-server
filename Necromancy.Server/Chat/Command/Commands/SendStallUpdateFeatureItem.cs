using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    //updates a stall feature item
    public class SendStallUpdateFeatureItem : ServerChatCommand
    {
        public SendStallUpdateFeatureItem(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message, ChatResponse response)
        {
            //recv_stall_update_feature_item = 0xB195,
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id);

            res.WriteInt32(10200101);
            res.WriteByte(2);
            res.WriteByte(2);
            res.WriteByte(2);

            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt32(9);

            res.WriteByte(0); // bool

            res.WriteInt32(0);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_stall_update_feature_item, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "stuf";
    }
}
