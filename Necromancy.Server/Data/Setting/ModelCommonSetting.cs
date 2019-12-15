namespace Necromancy.Server.Data.Setting
{
    public class ModelCommonSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public int Radius { get; set; }
        public int Height { get; set; }
        public int CrouchHeight { get; set; }
        public int NameHeight { get; set; }
        public ModelAtrSetting Atr { get; set; }
        public int ZRadiusOffset { get; set; }
        public int Effect { get; set; }
        public int Active { get; set; }

        /// <summary>
        /// Developer Comment
        /// </summary>
        public string Remarks { get; set; }

        public MonsterSetting Monster { get; set; }
    }
}
