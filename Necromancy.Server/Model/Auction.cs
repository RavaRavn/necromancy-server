using System;
using System.Collections.Generic;
using System.Text;


namespace Necromancy.Server.Model
{
    public class Auction
    {
        public int TypeId { get; set; } // Bid or Rebid
        public int SlotsId { get; set; }
        public Int64 unknown { get; set; }
        public int Lowest { get; set; }
        public int BuyNow { get; set; }
        public string Name { get; set; }
        public byte unknown1 { get; set; }
        public string Comment { get; set; }
        public Int16 Bid { get; set; }
        public int Timer { get; set; }
        public int BidAmount { get; set; } // Bid Price Or Bid Amount
        public int Statuses { get; set; }

        public Auction()
        {
            SlotsId = -1;
        }
    }
}
