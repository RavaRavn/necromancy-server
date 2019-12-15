namespace Necromancy.Server.Data.Setting
{
    public class MonsterCsvReader : CsvReader<MonsterSetting>
    {
        protected override int NumExpectedItems => 12;

        protected override MonsterSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            if (!int.TryParse(properties[3], out int catalogId))
            {
                return null;
            }
            
            if (!TryParseNullableInt(properties[4], out int? effectId))
            {
                return null;
            }
            
            if (!TryParseNullableInt(properties[5], out int? activeEffectId))
            {
                return null;
            }

            if (!TryParseNullableInt(properties[6], out int? inactiveEffectId))
            {
                return null;
            }

            if (!TryParseNullableInt(properties[8], out int? modelSwitching))
            {
                return null;
            }

            if (!int.TryParse(properties[9], out int attackSkillId))
            {
                return null;
            }


            if (!int.TryParse(properties[10], out int level))
            {
                return null;
            }

            if (!int.TryParse(properties[11], out int combatMode))
            {
                return null;
            }


            if (!int.TryParse(properties[11], out int loadingModel))
            {
                return null;
            }

            return new MonsterSetting
            {
                Id = id,
                Name = properties[1],
                Title = properties[2],
                CatalogId = catalogId,
                EffectId = effectId,
                ActiveEffectId = activeEffectId,
                InactiveEffectId = inactiveEffectId,
                NamePlateType = properties[7],
                ModelSwitching = modelSwitching,
                AtackSkillId = attackSkillId,
                Level = level,
                CombatMode = combatMode == 1,
                LoadingModel = loadingModel == 1
            };
        }
    }
}
