using System;

namespace Necromancy.Server.Model
{
    public class ShortcutBar
    {
        public int Id { get; set; }
        public int Slot0 { get; set; }
        public int Slot1 { get; set; }
        public int Slot2 { get; set; }
        public int Slot3 { get; set; }
        public int Slot4 { get; set; }
        public int Slot5 { get; set; }
        public int Slot6 { get; set; }
        public int Slot7 { get; set; }
        public int Slot8 { get; set; }
        public int Slot9 { get; set; }

        public int Action0 { get; set; }
        public int Action1 { get; set; }
        public int Action2 { get; set; }
        public int Action3 { get; set; }
        public int Action4 { get; set; }
        public int Action5 { get; set; }
        public int Action6 { get; set; }
        public int Action7 { get; set; }
        public int Action8 { get; set; }
        public int Action9 { get; set; }
        public ShortcutBar()
        {
            Id = -1;
            Slot0 = -1;
            Slot1 = -1;
            Slot2 = -1;
            Slot3 = -1;
            Slot4 = -1;
            Slot5 = -1;
            Slot6 = -1;
            Slot7 = -1;
            Slot8 = -1;
            Slot9 = -1;
            Action0 = -1;
            Action1 = -1;
            Action2 = -1;
            Action3 = -1;
            Action4 = -1;
            Action5 = -1;
            Action6 = -1;
            Action7 = -1;
            Action8 = -1;
            Action9 = -1;
        }

        public int[] getSlotArray()
        {
            int[] shortcutBar = new int[10];
            shortcutBar[0] = Slot0;
            shortcutBar[1] = Slot1;
            shortcutBar[2] = Slot2;
            shortcutBar[3] = Slot3;
            shortcutBar[4] = Slot4;
            shortcutBar[5] = Slot5;
            shortcutBar[6] = Slot6;
            shortcutBar[7] = Slot7;
            shortcutBar[8] = Slot8;
            shortcutBar[9] = Slot9;
            return shortcutBar;
        }
        public void setSlotArray(int [] shortcutBar)
        {
            Slot0 = shortcutBar[0];
            Slot1 = shortcutBar[1];
            Slot2 = shortcutBar[2];
            Slot3 = shortcutBar[3];
            Slot4 = shortcutBar[4];
            Slot5 = shortcutBar[5];
            Slot6 = shortcutBar[6];
            Slot7 = shortcutBar[7];
            Slot8 = shortcutBar[8];
            Slot9 = shortcutBar[9];
        }
        public int[] getActionArray()
        {
            int[] action = new int[10];
            action[0] = Action0;
            action[1] = Action1;
            action[2] = Action2;
            action[3] = Action3;
            action[4] = Action4;
            action[5] = Action5;
            action[6] = Action6;
            action[7] = Action7;
            action[8] = Action8;
            action[9] = Action9;
            return action;
        }
        public void setActionArray(int[] action)
        {
            Action0 = action[0];
            Action1 = action[1];
            Action2 = action[2];
            Action3 = action[3];
            Action4 = action[4];
            Action5 = action[5];
            Action6 = action[6];
            Action7 = action[7];
            Action8 = action[8];
            Action9 = action[9];
        }
    }
}
