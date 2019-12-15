namespace Necromancy.Server.Data.Setting
{
    public class ModelAtrSetting : ISettingRepositoryItem
    {
        public int Id { get; set; }
        public float NormalMagnification { get; set; }
        public float CrouchingMagnification { get; set; }
        public float SittingMagnification { get; set; }
        public float RollingMagnification { get; set; }
        public float DeathMagnification { get; set; }
        public float MotionMagnification { get; set; }
    }
}
