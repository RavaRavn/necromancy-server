using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : Handler
    {
        public send_map_get_info(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2001007);


            Router.Send(client, (ushort)AreaPacketId.recv_map_get_info_r, res);
            int TestInt = 0;
            SendDataNotifyNpcData(client,TestInt);
        }
        int[] NPCModelID = new int[] { 1911105/*, 1112101, 1122401, 1122101, 1311102, 1111301, 1121401, 1131401, 2073002, 1421101 */};
        int[] NPCSerialID = new int[] { 10000101, 10000102, 10000103, 10000104, 10000105, 10000106, 10000107, 10000108, 80000009, 10000101 };
        int[] NPCX = new int[] { 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1800, 1900, 1000 };
        int[] NPCY = new int[] { 0 , -100, -200, -300, 100, 200, 300, 400, 0, 100 };
        int[] NPCZ = new int[] { 25, -25, -25, 25, 25, -22, -25, 25, 0, 0 };
        int[] TestArrayInt = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };
        byte[] NPCViewAngle = new byte[] {  45, 46, 47, 48, 49 , 40, 41, 42, 43, 44};

       


        private void SendDataNotifyNpcData(NecClient client,int TestInt)
        {
            int x = -1;
            //IBuffer res2 = BufferProvider.Provide();

            foreach (int y in NPCModelID)
            {
                x++;
                IBuffer res2 = BufferProvider.Provide();
                res2.WriteInt32(x+44444444);             // NPC ID (object id)

                res2.WriteInt32(NPCSerialID[x]) ;      // NPC Serial ID from "npc.csv"

                res2.WriteByte(0);              // 0 - Clickable NPC (Active NPC, player can select and start dialog), 1 - Not active NPC (Player can't start dialog)

                res2.WriteCString($"NPC Name : {x}");//Name

                res2.WriteCString($"NPC Title : {x}");//Title

                res2.WriteFloat(NPCX[x]);//X Pos
                res2.WriteFloat(NPCY[x]);//Y Pos
                res2.WriteFloat(NPCZ[x]);//Z Pos
                res2.WriteByte(NPCViewAngle[x]);//view offset

                res2.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res2.WriteInt32(24);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);
                res2.WriteInt32(1);

                res2.WriteInt32(19);


                int numEntries = 19;


                for (int i = 0; i < numEntries; i++)

                {
                    // loop start
                    res2.WriteInt32(210901); // this is a loop within a loop i went ahead and broke it up
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                    res2.WriteInt32(10310503);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(1); // bool
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);
                    res2.WriteByte(0);

                }

                res2.WriteInt32(19);

                //this is an x19 loop but i broke it up
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);
                res2.WriteInt32(3);

                res2.WriteInt32(NPCModelID[x]);   //NPC Model from file "model_common.csv"
                


                res2.WriteInt16(100);       //NPC Model Size

                res2.WriteByte(237);

                res2.WriteByte(237);

                res2.WriteByte(237);

                res2.WriteInt32(11111110); //Hp Related Bitmask?  This setting makes the NPC "alive"

                res2.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull

                res2.WriteInt32(11111110);
                res2.WriteFloat(1000);
                res2.WriteFloat(1000);
                res2.WriteFloat(1000);

                res2.WriteInt32(128);

                int numEntries2 = 128;


                for (int i = 0; i < numEntries2; i++)

                {
                    res2.WriteInt32(237);
                    res2.WriteInt32(237);
                    res2.WriteInt32(237);

                }

                Router.Send(client, (ushort)AreaPacketId.recv_data_notify_npc_data, res2);
            }
        }
    }
}