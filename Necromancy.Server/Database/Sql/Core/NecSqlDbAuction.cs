using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlCreateAuction =
            "INSERT INTO `Auction` (`TypeId`, `SlotsId`, `unknown`, `Lowest`, `BuyNow`, `Name`, `unknown1`, `Comment`, `Bid`, `Timer`, `BidAmount`, `Statuses`) VALUES (@TypeId, @SlotsId, @unknown, @Lowest, @BuyNow, @Name, @unknown1, @Comment, @Bid, @Timer, @BidAmount, @Statuses);";

        private const string SqlSelectAuctionById =
            "SELECT `TypeId`, `SlotsId`, `unknown`, `Lowest`, `BuyNow`, `Name`, `unknown1`, `Comment`, `Bid`, `Timer`, `BidAmount`, `Statuses` FROM `Auction` WHERE `TypeId`=@TypeId; ";

        private const string SqlUpdateAuction =
            "UPDATE `Auction` SET `TypeId`=@TypeId, `SlotsId`=@SlotsId, `unknown`=@unknown,  `Lowest`=@Lowest, `BuyNow`=@BuyNow, `Name`=@Name, `unknown1`=@unknown1, `Comment`=@Comment, `Bid`=@Bid, `Timer`=@Timer, `BidAmount`=@BidAmount, `Statuses`=@Statuses WHERE `TypeId`=@TypeId;";

        private const string SqlDeleteAuction =
            "DELETE FROM `Auction` WHERE `TypeId`=@TypeId;";

        public bool InsertAuction(Auction Auction)
        {
            int rowsAffected = ExecuteNonQuery(SqlCreateAuction, command =>
            {
                AddParameter(command, "@TypeId", Auction.TypeId);
                AddParameter(command, "@SlotsId", Auction.SlotsId);
                AddParameter(command, "@unknown", Auction.unknown);
                AddParameter(command, "@Lowest", Auction.Lowest);
                AddParameter(command, "@BuyNow", Auction.BuyNow);
                AddParameter(command, "@Name", Auction.Name);
                AddParameter(command, "@unknown1", Auction.unknown1);
                AddParameter(command, "@Comment", Auction.Comment);
                AddParameter(command, "@Bid", Auction.Bid);
                AddParameter(command, "@Timer", Auction.Timer);
                AddParameter(command, "@BidAmount", Auction.BidAmount);
                AddParameter(command, "@Statuses", Auction.Statuses);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            Auction.TypeId = (int)autoIncrement;
            return true;
        }


        public Auction SelectAuctionById(int AuctionId)
        {
            Auction Auction = null;
            ExecuteReader(SqlSelectAuctionById,
                command => { AddParameter(command, "@TypeId", AuctionId); }, reader =>
                {
                    if (reader.Read())
                    {
                        Auction = ReadAuction(reader);
                    }
                });
            return Auction;
        }

        public bool UpdateAuction(Auction Auction)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateAuction, command =>
            {
                AddParameter(command, "@TypeId", Auction.TypeId);
                AddParameter(command, "@SlotsId", Auction.SlotsId);
                AddParameter(command, "@unknown", Auction.unknown);
                AddParameter(command, "@Lowest", Auction.Lowest);
                AddParameter(command, "@BuyNow", Auction.BuyNow);
                AddParameter(command, "@Name", Auction.Name);
                AddParameter(command, "@unknown1", Auction.unknown1);
                AddParameter(command, "@Comment", Auction.Comment);
                AddParameter(command, "@Bid", Auction.Bid);
                AddParameter(command, "@Timer", Auction.Timer);
                AddParameter(command, "@BidAmount", Auction.BidAmount);
                AddParameter(command, "@Statuses", Auction.Statuses);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteAuction(int AuctionId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteAuction,
                command => { AddParameter(command, "@TypeId", AuctionId); });
            return rowsAffected > NoRowsAffected;
        }

        private Auction ReadAuction(DbDataReader reader)
        {
            Auction Auction = new Auction();
            Auction.TypeId = GetInt32(reader, "TypeId");
            Auction.SlotsId = GetInt32(reader, "SlotsId");
            Auction.unknown = (byte)GetInt32(reader, "unknown");
            Auction.Lowest = GetInt32(reader, "Lowest");
            Auction.BuyNow = GetInt32(reader, "BuyNow");
            Auction.Name = GetString(reader, "Name");
            Auction.unknown1 = (byte)GetInt32(reader, "unknown1");
            Auction.Comment = GetString(reader, "Comment");
            Auction.Bid = GetInt16(reader, "Bid");
            Auction.Timer = GetInt32(reader, "Timer");
            Auction.BidAmount = GetInt32(reader, "BidAmount");
            Auction.Statuses = GetInt32(reader, "Statuses");
            return Auction;
        }
    }
}
