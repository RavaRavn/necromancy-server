using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Common
{
    class LoadEquip
    {
        static int numEntries = 19;
        public static void BasicTraits(IBuffer res, Character myCharacter)
        {
            res.WriteInt32(myCharacter.Raceid); //race
            res.WriteInt32(myCharacter.Sexid); ; //gender
            res.WriteByte(myCharacter.HairId); //hair
            res.WriteByte(myCharacter.HairColorId); //color
            res.WriteByte(myCharacter.FaceId); //face
        }

            
            public static void SlotSetup( IBuffer res, Character myCharacter)
            {
                
                TemporaryCharacterSwitch(myCharacter); //needed to instantiate Weapon ID for Weapon logic below
                int Armor = 25;         //Armor 25
                int Accessory = 26;     //Accessory 26
                int Shield = 21;        //Shield 19-21
                int Quiver = 22;        //Arrows 22 Bolt 23 Bullet 24
                int Other = 27;         //None of the Above
                int Weapon =  Convert.ToInt32(string.Concat($"{(myCharacter.EquipId[0])}"[1],$"{(myCharacter.EquipId[0])}"[2]));
                myCharacter.battlePose = (byte)Weapon;
                //Console.WriteLine($"Weapon : {Weapon}");
                
                //ItemType Select See str_table ID 100 SubID 121 for Item Type info. Increment by +1
                int[] EquipItemType = new int[]
                    {Weapon, Shield, Quiver, Armor, Armor, Armor, Armor, Armor, Other/*Cape-Other*/, Accessory, Accessory, Accessory, Accessory, Other/*TalkRing-Other*/, Armor, Armor, Armor, Armor, Armor};
                
                //sub_483660 
                for (int i = 0; i < numEntries; i++)
                {
                    res.WriteInt32(EquipItemType[i]);
                }

            }

        public static void EquipItems(IBuffer res, Character myCharacter)
        {
            //sub_483420
            int x = 0;
            myCharacter.EquipId = new int[numEntries];
            

            byte[] headSlot = TemporaryCharacterSwitch(myCharacter);
            
            //sub_4948C0
            for (int i = 0; i < numEntries; i++)
            {

                res.WriteInt32(myCharacter.EquipId[x]);//Sets your Item ID per Iteration
                res.WriteByte(0);// 
                res.WriteByte(0);// (theory bag)
                res.WriteByte(0);// (theory Slot)

                res.WriteInt32(myCharacter.EquipId[x]);//testing (Theory, Icon related)
                res.WriteByte(0); //
                res.WriteByte(0);// (theory bag)
                res.WriteByte(0);// (theory Slot)

                res.WriteByte(headSlot[x]);// Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(0); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model 
                res.WriteByte(0); // separate in assembly
                x++;
            }

        }

        public static void EquipSlotBitMaskOld(IBuffer res, Character myCharacter)
        {
            int rr = 000;
            //sub_483420   // 2 shield 4accessory? 8Helmet 12belt? 16torso 32 pants 48torsopants 64 hands 96handpants 128 feet 192handfeet 
            res.WriteInt32(001); //Right Hand    //1 for weapon
            res.WriteInt32(002); //Left Hand     //2 for Shield
            res.WriteInt32(016); //Torso         //16 for torso
            res.WriteInt32(008); //Head          //08 for head
            res.WriteInt32(032); //Legs          //32 for legs
            res.WriteInt32(064); //Arms          //64 for Arms
            res.WriteInt32(128); //Feet          //128 for feet
            res.WriteInt32(004); //???Cape
            res.WriteInt32(rr); //???Ring
            res.WriteInt32(rr); //???Earring
            res.WriteInt32(rr); //???Necklace
            res.WriteInt32(rr); //???Belt
            res.WriteInt32(016); //Avatar Torso
            res.WriteInt32(128); //Avatar Feet
            res.WriteInt32(064); //Avatar Arms
            res.WriteInt32(032); //Avatar Legs
            res.WriteInt32(008); //Avatar Head  
            res.WriteInt32(004); //???Talk Ring
            res.WriteInt32(000); //???Quiver  
        }
        
        public static void EquipSlotBitMask(IBuffer res, Character myCharacter)
        {
            int[] EquipBitMask = new int[] //Correct Bit Mask
            {
                1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144,/* 524288,
                1048576, 2097152*/
            };
            EquipBitMask = new int[] //Temporary Bit Mask until i re-figure out Avatar Item Display Precedence.
            {
                1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 8, 16, 32, 64, 128
            };
            for (int i = 0; i < numEntries; i++)
            {
                //sub_483420   
                res.WriteInt32(EquipBitMask[i]); //bitmask per equipment slot
            }
            
        }

        private static byte[] TemporaryCharacterSwitch(Character myCharacter)
        {
            string CharacterSet = myCharacter.Name;
            byte[] headSlot = new byte[numEntries];

            switch (CharacterSet)
            {
                case "Talin":
                    myCharacter.EquipId = new int[]
                    {
                        10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/, 210701 /*Torso*/,
                        360103 /*Pants*/, 410505 /*Hands*/, 560103 /*Feet*/, 600101 /*Cape*/, 30300101 /*Necklace*/,
                        30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 110301 /*Avatar Head */,
                        210701 /*Avatar Torso*/, 360103 /*Avatar Pants*/, 410505 /*Avatar Hands*/, 560103 /*Avatar Feet*/
                    };
                    headSlot = new byte[19] { 0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0 };
                    break;
                case "Kadred":
                    myCharacter.EquipId = new int[]
                    {
                        10100301 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/, 260103 /*Torso*/,
                        360103 /*Pants*/, 460103 /*Hands*/, 510301 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                        30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 160801 /*Avatar Head */,
                        260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 560801 /*Avatar Feet*/
                    };
                    headSlot = new byte[19] { 0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0, 0, 0 };
                    break;
                case "Zakura":
                    myCharacter.EquipId = new int[]
                    {
                        11400303 /*Weapon*/, 0 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/, 260103 /*Torso*/,
                        360103 /*Pants*/, 460103 /*Hands*/, 510301 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                        30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 100403 /*Avatar Head */,
                        260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 510301 /*Avatar Feet*/
                    };
                    headSlot = new byte[19] { 0, 0, 0, 42, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 42, 0, 0, 0, 0 };
                    break;
                case "Test1":
                    myCharacter.EquipId = new int[]
                    {
                        11500102 /*Weapon*/, 15100801 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/, 260103 /*Torso*/,
                        360103 /*Pants*/, 460103 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                        30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 161101 /*Avatar Head */,
                        261101 /*Avatar Torso*/, 361101 /*Avatar Pants*/, 461101 /*Avatar Hands*/, 561101 /*Avatar Feet*/
                    };
                    headSlot = new byte[19] { 0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0 };
                    break;
                case "One":
                    myCharacter.EquipId = new int[]
                    {
                        10800405 /*Weapon*/, 15100901 /*Shield* */, 20000101 /*Arrow*/, 110301 /*head*/, 210701 /*Torso*/,
                        360103 /*Pants*/, 401201 /*Hands*/, 560103 /*Feet*/, 690101 /*Cape*/, 30300101 /*Necklace*/,
                        30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 160801 /*Avatar Head */,
                        260801 /*Avatar Torso*/, 360801 /*Avatar Pants*/, 460801 /*Avatar Hands*/, 560801 /*Avatar Feet*/
                    };
                    headSlot = new byte[19] { 0, 0, 0, 00, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 00, 0, 0, 0, 0 };
                    break;
                default:
                    myCharacter.EquipId = new int[]
                    {
                        10800405 /*Weapon*/, 15200702 /*Shield* */, 20000101 /*Arrow*/, 110504 /*head*/, 260103 /*Torso*/,
                        360103 /*Pants*/, 460103 /*Hands*/, 560103 /*Feet*/, 600101 /*Cape*/, 30300101 /*Necklace*/,
                        30200107 /*Earring*/, 30400105 /*Belt*/, 30100106 /*Ring*/, 70000101 /*Talk Ring*/, 161401 /*Avatar Head */,
                        261401 /*Avatar Torso*/, 361401 /*Avatar Pants*/, 461401 /*Avatar Hands*/, 561401 /*Avatar Feet*/
                    };
                    headSlot = new byte[19] { 0, 0, 0, 004, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 04, 0, 0, 0, 0 };
                    break;
                    
            }
            
            return headSlot;

        }
        
    }

}


