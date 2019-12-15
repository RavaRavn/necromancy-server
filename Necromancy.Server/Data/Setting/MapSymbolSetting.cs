namespace Necromancy.Server.Data.Setting
{
    public class MapSymbolSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public int Map { get; set; }
        public int DisplayConditionflag { get; set; }
        public int SplitMapNumber { get; set; }
        public int SettingTypeFlag { get; set; }
        public int SettingIdOrText { get; set; }
        public int IconType { get; set; }
        public int DisplayPositionX { get; set; }
        public int DisplayPositionY { get; set; }
        public int DisplayPositionZ { get; set; }

    }
}
