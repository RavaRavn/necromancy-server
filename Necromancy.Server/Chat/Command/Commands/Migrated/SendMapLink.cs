using System;
using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendMapLink : ServerChatCommand
    {
        public SendMapLink(NecServer server) : base(server)
        {
        }

        int[] EquipBitMask = new int[]
        {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
            1048576, 2097152
        };

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            // 10
            int colorChoice = Convert.ToInt32(command[0]);


            if (colorChoice == 10)
            {
                colorChoice = EquipBitMask[Util.GetRandomNumber(0, 4)];
            } // pick random model if you don't specify an ID.

            IBuffer res1 = BufferProvider.Provide(); // it's the aura portal for map
            res1.WriteInt32(Util.GetRandomNumber(1000, 1010)); // Unique ID

            res1.WriteFloat(client.Character.X); //x
            res1.WriteFloat(client.Character.Y); //y
            res1.WriteFloat(client.Character.Z + 2); //z
            res1.WriteByte(client.Character.Heading); // offset

            res1.WriteFloat(1000); // Height
            res1.WriteFloat(100); // Width

            res1.WriteInt32(
                colorChoice); // Aura color 0=blue 1=gold 2=white 3=red 4=purple 5=black  0 to 5, crash above 5
            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_maplink, res1, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "MapLink";
    }
}
