using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_select_package_update : ClientHandler
    {
        public send_select_package_update(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_select_package_update;

        public override void Handle(NecClient client, NecPacket packet)
        {
            bool loot = false;
            int objectID = packet.Data.ReadInt32();
            int operation = packet.Data.ReadInt32();
            int errcode = 0;

            IBuffer res = BufferProvider.Provide();

            if (operation == 0x03)//delete mail
                res.WriteInt32(0);
            else if (operation == 0x04)//unprotect message
                res.WriteInt32(0);
            else if (operation == 0x01 || operation == 0x0)//receive message
            {
                res.WriteInt32(0);
                loot = true;
            }
            else if (operation == 0x2)//reply to message
                res.WriteInt32(0);

            res.WriteInt32(errcode);//Error message Call. 0 for success. see additional options in Sys_msg.csv
                /*
                1	You have unopened mails	SYSTEM_WARNING
                2	No mails to delete	SYSTEM_WARNING
                3	You have %d unreceived mails. Please check your inbox.	SYSTEM_WARNING
                -2414	Mail title includes banned words	SYSTEM_WARNING

                */

            Router.Send(client, (ushort) AreaPacketId.recv_select_package_update_r, res, ServerType.Area);

            if(loot)
            {
                SendLootAccessObject(client, objectID);
                loot = false;
            }
        }

        private void SendLootAccessObject(NecClient client, int objectID)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //ObjectID or error check

            Router.Send(client, (ushort)AreaPacketId.recv_loot_access_object_r, res, ServerType.Area);
        }
    }
}