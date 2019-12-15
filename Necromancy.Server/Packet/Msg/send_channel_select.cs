using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_channel_select : ClientHandler
    {
        public send_channel_select(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_channel_select;

        public override void Handle(NecClient client, NecPacket packet)
        {
            if (!Server.Maps.TryGet(client.Character.MapId, out Map map))
            {
                Logger.Error(client, $"No map found for MapId: {client.Character.MapId}");
                client.Close();
                return;
            }


            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0); //Error

            //sub_4E4210_2341  // impacts map spawn ID
            res.WriteInt32(map.Id); //MapSerialID
            res.WriteInt32(map.Id); //MapID
            res.WriteFixedString("127.0.0.1", 65); //IP
            res.WriteInt16(60002); //Port

            //sub_484420   //  does not impact map spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.Heading); //View offset
            //

            Router.Send(client, (ushort) MsgPacketId.recv_channel_select_r, res, ServerType.Msg);
        }
    }
}
