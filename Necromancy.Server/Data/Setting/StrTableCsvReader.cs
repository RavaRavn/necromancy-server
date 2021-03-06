namespace Necromancy.Server.Data.Setting
{
    public class StrTableCsvReader : CsvReader<StrTableSetting>
    {
        protected override int NumExpectedItems => 4;

        protected override StrTableSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            if (!int.TryParse(properties[1], out int subId))
            {
                return null;
            }

            if (!int.TryParse(properties[2], out int stringId))
            {
                return null;
            }

            return new StrTableSetting
            {
                Id = id,
                SubId = subId,
                StringId = stringId,
                Text = properties[3]
            };
        }
    }
}
