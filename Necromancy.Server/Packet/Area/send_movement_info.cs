using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.IO;

namespace Necromancy.Server.Packet.Area
{
    public class send_movement_info : Handler
    {
        public send_movement_info(NecServer server) : base(server)
        {

        }

        public override ushort Id => (ushort)AreaPacketId.send_movement_info;



        public override void Handle(NecClient client, NecPacket packet)
        {


            if (client.Character != null)
            {
                client.Character.X = packet.Data.ReadFloat();
                client.Character.Y = packet.Data.ReadFloat();
                client.Character.Z = packet.Data.ReadFloat();


                //these bytes below with a varible as a name have not been confirmed this is for testing
                byte a = packet.Data.ReadByte();

                byte b = packet.Data.ReadByte();
                byte b1 = packet.Data.ReadByte();

                byte c = packet.Data.ReadByte(); // some sort of direction modified var

                byte d = packet.Data.ReadByte();

                byte e = packet.Data.ReadByte();
                byte e1 = packet.Data.ReadByte(); // direction char is moving based from the view of camera? if that makes sense

                byte f = packet.Data.ReadByte(); // also some sort of direction modified var

                int Incline = packet.Data.ReadInt32();
                
                byte h = packet.Data.ReadByte();

                byte i = packet.Data.ReadByte();

                byte j = packet.Data.ReadByte();

                byte k = packet.Data.ReadByte();
                byte k1 = packet.Data.ReadByte();
                byte k2 = packet.Data.ReadByte();
                byte k3 = packet.Data.ReadByte();

                byte l = packet.Data.ReadByte();

                client.Character.movementAnim = packet.Data.ReadByte();

                client.Character.animJumpFall = packet.Data.ReadByte();

                // the game divides the normal 360 radius by 2. giving view direction only 1-180

                if (client.Character.viewOffset <= 180) // NORTH
                {
                    client.Character.wepEastWestAnim = 0;
                    client.Character.wepNorthSouthAnim = 127;

                    if (client.Character.viewOffset <= 168.75) // SOUTH-EAST
                    {
                        client.Character.wepEastWestAnim = 127;
                        client.Character.wepNorthSouthAnim = 127;

                        if (client.Character.viewOffset <= 146.25) // EAST
                        {
                            client.Character.wepEastWestAnim = 127;
                            client.Character.wepNorthSouthAnim = 0;

                            if (client.Character.viewOffset <= 123.75) // SOUTH-EAST
                            {
                                client.Character.wepEastWestAnim = 127;
                                client.Character.wepNorthSouthAnim = 128;

                                if (client.Character.viewOffset <= 101.25) // SOUTH
                                {
                                    client.Character.wepEastWestAnim = 0;
                                    client.Character.wepNorthSouthAnim = 128;

                                    if (client.Character.viewOffset <= 78.75) // SOUTH-WEST
                                    {
                                        client.Character.wepEastWestAnim = 128;
                                        client.Character.wepNorthSouthAnim = 128;

                                        if (client.Character.viewOffset <= 56.25) // WEST
                                        {
                                            client.Character.wepEastWestAnim = 128;
                                            client.Character.wepNorthSouthAnim = 0;

                                            if (client.Character.viewOffset <= 33.75) // NORTH-WEST
                                            {
                                                client.Character.wepEastWestAnim = 128;
                                                client.Character.wepNorthSouthAnim = 127;

                                                if (client.Character.viewOffset <= 11.25) // NORTH
                                                {
                                                    client.Character.wepEastWestAnim = 0;
                                                    client.Character.wepNorthSouthAnim = 127;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                //int zz = (e1*-1)+e1;
                //byte zzz = (byte)zz;
                if (client.Character.weaponEquipped == false)
                {
                    client.Character.wepEastWestAnim = f;
                    client.Character.wepNorthSouthAnim = e1;
                }

                /*
                if (client.Character.weaponEquipped == true)
                {
                    client.Character.wepEastWestAnim = f;
                    client.Character.wepNorthSouthAnim = e1;
                }
                */

                // how to sideways walk ^ 127-127 is character left // 128-128 is character right
                {
                    // for (byte xd = 0; xd < 255; xd++)
                    {

                        IBuffer res2 = BufferProvider.Provide();

                        res2.WriteInt32(client.Character.Id);//Character ID
                        res2.WriteFloat(client.Character.X);
                        res2.WriteFloat(client.Character.Y);
                        res2.WriteFloat(client.Character.Z);

                        res2.WriteByte(client.Character.wepEastWestAnim); //HANDLES EAST AND WEST ANIMS WITH WEAPONS
                        res2.WriteByte(client.Character.wepNorthSouthAnim);// // HANDLES NORTH AND SOUTH ANIMS WITH WEAPONS
                        res2.WriteByte(255); // I DUNNO BUT IT WORKS

                        res2.WriteInt16(0xFFFF); //FIXES MOVEMENT LAG

                        res2.WriteByte(0);// DONT TOUCH >.> CAUSES VISUAL TELEPORTING
                        res2.WriteByte(client.Character.movementAnim); //MOVEMENT ANIM
                        res2.WriteByte(client.Character.animJumpFall);//JUMP & FALLING ANIM




                        Router.Send(client.Map, (ushort)AreaPacketId.recv_0xE8B9, res2, client);


                        //System.Threading.Thread.Sleep(1000);
                    }

                    IBuffer res = BufferProvider.Provide();

                    res.WriteInt32(client.Character.Id);//Character ID
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.viewOffset);//View offset
                    res.WriteByte(0);//Character state?

                    Router.Send(client.Map, (ushort)AreaPacketId.recv_0x6B6A, res, client);

                   // Router.Send(client.Map, (ushort)AreaPacketId.recv_object_point_move_notify, res, client);


                }
            }
        }
    }
}

/*
  SAMPLE -  [REMOVED CORDS, AND THE LAST 7 BYTES AS THEY DID NOT CHANGE OVER MY TEST DATA]

-a -b -b1-c -d -e -e1-f -[incline]-h -i -j -    Varibles to position of packet

-AE-4A-A3-BD-5A-2F-7F-BF-[incline]-86-01-75-
-76-51-A3-BD-49-2F-7F-BF-[incline]-61-F8-06-	Running Sideways
-53-C9-A2-BD-A4-30-7F-BF-[incline]-C6-0A-B1-

-0C-4D-67-BD-6D-97-7F-3F-[incline]-88-A1-91-
-F0-92-5D-BE-63-EF-79-3F-[incline]-87-00-E1-	Forward running
-7D-8A-5D-BE-DB-EF-79-3F-[incline]-6B-00-E1-
-F0-92-5D-BE-63-EF-79-3F-[incline]-87-00-E1-
                                                running in said directions below
-8C-93-7D-3F-7C-92-0C-BE-[incline]-D9-C2-F0-  e

-94-E6-90-3D-C2-5B-7F-BF-[incline]-E3-EB-C0-  s 

-04-F8-7F-BF-B4-BD-7F-BC-[incline]-E6-DE-C4-  w

-4A-D1-1F-BE-E6-DC-7C-3F-[incline]-32-E7-CB-  n

-07-97-EF-3E-CA-3D-62-3F-[incline]-37-DB-B2-  ne

-CE-E1-51-3F-56-94-12-BF-[incline]-16-65-20-  se

-53-6A-35-BF-59-9F-34-BF-[incline]-14-8B-CE-  sw

-6E-D9-28-BF-D1-6B-40-3F-[incline]-17-6C-BB-  nw


*/
