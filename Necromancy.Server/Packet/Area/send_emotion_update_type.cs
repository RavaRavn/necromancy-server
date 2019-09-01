using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_emotion_update_type : Handler
    {
        public send_emotion_update_type(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_emotion_update_type;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            int emote = packet.Data.ReadInt32();

          
            res.WriteInt32(0); 

            Router.Send(client, (ushort) AreaPacketId.recv_emotion_update_type_r, res);

            SendEmotionNotifyType(client, emote);
        }

        public void SendEmotionNotifyType(NecClient client, int emote)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(client.Character.Id); //Character ID
            res.WriteInt32(emote); //Emote ID
            
            Router.Send(client.Map, (ushort) AreaPacketId.recv_emotion_notify_type, res, client);
        }
    }
}