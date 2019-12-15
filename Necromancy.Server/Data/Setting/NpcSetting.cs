namespace Necromancy.Server.Data.Setting
{
    public class NpcSetting: ISettingRepositoryItem
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }
}
