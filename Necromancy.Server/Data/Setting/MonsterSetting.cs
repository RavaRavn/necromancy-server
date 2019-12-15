namespace Necromancy.Server.Data.Setting
{
    public class MonsterSetting: ISettingRepositoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int CatalogId { get; set; }
        public int? EffectId { get; set; }
        public int? ActiveEffectId { get; set; }
        public int? InactiveEffectId { get; set; }
        public string NamePlateType { get; set; }
        public int? ModelSwitching { get; set; }
        public int AtackSkillId { get; set; }
        public int Level { get; set; }
        public bool CombatMode { get; set; }
        public bool LoadingModel { get; set; }
    }
}
