using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_regist_party_recruit : ClientHandler
    {
        public send_party_regist_party_recruit(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_regist_party_recruit;

       public override void Handle(NecClient client, NecPacket packet)
        {
            int objectiveID = packet.Data.ReadInt32(); // always 0, 1, or 2. based on drop down selection
            int detailsID = packet.Data.ReadInt32(); // Can Be Dungeon ID or Quest ID based on objective
            int targetID = packet.Data.ReadInt32(); // Unknown. possbly Target ID???
            int otherID = packet.Data.ReadInt32(); // Selected check box under other
            string comment = packet.Data.ReadFixedString(60); //Comment Box accepts up to 60 characters. 


            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(objectiveID);
            res.WriteInt32(detailsID);
            res.WriteInt32(targetID);
            res.WriteInt32(otherID);




            Router.Send(client, (ushort)AreaPacketId.recv_party_notify_recruit_request, res, ServerType.Area);
        }
    }
}