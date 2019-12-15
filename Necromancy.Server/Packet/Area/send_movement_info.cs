using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System.Globalization;

namespace Necromancy.Server.Packet.Area
{
    public class send_movement_info : ClientHandler
    {
        public send_movement_info(NecServer server) : base(server)
        {

        }

        public override ushort Id => (ushort)AreaPacketId.send_movement_info;

        int i = 0;

        public override void Handle(NecClient client, NecPacket packet)
        {

                client.Character.X = packet.Data.ReadFloat();
                client.Character.Y = packet.Data.ReadFloat();
                client.Character.Z = packet.Data.ReadFloat();

                float percentMovementIsX = packet.Data.ReadFloat();
                float percentMovementIsY = packet.Data.ReadFloat();
                float verticalMovementSpeedMultiplier = packet.Data.ReadFloat(); //  Confirm by climbing ladder at 1 up or -1 down. or Jumping

                float movementSpeed = packet.Data.ReadFloat();

                float horizontalMovementSpeedMultiplier = packet.Data.ReadFloat(); //always 1 when moving.  Confirm by Coliding with an  object and watching it Dip.

                client.Character.movementPose = packet.Data.ReadByte(); //Character Movement Type: Type 8 Falling / Jumping. Type 3 normal:  Type 9  climbing

                client.Character.movementAnim = packet.Data.ReadByte(); //Action Modifier Byte
                                                                        //146 :ladder left Foot Up.      //147 Ladder right Foot Up. 
                                                                        //151 Left Foot Down,            //150 Right Root Down .. //155 falling off ladder
                                                                        //81  jumping up,                //84  jumping down       //85 landing


                //Battle Logic until we find out how to write battle byte requrements in 'send_data_get_Self_chara_data_request' so the client can send the right info
                if (client.Character.battleAnim !=0)
                {
                client.Character.movementPose = 8 /*client.Character.battlePose*/;  //Setting the pose byte to the 2nd and 3rd digits of our equipped weapon ID. For battle!!
                client.Character.movementAnim = client.Character.battleAnim; //Setting the animation byte to an animation from C:\WO\Chara\chara\00\041\anim. 231, 232, 233, and 244 are attack animations
                }





                IBuffer res2 = BufferProvider.Provide();

                res2.WriteInt32(client.Character.InstanceId);//Character ID
                res2.WriteFloat(client.Character.X);
                res2.WriteFloat(client.Character.Y);
                res2.WriteFloat(client.Character.Z);
                                               
                res2.WriteFloat(percentMovementIsX);
                res2.WriteFloat(percentMovementIsY);
                res2.WriteFloat(verticalMovementSpeedMultiplier);
               
                res2.WriteFloat(movementSpeed);

                res2.WriteFloat(horizontalMovementSpeedMultiplier);

                res2.WriteByte(client.Character.movementPose); //MOVEMENT ANIM
                res2.WriteByte(client.Character.movementAnim);//JUMP & FALLING ANIM

                Router.Send(client.Map, (ushort)AreaPacketId.recv_0x8D92, res2, ServerType.Area, client);

                client.Character.battleAnim = 0; //re-setting the byte to 0 at the end of every iteration to allow for normal movements.






            //Uncomment for debugging movement. causes heavy console output. recommend commenting out "Packet" method in NecLogger.CS when debugging movement
            if (movementSpeed != 0)
            {
                Logger.Debug($"Character {client.Character.Name} is in map {client.Character.MapId} @ : X[{client.Character.X}]Y[{client.Character.Y}]Z[{client.Character.Z}]");
                Logger.Debug($"X Axis Aligned : {percentMovementIsX.ToString("P", CultureInfo.InvariantCulture)} | Y Axis Aligned  : {percentMovementIsY.ToString("P", CultureInfo.InvariantCulture)}");
                Logger.Debug($"vertical Speed multi : {verticalMovementSpeedMultiplier}| Move Speed {movementSpeed} | Horizontal Speed Multi {horizontalMovementSpeedMultiplier}");
                Logger.Debug($"Movement Type[{client.Character.movementPose}]  Type Anim [{client.Character.movementAnim}] View Offset:{client.Character.Heading}");
                Logger.Debug($"---------------------------------------------------------------");

                //Logger.Debug($"Var 1 {(byte)(percentMovementIsX*255)} |Var 2 {(byte)(percentMovementIsY*255)}  ");
                //Logger.Debug($" Y to Map Y[{d}][{e}][{e1}][{f} |  and {percentMovementIsY}");
            }
            else
            {
                Logger.Debug($"Movement Stop Reset");
                Logger.Debug($"---------------------------------------------------------------");
            }



///////////
/////////-----ToDO:  Find a home for the commands below this line as solutions develop.  Do not Delete!
///////////




            if (client.Character.takeover == true)
            {
                Logger.Debug($"Moving object ID {client.Character.eventSelectReadyCode}.  i is {i}");
                IBuffer res = BufferProvider.Provide();
                IBuffer res3 = BufferProvider.Provide();


                res.WriteInt32(client.Character.eventSelectReadyCode);
                res.WriteFloat(client.Character.X);
                res.WriteFloat(client.Character.Y);
                res.WriteFloat(client.Character.Z);
                res.WriteByte(client.Character.Heading); //Heading
                res.WriteByte((byte)i);//state
                i++;
                if (i == 255) i = 0;
                
                Router.Send(client, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                Router.Send(client, (ushort)AreaPacketId.recv_object_point_move_r, res3, ServerType.Area);


            }
            //CheckMapChange(client);

        }

        private void CheckMapChange(NecClient client)
        {
            switch (client.Character.MapId)
            {
                case 1001001:
                    if ((client.Character.X < 4842.5 && client.Character.X > 4282) && client.Character.Y > 4448)
                    {
                        Map map = Server.Maps.Get(1001004);
                        map.X = 1;
                        map.Y = 1;
                        map.Z = 1;
                        map.Orientation = 0;
                        map.EnterForce(client);
                    }
                    else if ((client.Character.X < 225 && client.Character.X > 50) && client.Character.Y > 10200)
                    {
                        Map map = Server.Maps.Get(1001007);
                        map.X = -5622;
                        map.Y = -5874;
                        map.Z = 1;
                        map.Orientation = 93;
                        map.EnterForce(client);
                    }
                    else if (client.Character.X > 6800 && (client.Character.Y > 945 && client.Character.Y < 1723))
                    {
                        Map map = Server.Maps.Get(1001902);
                        map.X = 22697;
                        map.Y = -180;
                        map.Z = 5;
                        map.Orientation = 132;
                        map.EnterForce(client);
                    }
                    break;
                case 1001002:
                case 1001902:
                    if (client.Character.X < 21797 && (client.Character.Y > -755 && client.Character.Y < 485))
                    {
                        Map map = Server.Maps.Get(1001001);
                        map.X = 6700;
                        map.Y = 1452;
                        map.Z = -3;
                        map.Orientation = 51;
                        map.EnterForce(client);
                    }
                    else if ((client.Character.X > 36246 && client.Character.X < 37254) && client.Character.Y > 5313)
                    {
                        Map map = Server.Maps.Get(1001003);
                        map.X = 3701;
                        map.Y = -7057;
                        map.Z = 5;
                        map.Orientation = 0;
                        map.EnterForce(client);
                    }
                    break;
                case 1001003:
                    if ((client.Character.X < 3926 && client.Character.X > 3518) && client.Character.Y < -7511)
                    {
                        Map map = Server.Maps.Get(1001902);
                        map.X = 36638;
                        map.Y = 5216;
                        map.Z = -10;
                        map.Orientation = 87;
                        map.EnterForce(client);
                    }
                    break;
                case 1001004:
                    if ((client.Character.X < 1046 && client.Character.X > -1062) && client.Character.Y > 5300)
                    {
                        Map map = Server.Maps.Get(1001009);
                        map.X = -410;
                        map.Y = -859;
                        map.Z = 68;
                        map.Orientation = 0;
                        map.EnterForce(client);
                    }
                    else if (client.Character.X < -413 && (client.Character.Y > -712 && client.Character.Y < -345))
                    {
                        Map map = Server.Maps.Get(1001001);
                        map.X = 4243;
                        map.Y = 4492;
                        map.Z = 405;
                        map.Orientation = 67;
                        map.EnterForce(client);
                    }
                    break;
                case 1001005:
                    break;
                case 1001006:
                    break;
                case 1001007:
                    if ((client.Character.X < -5400 && client.Character.X > -5845) && client.Character.Y < -6288)
                    {
                        Map map = Server.Maps.Get(1001001);
                        map.X = 159;
                        map.Y = 9952;
                        map.Z = 601;
                        map.Orientation = 46;
                        map.EnterForce(client);
                    }
                    break;
                case 1001008:
                    break;
                case 1001009:
                    break;
                default:
                    break;
            }
        }
    }
}
