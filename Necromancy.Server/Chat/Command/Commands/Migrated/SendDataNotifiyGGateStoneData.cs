using System;
using System.Collections.Generic;
using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendDataNotifiyGGateStoneData : ServerChatCommand
    {
        //Makes a random statue or "Gaurdian Gate"
        public SendDataNotifiyGGateStoneData(NecServer server) : base(server)
        {
        }

        int[] GGateModelIds = new int[]
        {
            1800000, /*	Stone slab of guardian statue	*/
            1801000, /*	Bulletin board	*/
            1802000, /*	Sign	*/
            1803000, /*	Stone board	*/
            1804000, /*	Guardians Gate	*/
            1805000, /*	Warp device	*/
            1806000, /*	Puddle	*/
            1807000, /*	machine	*/
            1808000, /*	Junk mountain	*/
            1809000, /*	switch	*/
            1810000, /*	Statue	*/
            1811000, /*	Horse statue	*/
            1812000, /*	Agate balance	*/
            1813000, /*	Dagger scale	*/
            1814000, /*	Apple balance	*/
            1815000, /*	torch	*/
            1816000, /*	Royal shop sign	*/
            1817000, /*	Witch pot	*/
            1818000, /*	toilet	*/
            1819000, /*	Abandoned tree	*/
            1820000, /*	Pedestal with fire	*/
            1900000, /*	For transparency	*/
            1900001, /*	For transparency	*/
        };

        int[] EquipBitMask = new int[]
        {
            1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
            1048576, 2097152
        };

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            // 100
            int GGateChoice = Convert.ToInt32(command[0]);

            if (GGateChoice == 100)
            {
                GGateChoice = Util.GetRandomNumber(0, GGateModelIds.Length);
            } // pick random model if you don't specify an ID.

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(
                GGateModelIds[GGateChoice]); // Unique Object ID.  Crash if already in use (dont use your character ID)
            res.WriteInt32(GGateChoice); // Serial ID for Interaction? from npc.csv????
            res.WriteByte((byte) Util.GetRandomNumber(0, 2)); // 0 = Text, 1 = F to examine  , 2 or above nothing
            res.WriteCString($"The number of GGate you picked from array is : {GGateChoice}"); //"0x5B" //Name
            res.WriteCString($"The Model ID of your GGate is: {GGateModelIds[GGateChoice]}"); //"0x5B" //Title
            res.WriteFloat(client.Character.X + Util.GetRandomNumber(25, 150)); //X Pos
            res.WriteFloat(client.Character.Y + Util.GetRandomNumber(25, 150)); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.Heading); //view offset
            res.WriteInt32(
                GGateModelIds[
                    GGateChoice]); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv

            res.WriteInt16(100); //  size of the object

            res.WriteInt32(0); // 0 = collision, 1 = no collision  (active/Inactive?)

            res.WriteInt32(
                EquipBitMask[
                    Util.GetRandomNumber(0,
                        4)]); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx


            Router.Send(client, (ushort) AreaPacketId.recv_data_notify_ggate_stone_data, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "GGate";
    }
}
