using Necromancy.Server.Model;

namespace Necromancy.Server.Data.Setting
{
    public class ItemInfoCsvReader : CsvReader<ItemSetting>
    {
        protected override int NumExpectedItems => 2;

        protected override ItemSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            return new ItemSetting
            {
                Id = id,
                Name = properties[1]
            };
        }
    }
}
