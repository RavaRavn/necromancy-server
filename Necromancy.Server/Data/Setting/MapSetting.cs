namespace Necromancy.Server.Data.Setting
{
    public class MapSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Orientation { get; set; }
    }
}
