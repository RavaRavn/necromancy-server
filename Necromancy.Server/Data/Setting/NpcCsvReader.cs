namespace Necromancy.Server.Data.Setting
{
    public class NpcCsvReader : CsvReader<NpcSetting>
    {
        protected override int NumExpectedItems => 15;

        protected override NpcSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            if (!int.TryParse(properties[12], out int level))
            {
                return null;
            }

            return new NpcSetting
            {
                Id = id,
                Level = level,
                Name = properties[1],
                Title = properties[13]
            };
        }
    }
}
